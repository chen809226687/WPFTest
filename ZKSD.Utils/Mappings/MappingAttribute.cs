/*********************************************************
* 项目名称：MappingAttribute.cs
* 开发人员：liyan
* 公    司：聚时科技
* 创建时间：2021/11/30 11:40:08
* 更新时间：2021/11/30 11:40:08
* CLR版本 ：4.0.30319.42000
*
* 描述说明：通过Attribute属性反射实体
* 
* *******************************************************/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Mappings
{
    public static class MappingAttribute
    {
        /// <summary>
        /// 获取Description属性的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescriptionAttribute<T>(T value)
        {
            try
            {
                var type = typeof(T);
                var memberInfo = type.GetMember(value.ToString()).FirstOrDefault();
                var descriptionAttribute = memberInfo.GetCustomAttribute<DescriptionAttribute>();
                if (descriptionAttribute == null)
                    return string.Empty;
                return descriptionAttribute.Description;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        /// <summary>
        /// 获取Description属性的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static A GetAttribute<T, A>(T value) where A : Attribute
        {
            try
            {
                var type = typeof(T);
                var memberInfo = type.GetMember(value.ToString()).FirstOrDefault();
                var attribute = memberInfo.GetCustomAttribute<A>();
                if (attribute == null)
                    return null;
                return attribute;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// 根据Josn字符串定义DescriptionAttribute属性反射实体（入口）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="josnStr"></param>
        /// <param name="attributePropertyName"></param>
        /// <returns></returns>
        public static T GetEntityByDescriptionAttribute<T>(string josnStr, T t = default) where T : class, new()
        {
            try
            {
                return GetEntityByAttribute<T, DescriptionAttribute>(josnStr, "Description",t);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// 根据Josn字符串定义DescriptionAttribute属性反射实体（入口）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="josnStr"></param>
        /// <param name="attributePropertyName"></param>
        /// <returns></returns>
        public static object GetEntityByDescriptionAttribute(string josnStr, Type type)
        {
            try
            {
                return GetEntityByAttribute<DescriptionAttribute>(josnStr, "Description", type);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// 根据Josn字符串定义Attribute属性反射实体（入口）
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <typeparam name="A">Attr类型</typeparam>
        /// <param name="josnStr"></param>
        /// <param name="attributePropertyName">Attr中定义反射类型属性名称</param>
        /// <param name="t">默认可以不传</param>
        /// <param name="attr">默认可以不传</param>
        /// <returns>T</returns>

        public static T GetEntityByAttribute<T, A>(string josnStr, string attributePropertyName, T t = default, A attr = default) where T : class, new() where A : Attribute
        {
            try
            {
                if (string.IsNullOrEmpty(josnStr) || string.IsNullOrWhiteSpace(josnStr)) return null;
                Type tType;
                if (t == null)
                {
                    tType = typeof(T);
                }
                else
                {
                    tType = t.GetType();
                }
       
                if (t == null)
                {
                    t = Activator.CreateInstance<T>();
                }
                if (attr == null)
                {
                    attr = Activator.CreateInstance<A>();
                }

                if (tType.IsGenericType)//数组或集合
                {
                    Type modelType = tType.GetGenericArguments().First();
                    dynamic modelList = Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[] { modelType }));
                    dynamic coll = JsonConvert.DeserializeObject(josnStr);
                    if (coll == null) return default(T);
                    foreach (var val in coll)
                    {
                        string josn = val.ToString();
                        if (modelType.IsGenericType)
                        {
                            dynamic tModel = Activator.CreateInstance(modelType);
                            dynamic res = GetEntityByAttribute(josn, attributePropertyName, tModel, attr);
                            modelList.Add(res);
                        }
                        else if (typeof(string).Equals(modelType))
                        {
                            modelList.Add(josn);
                        }
                        else if (modelType.IsClass && !typeof(string).Equals(modelType))
                        {
                            dynamic tModel = Activator.CreateInstance(modelType);
                            dynamic res = GetEntityByAttribute_Imp(josn, attributePropertyName, tModel, attr);
                            modelList.Add(res);
                        }
                        else if (modelType.IsEnum)
                        {
                            dynamic res = Enum.Parse(modelType, josn);
                            modelList.Add(res);
                        }
                        else
                        {
                            dynamic res = Convert.ChangeType(val, modelType);
                            modelList.Add(res);
                        }
                    }
                    return modelList;
                }
                else
                {
                    return GetEntityByAttribute_Imp(josnStr, attributePropertyName, t, attr);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// 根据Josn字符串定义Attribute属性反射实体（实现）
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <typeparam name="A">Attr类型</typeparam>
        /// <param name="josnStr"></param>
        /// <param name="attributePropertyName">Attr中定义反射类型属性名称</param>
        /// <param name="t">默认可以不传</param>
        /// <param name="attr">默认可以不传</param>
        /// <returns>T</returns>
        private static T GetEntityByAttribute_Imp<T, A>(string josnStr, string attributePropertyName, T t = default, A attr = default) where T : class, new() where A : Attribute
        {
            if (string.IsNullOrEmpty(josnStr) || string.IsNullOrWhiteSpace(josnStr)) return null;
            var dic = JsonConvert.DeserializeObject<Dictionary<dynamic, dynamic>>(josnStr);
            if (dic == null) return null;
            if (t == null)
            {
                t = Activator.CreateInstance<T>();
            }
            if (attr == null)
            {
                attr = Activator.CreateInstance<A>();
            }
            foreach (PropertyInfo property in t.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                A proAttr = property.GetCustomAttribute<A>();
                if (proAttr == null) continue;

                PropertyInfo attrProperty = proAttr.GetType().GetProperty(attributePropertyName);

                dynamic val = attrProperty.GetValue(proAttr);
                if (dic.ContainsKey(val))
                {
                    try
                    {
                        if (property.PropertyType.IsGenericType)//集合
                        {
                            dynamic tModel = Activator.CreateInstance(property.PropertyType);
                            dynamic propertyVal = GetEntityByAttribute(dic[val].ToString(), attributePropertyName, tModel, attr);
                            property.SetValue(t, propertyVal);
                        }
                        else if (typeof(string).Equals(property.PropertyType))
                        {
                            if (dic[val] == null) continue;
                            string propertyVal = dic[val].ToString();
                            property.SetValue(t, propertyVal);
                        }
                        else if (property.PropertyType.IsClass && !typeof(string).Equals(property.PropertyType))
                        {
                            dynamic tModel = Activator.CreateInstance(property.PropertyType);
                            dynamic propertyVal = GetEntityByAttribute_Imp(dic[val].ToString(), attributePropertyName, tModel, attr);
                            property.SetValue(t, propertyVal);
                        }
                        else if (property.PropertyType.IsEnum)
                        {
                            dynamic propertyVal = Enum.Parse(property.PropertyType, dic[val].ToString());
                            property.SetValue(t, propertyVal);
                        }
                        else
                        {
                            dynamic value = dic[val];
                            dynamic propertyVal = Convert.ChangeType(value, property.PropertyType);
                            property.SetValue(t, propertyVal);
                        }
                    }
                    catch(Exception ex)
                    {
                        throw new Exception(ex.ToString());
                    }
                    dic.Remove(val);
                }
            }
            return t;
        }


        /// <summary>
        /// 根据Josn字符串定义Attribute属性反射实体（入口）
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <typeparam name="A">Attr类型</typeparam>
        /// <param name="josnStr"></param>
        /// <param name="attributePropertyName">Attr中定义反射类型属性名称</param>
        /// <param name="returnType"></param>
        /// <returns>T</returns>

        public static dynamic GetEntityByAttribute<A>(string josnStr, string attributePropertyName, Type returnType) where A : Attribute
        {
            try
            {
                if (string.IsNullOrEmpty(josnStr) || string.IsNullOrWhiteSpace(josnStr)) return null;
                object t= Activator.CreateInstance(returnType);
                Attribute attr= Activator.CreateInstance<A>();

                if (returnType.IsGenericType)//数组或集合
                {
                    Type modelType = returnType.GetGenericArguments().First();
                    dynamic modelList = Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[] { modelType }));
                    dynamic coll = JsonConvert.DeserializeObject(josnStr);
                    foreach (var val in coll)
                    {
                        string josn = val.ToString();
                        if (modelType.IsGenericType)
                        {
                            dynamic tModel = Activator.CreateInstance(modelType);
                            dynamic res = GetEntityByAttribute(josn, attributePropertyName, tModel, attr);
                            modelList.Add(res);
                        }
                        else if (typeof(string).Equals(modelType))
                        {
                            modelList.Add(josn);
                        }
                        else if (modelType.IsClass && !typeof(string).Equals(modelType))
                        {
                            dynamic tModel = Activator.CreateInstance(modelType);
                            dynamic res = GetEntityByAttribute_Imp(josn, attributePropertyName, tModel, attr);
                            modelList.Add(res);
                        }
                        else if (modelType.IsEnum)
                        {
                            dynamic res = Enum.Parse(modelType, josn);
                            modelList.Add(res);
                        }
                        else
                        {
                            dynamic res = Convert.ChangeType(val, modelType);
                            modelList.Add(res);
                        }
                    }
                    return modelList;
                }
                else
                {
                    return GetEntityByAttribute_Imp(josnStr, attributePropertyName, t, attr);
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"josnstr:{josnStr},attributePropertyName:{attributePropertyName},returnType:{returnType.ToString()},Exception:{ex.ToString()}");
            }
        }
    }
}
