/*********************************************************
* 项目名称：MappingEnum.cs
* 开发人员：liyan
* 公    司：聚时科技
* 创建时间：2021/11/30 11:46:44
* 更新时间：2021/11/30 11:46:44
* CLR版本 ：4.0.30319.42000
*
* 描述说明：通过Enum反射实体
* 
* *******************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Mappings
{
    public static class MappingEnum
    {
        /// <summary>
        /// 通过字符串获取枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueName"></param>
        /// <returns></returns>
        public static T GetEnumValue<T>(string valueName) where T : Enum
        {
            try
            {
                T t = (T)System.Enum.Parse(typeof(T), valueName);
                return t;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// 通过字符串获取枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueName"></param>
        /// <returns></returns>
        public static T GetEnumByIndex<T>(int index) where T : Enum
        {
            try
            {
                T t = GetEnumValue<T>(typeof(T).GetEnumName(index));
                return t;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public static string GetEnumDescription<T>(T enumValue) where T : Enum
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
        /// 通过字符串获取枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueName"></param>
        /// <returns></returns>
        public static List<T> GetEnumValues<T>() where T : Enum
        {
            try
            {
                List<T> list = new List<T>();

                foreach (T val in Enum.GetValues(typeof(T)))
                {
                    list.Add(val);
                }
                return list;
            }
            catch
            {
                return null;
            }
        }
    }
}
