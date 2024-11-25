using sinsegye.acpsharp.acp;
using sinsegye.acpsharp.plcopen;
using System;

namespace ACPTest
{
    internal class Program
    {
        static void Main(string[] args)
        {


       


            TcAdsClient ads;





            //初始化Client,构造函数（ip后面+.1.1,port号(读写变量业务使用600),是否重连,初始化发送超时时间，初始化接收超时时间，通讯类型，）
            // AcpClient _acpClient = new AcpClient();
            AcpClient _acpClient = new AcpClient("192.168.110.126");

            // _acpClient.Connect("192.168.110.126.1.1", false);


            BYTE bYTE = 0;

            string AAA;
            var error20 = _acpClient.ReadVar("Application.GVL.Basket_BoxUnit", bYTE, out AAA);

            ARRAY<INT> intArray = new ARRAY<INT>(10);
            byte[] byteArray;
            var error200 = _acpClient.ReadVar("Application.GVL.PNType", out byteArray, out AAA);

            byte[] firstTenBytes = new byte[20];
            Array.Copy(byteArray, firstTenBytes, 20);
            intArray.Unmarshal(firstTenBytes);



            //订阅相关
            //注册回调函数
            var error2 = _acpClient.RegisterRouterEvent(SubCallback);
            ////添加订阅 
            //string[] strings1 = { "Application.GVL.Basket_BoxUnit","" };


            string[] strings1 = new string[10];
            for (int i = 0; i < strings1.Length; i++)
            {
                strings1[i] = "Application.GVL.PNType[" + i.ToString() + "]";
            }



            var error3 = _acpClient.AddBatchSubVar(strings1, 6, 15);

            Console.ReadLine();
        }

        //订阅后的回调函数
        public static void SubCallback(ValueChangeDates symbolSubNode)
        {
            //遍历搜索找到变量，接收值
            foreach (var aa in symbolSubNode.ValueChangeDateList)
            {

            }
        }
    }
}
