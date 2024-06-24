using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBus
{
    public class Class1
    {
        public Class1()
        {
            MessageBus.MessageReceived += (message, data) =>
            {
                if (message == "aaa")
                {
                    // 在这里处理收到的消息
                    // 可以根据需要处理收到的数据
                    // 例如，更新 UI 界面或执行相应的操作
                }
            };
        }
    }


    public static class MessageBus
    {
        public static event Action<string, object> MessageReceived;

        public static void SendMessage(string message, object data)
        {
            MessageReceived?.Invoke(message, data);
        }
    }
}
