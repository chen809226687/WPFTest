/*********************************************************
* 项目名称：ReflectHelper.cs
* 开发人员：liyan
* 公    司：聚时科技
* 创建时间：2022/4/30 18:06:27
* 更新时间：2022/4/30 18:06:27
* CLR版本 ：4.0.30319.42000
*
* 描述说明：
* 
* *******************************************************/

using NPOI.POIFS.Crypt.Dsig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static NPOI.HSSF.Util.HSSFColor;

namespace ZKSD.Utils
{
   public class ReflectionHelper
    {
        /// <summary>
        /// 清除事件绑定的函数
        /// </summary>
        /// <param name="objectHasEvents">拥有事件的实例</param>
        /// <param name="eventName">事件名称</param>
        public static void ClearAllEvents(object objectHasEvents, string eventName)
        {
            if (objectHasEvents == null)
            {
                return;
            }
            try
            {
                EventInfo[] events = objectHasEvents.GetType().GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (events == null || events.Length < 1)
                {
                    return;
                }
                for (int i = 0; i < events.Length; i++)
                {
                    EventInfo ei = events[i];
                    if (ei.Name == eventName)
                    {
                        FieldInfo fi = ei.DeclaringType.GetField(eventName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        if (fi != null)
                        {
                            fi.SetValue(objectHasEvents, null);
                        }
                        break;
                    }
                }
            }
            catch
            {
            }
        }


        /// <summary>
        /// 反射绑定事件
        /// </summary>
        /// <param name="targetObject">拥有事件的实例</param>
        /// <param name="eventName">事件名称</param>
        public static void AddEvent<T>(object targetObject, string eventName, Action<T> action)
        {
            if (targetObject == null || action == null || string.IsNullOrEmpty(eventName))
            {
                return;

            }
            try
            {
                EventInfo eventInfo = targetObject.GetType().GetEvent(eventName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                eventInfo?.AddEventHandler(targetObject, action);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 反射绑定事件
        /// </summary>
        /// <param name="targetObject">拥有事件的实例</param>
        /// <param name="eventName">事件名称</param>
        public static void RemoveEvent<T>(object targetObject, string eventName, Action<T> action)
        {
            if (targetObject == null || action == null || string.IsNullOrEmpty(eventName))
            {
                return;
            }
            try
            {
                EventInfo eventInfo = targetObject.GetType().GetEvent(eventName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                eventInfo?.RemoveEventHandler(targetObject, action);
            }
            catch
            {
            }
        }



        /// <summary>
        /// 获取指定类中指定类型的集合
        /// </summary>
        /// <param name="thisObj"></param>
        /// <param name="genericType"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static object GetGenericValueByType(object thisObj, Type genericType, Type targetType)
        {
            if (thisObj == null || genericType == null || targetType == null) return null;
            PropertyInfo property = thisObj.GetType().GetRuntimeProperties().Where(o =>
                    o.PropertyType.IsGenericType &&
                    o.PropertyType.GetGenericTypeDefinition() == genericType &&
                    o.PropertyType.GenericTypeArguments.Contains(targetType)).FirstOrDefault();

            if (property != null)
            {
                return property.GetValue(thisObj);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取指定类中指定类型的集合对象的属性
        /// </summary>
        /// <param name="thisType"></param>
        /// <param name="genericType"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static PropertyInfo GetGenericPropertyByType(Type thisType, Type genericType, Type targetType)
        {
            if (thisType == null || genericType == null || targetType == null) return null;
            PropertyInfo property = thisType.GetRuntimeProperties().Where(o =>
                    o.PropertyType.IsGenericType &&
                    o.PropertyType.GetGenericTypeDefinition() == genericType &&
                    o.PropertyType.GenericTypeArguments.Contains(targetType)).FirstOrDefault();

            return property;
        }

        /// <summary>
        /// 检查两个对象属性值
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool CheckPropertyValue(object value1, object value2, string propertyName)
        {
            if (value1 == null|| value2==null ||string.IsNullOrEmpty(propertyName))return false;

            return true;
        }




        /// <summary>
        /// 设置指定类中指定类型的集合对象的属性
        /// </summary>
        public static void SetPropertyValue(object obj, string propertyName, object val)
        {
            if (obj == null) return;
            PropertyInfo propertyInfo = obj.GetType().GetProperties().Where(o => o.Name == propertyName).FirstOrDefault();
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(obj, val);
            }

        }


        /// <summary>
        /// 获取指定类中指定类型的集合对象的属性
        /// </summary>
        public static object GetPropertyValue(object obj, string propertyName)
        {
            if (obj == null) return null;
            PropertyInfo propertyInfo = obj.GetType().GetProperties().Where(o => o.Name == propertyName).FirstOrDefault();
            if (propertyInfo != null)
            {
              return  propertyInfo.GetValue(obj);
            }

            return null;
        }

    }
}
