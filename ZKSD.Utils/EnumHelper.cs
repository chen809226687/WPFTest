using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZKSD.Utils
{
   public static class EnumHelper
    {
        public static string GetEnumDescription(Enum enumValue)
        {
            string value = enumValue.ToString();
            FieldInfo field = enumValue.GetType().GetField(value);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);  //获取描述属性
            if (objs == null || objs.Length == 0)  //当描述属性没有时，直接返回名称
                return value;
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
            return descriptionAttribute.Description;
        }

        /// <summary>
        /// 根据字符串获取枚举值，如何解析失败使用枚举值的第一个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T GetEnumByStr<T>(string str) where T : struct
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException();
            Array a = System.Enum.GetValues(typeof(T));
            T r = (T)a.GetValue(0);
            try
            {
                if (System.Enum.TryParse(str, out T t))
                    r = t;
            }
            catch (Exception)
            {
            }
            return r;
        }
        /// <summary>
        /// 获取枚举描述列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<string> GetEnumDescriptions<T>()
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException();
            List<string> strs = new List<string>();
            Array a = System.Enum.GetValues(typeof(T));
            foreach (var item in a)
            {
                strs.Add(EnumHelper.GetEnumDescription(item));
            }
            return strs;
        }
        /// 获取枚举描述列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<string> GetEnumDescriptions(Type type)
        {
            if (!type.IsEnum)
                throw new ArgumentException();
            List<string> strs = new List<string>();
            Array a = System.Enum.GetValues(type);
            foreach (var item in a)
            {
                strs.Add(EnumHelper.GetEnumDescription(item));
            }
            return strs;
        }
        /// <summary>
        /// 获取枚举值上的Description特性的说明
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="obj">枚举值</param>
        /// <returns>特性的说明</returns>
        public static string GetEnumDescription<T>(T obj)
        {
            try
            {
                var type = obj.GetType();

                FieldInfo field = type.GetField(System.Enum.GetName(type, obj));
                if (!(Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute descAttr))
                {
                    return string.Empty;
                }
                return descAttr.Description;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string GetEnumDescriptionFromString<T>(string str)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException();
            FieldInfo[] fields = type.GetFields();
            var field = fields
                            .SelectMany(f => f.GetCustomAttributes(
                                typeof(DescriptionAttribute), false), (
                                    f, a) => new { Field = f, Att = a })
                            .Where(f => f.Field.Name == str).SingleOrDefault();
            return field == null ? "" : ((DescriptionAttribute)field.Att).Description;
        }


        public static T GetEnumValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException();
            FieldInfo[] fields = type.GetFields();
            var field = fields
                            .SelectMany(f => f.GetCustomAttributes(
                                typeof(DescriptionAttribute), false), (
                                    f, a) => new { Field = f, Att = a })
                            .Where(a => ((DescriptionAttribute)a.Att)
                                .Description == description).SingleOrDefault();
            return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
        }

        public static T GetEnumFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException();
            foreach (var field in type.GetFields())
            {
                DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute == null)
                    continue;
                if (attribute.Description == description)
                {
                    return (T)field.GetValue(null);
                }
            }
            return default(T);
        }


    }
}
