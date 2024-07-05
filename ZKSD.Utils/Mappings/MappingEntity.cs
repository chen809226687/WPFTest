/*********************************************************
* 项目名称：MappingEntity.cs
* 开发人员：liyan
* 公    司：聚时科技
* 创建时间：2021/11/30 11:38:28
* 更新时间：2021/11/30 11:38:28
* CLR版本 ：4.0.30319.42000
*
* 描述说明：通过属性反射实体
* 
* *******************************************************/

using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using static log4net.Appender.ColoredConsoleAppender;

namespace Utils.Mappings
{
    public static class MappingEntity
    {
        /// <summary>
        /// 两个实体属性赋值
        /// </summary>
        /// <typeparam name="S">源单类</typeparam>
        /// <typeparam name="T">需要转换的实体类</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T EntityConvert<S, T>(S source)
        {
            try
            {
                T target = Activator.CreateInstance<T>();
                var tType = source.GetType();
                var sType = typeof(T);
                foreach (PropertyInfo now in tType.GetProperties())
                {
                    var name = sType.GetProperty(now.Name);
                    if (name == null)
                        continue;
                    sType.GetProperty(now.Name).SetValue(target, now.GetValue(source));
                }
                return target;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        /// <summary>
        /// 两个实体属性赋值
        /// </summary>
        /// <typeparam name="S">源单类</typeparam>
        /// <typeparam name="T">需要转换的实体类</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T EntityConvertBySerialize<S, T>(S source) where T : class, new() where S : class, new()
        {
            try
            {
                if (source == null) return null;
                string json = JsonConvert.SerializeObject(source);
                T t = JsonConvert.DeserializeObject<T>(json);
                return t;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        /// <summary>
        /// 两个实体取交集，不同的属性用默认值代替
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source1"></param>
        /// <param name="source2"></param>
        /// <param name="defSource"></param>
        /// <param name="resT"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T GetEntityIntersection<T>(List<T> source, T defSource) where T : class, new()
        {
            try
            {
                if (source == null || source.Count == 0) return defSource;
                if (source.Count == 1) return source[0];
                var sType = typeof(T);
                T newSource = EntityConvertBySerialize<T, T>(source[0]);
                for (int i = 1; i < source.Count; i++)
                {
                    T source1;
                    T source2;
                    if (i == 1)
                    {
                        source1 = source[0];
                        source2 = source[1];
                    }
                    else
                    {
                        source1 = source[i];
                        source2 = newSource;
                    }
                    foreach (PropertyInfo pro in sType.GetProperties())
                    {
                        object val1 = sType.GetProperty(pro.Name).GetValue(source1);
                        object val2 = sType.GetProperty(pro.Name).GetValue(source2);
                        if (pro.PropertyType.IsClass && !typeof(string).Equals(pro.PropertyType))
                        {
                            Type childType = pro.PropertyType;
                            foreach (PropertyInfo proChild in childType.GetProperties())
                            {
                                object childVal1 = childType.GetProperty(proChild.Name).GetValue(val1);
                                object childVal2 = childType.GetProperty(proChild.Name).GetValue(val2);

                                if (childVal1 == null && childVal2 != null)
                                {
                                    object defVal = sType.GetProperty(pro.Name).GetValue(defSource);
                                    object resTVal = sType.GetProperty(pro.Name).GetValue(newSource);
                                    object childDefVal = childType.GetProperty(proChild.Name).GetValue(defVal);
                                    childType.GetProperty(proChild.Name).SetValue(resTVal, childDefVal);
                                }

                                if (childVal1 != null)
                                {
                                    if (!childVal1.Equals(childVal2))
                                    {
                                        object defVal = sType.GetProperty(pro.Name).GetValue(defSource);
                                        object resTVal = sType.GetProperty(pro.Name).GetValue(newSource);
                                        object childDefVal = childType.GetProperty(proChild.Name).GetValue(defVal);
                                        childType.GetProperty(proChild.Name).SetValue(resTVal, childDefVal);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (val1 == null && val2 != null)
                            {
                                sType.GetProperty(pro.Name).SetValue(newSource, sType.GetProperty(pro.Name).GetValue(defSource));
                            }

                            if (val1 != null)
                            {
                                if (!val1.Equals(val2))
                                {
                                    sType.GetProperty(pro.Name).SetValue(newSource, sType.GetProperty(pro.Name).GetValue(defSource));
                                }
                            }
                        }
                    }
                }

                return newSource;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        /// <summary>
        /// 两个实体属性赋值
        /// </summary>
        /// <typeparam name="S">源单类</typeparam>
        /// <typeparam name="T">需要转换的实体类</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static object EntityConvertBySerialize(object source, Type retType)
        {
            try
            {
                if (source == null) return null;
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                string json = JsonConvert.SerializeObject(source, settings);
                return JsonConvert.DeserializeObject(json, retType);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public static Tout EntityConvertByBinarySerialize<Tin, Tout>(Tin source) where Tin : class where Tout : class
        {
            try
            {
                if (source == null) return null;
                var bytes = BinarySerialize(source);
                return BinaryDeserialize<Tout>(bytes);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public static byte[] BinarySerialize(object obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Position = 0;
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, obj);
                return stream.GetBuffer();
            }
        }

        public static T BinaryDeserialize<T>(byte[] buffer)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(buffer, 0, buffer.Length);
                stream.Position = 0;
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }



        /// <summary>
        /// 两个实体属性赋值
        /// </summary>
        /// <typeparam name="S">需要转换的实体类</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static S EntityConvert<S>(object targetSource)
        {
            try
            {
                S target = Activator.CreateInstance<S>();
                var tType = targetSource.GetType();
                var sType = typeof(S);
                foreach (PropertyInfo now in tType.GetProperties())
                {
                    var name = sType.GetProperty(now.Name);
                    if (name == null)
                        continue;
                    sType.GetProperty(now.Name).SetValue(target, now.GetValue(targetSource));
                }
                return target;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// 通过属性名称获取属性值
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <typeparam name="P">属性类型</typeparam>
        /// <param name="property"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T GetValueByPropertyName<T, P>(P property, string propertyName)
        {
            try
            {
                var propNameVal = property.GetType().GetProperty(propertyName).GetValue(property, null);
                return (T)propNameVal;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// 通过属性名称获取属性值
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T GetValueByPropertyName<T>(object obj, string propertyName)
        {
            try
            {
                var propNameVal = obj.GetType().GetProperty(propertyName).GetValue(obj, null);
                return (T)propNameVal;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// 通过属性名称获取属性值
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <typeparam name="P">属性类型</typeparam>
        /// <param name="property"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T GetValueByPropertyType<T, P>(P property)
        {
            try
            {
                foreach (var prop in property.GetType().GetProperties())
                {
                    if (prop.PropertyType.Name == typeof(T).Name)
                    {
                        var propNameVal = prop.GetValue(property, null);
                        return (T)propNameVal;
                    }
                }
                return default(T);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        /// <summary>
        /// 设置值通过属性名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <param name="propertyName"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool SetValueByPropertyName<T>(T property, string propertyName, object val)
        {
            try
            {
                foreach (var prop in property.GetType().GetProperties())
                {
                    if (prop.PropertyType.Name == propertyName)
                    {
                        prop.SetValue(property, val);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        /// <summary>
        /// 两个实体属性赋值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool EntityConvert(object target, object source)
        {
            try
            {
                if (target == null || source == null)
                {
                    return false;
                }
                var tType = target.GetType();
                var sType = source.GetType();
                foreach (PropertyInfo pro in tType.GetProperties())
                {
                    var name = sType.GetProperty(pro.Name);
                    if (name == null)
                        continue;
                    pro.SetValue(target, sType.GetProperty(pro.Name).GetValue(source));
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        /// <summary>
        /// 获取索引
        /// </summary>
        /// <param name="_enum"></param>
        /// <returns></returns>
        public static int GetIndex(Enum _enum)
        {
            if (_enum != null)
            {
                return _enum.GetHashCode();
            }
            else
            {
                return -9999;
            }
        }

        /// <summary>
        /// 通过Index获取Enum
        /// </summary>
        /// <param name="_enum"></param>
        /// <returns></returns>
        public static object GetEnumObjByIndexAndEnumType(int index, Type type)
        {
            return Enum.ToObject(type, index);
        }

        /// <summary>
        /// 比较实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newObj"></param>
        /// <param name="oldObj"></param>
        /// <returns></returns>
        public static bool CompareProperties<T>(T newObj, T oldObj) where T : class
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            bool isMatch = true;
            for (int i = 0; i < properties.Length; i++)
            {
                string s = properties[i].DeclaringType.Name;
                if (properties[i].GetValue(newObj, null).ToString() != properties[i].GetValue(oldObj, null).ToString())
                {
                    isMatch = false;
                    break;
                }
            }

            return isMatch;
        }

    }
}
