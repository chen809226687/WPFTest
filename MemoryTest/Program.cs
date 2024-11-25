using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using sinsegye.acpsharp.acp;
using sinsegye.acpsharp.plcopen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

class Program
{
    static void Main()
    {

        var structvar = ReadStruct("C:\\Users\\80922\\Desktop\\ACP结构体解析\\StructTianzhun.json", "Application.GVL.TEST", "StrPLCWriteToPC");

    }


    static object ReadStruct(string path, string nodename, string name, int length = 1)
    {
        var structDefinition = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(path));
        var types = new Dictionary<string, Type>();
        var assemblyName = new AssemblyName("DynamicStructAssembly");
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");

     




        foreach (var property in structDefinition)
        {
            var jsonValue = JToken.FromObject(property.Value);
            CreateTypeFromJson(property.Key, jsonValue.ToObject<Dictionary<string, object>>(), moduleBuilder, types);
        }


        //计算偏移量


        foreach (var typeEntry in types)
        {
            int size = CalculateStructSize(typeEntry.Value);
            Console.WriteLine($"结构体类型: {typeEntry.Key}, 大小: {size} 字节");
            PrintFieldOffsets(typeEntry.Value);
        }


        AcpClient _acpClient = new AcpClient("192.168.110.8.1.1", 600);
        byte[] bytes;
        string str;
        var error = _acpClient.ReadVar(nodename, out bytes, out str);


        // 反序列化为结构体实例
        var instance = DeserializeFromBytes(bytes, types[name], length);
        return instance;
    }



    static Type CreateTypeFromJson(string name, Dictionary<string, object> jsonDefinition, ModuleBuilder moduleBuilder, Dictionary<string, Type> types)
    {
        if (types.ContainsKey(name)) return types[name];

        var typeBuilder = moduleBuilder.DefineType(name, TypeAttributes.Public | TypeAttributes.SequentialLayout | TypeAttributes.Sealed, typeof(ValueType));

        foreach (var field in jsonDefinition)
        {
            string fieldName = field.Key;

            if (field.Value is JObject nestedDict)
            {
                var nestedDefinition = nestedDict.ToObject<Dictionary<string, object>>();
                Type nestedType = CreateTypeFromJson(fieldName, nestedDefinition, moduleBuilder, types);
                typeBuilder.DefineField(fieldName, nestedType, FieldAttributes.Public);
            }
            else if (field.Value is string strValue)
            {
                Type resolvedType = strValue switch
                {
                    "int" => typeof(int),
                    "byte" => typeof(byte),
                    "INT16" => typeof(Int16),
                    "INT64" => typeof(Int64),
                    "ushort" => typeof(ushort),
                    "double" => typeof(double),
                    "bool" => typeof(bool),
                    "string" => typeof(StringWithLength), // 使用包装类
                    _ when types.ContainsKey(strValue) => types[strValue],
                    _ => throw new NotSupportedException($"不支持的类型: {strValue}")
                };
                typeBuilder.DefineField(fieldName, resolvedType, FieldAttributes.Public);
            }
            else
            {
                throw new NotSupportedException($"不支持的字段类型: {field.Value.GetType()}");
            }
        }

        var createdType = typeBuilder.CreateType();
        types[name] = createdType;
        Console.WriteLine($"定义的结构体类型: {name}");
        return createdType;
    }

    public static int CalculateStructSize(Type type)
    {
        int totalSize = 0;
        int maxAlignment = 1;

        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            Type fieldType = field.FieldType;
            int fieldSize;
            int alignment;

            if (fieldType.IsArray)
            {
                Type elementType = fieldType.GetElementType();
                // 假设数组长度为1，若有实际数组长度请传入
                fieldSize = CalculateStructSize(elementType) * 1; // 这里假设长度为1
                alignment = GetAlignment(elementType);
            }
            else if (fieldType.IsValueType && !fieldType.IsPrimitive)
            {
                fieldSize = CalculateStructSize(fieldType);
                alignment = GetAlignment(fieldType);
            }
            else
            {
                fieldSize = Marshal.SizeOf(fieldType);
                alignment = GetAlignment(fieldType);
            }

            totalSize = AlignOffset(totalSize, alignment);
            totalSize += fieldSize;

            maxAlignment = Math.Max(maxAlignment, alignment);
        }

        return AlignOffset(totalSize, maxAlignment);
    }

    static int GetAlignment(Type type)
    {
        return type.IsValueType ? Marshal.SizeOf(type) : IntPtr.Size;
    }

    static int AlignOffset(int offset, int alignment)
    {
        return (offset + alignment - 1) & ~(alignment - 1);
    }

    static void PrintFieldOffsets(Type type)
    {
        Console.WriteLine($"  {type.Name} 的字段偏移量:");
        int offset = 0;

        foreach (var field in type.GetFields())
        {
            int fieldSize = Marshal.SizeOf(field.FieldType);
            Console.WriteLine($"    字段 {field.Name}: 偏移量 {offset} 字节, 大小 {fieldSize} 字节");
            offset = AlignOffset(offset, GetAlignment(field.FieldType)) + fieldSize;
        }
    }

    static object DeserializeFromBytes(byte[] data, Type type, int length = 1)
    {
        if (!type.IsValueType)
        {
            throw new ArgumentException("结构体必须是值类型", nameof(type));
        }

        if (length > 1)
        {
            // 计算每个结构体的大小
            int elementSize = Marshal.SizeOf(type);

            // 创建结构体数组实例
            Array array = Array.CreateInstance(type, length);

            for (int i = 0; i < length; i++)
            {
                // 提取每个元素的字节
                byte[] elementData = new byte[elementSize];
                Array.Copy(data, i * elementSize, elementData, 0, elementSize);

                // 创建元素实例并填充
                IntPtr ptr = Marshal.AllocHGlobal(elementSize);
                try
                {
                    Marshal.Copy(elementData, 0, ptr, elementSize);
                    var elementInstance = Marshal.PtrToStructure(ptr, type);
                    array.SetValue(elementInstance, i); // 设置数组元素
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }

            return array; // 返回结构体数组
        }
        else
        {
            int size = Marshal.SizeOf(type);
            // 创建单个结构体的实例
            object instance = Activator.CreateInstance(type);
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(instance));
            try
            {
                Marshal.Copy(data, 0, ptr, data.Length);
                instance = Marshal.PtrToStructure(ptr, type);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }

            return instance; // 返回单个结构体实例
        }
    }



}

public struct StringWithLength
{
    public int Length;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)] // 假设最大长度为256
    public string Value;

    public StringWithLength(string value)
    {
        Length = value.Length;
        Value = value;
    }
}