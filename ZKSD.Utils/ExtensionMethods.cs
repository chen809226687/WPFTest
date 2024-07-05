using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ZKSD.Utils
{

    /// <summary>
    /// 注：扩展方法必须在静态类中
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// 深克隆
        /// </summary>
        /// <param name="obj">原始版本对象</param>
        /// <returns>深克隆后的对象</returns>
        public static object DepthClone(this object obj)
        {
            object clone = new object();
            using (Stream stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                try
                {
                    formatter.Serialize(stream, obj);
                    stream.Seek(0, SeekOrigin.Begin);
                    clone = formatter.Deserialize(stream);
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                    throw;
                }
            }
            return clone;
        }

    }
}
