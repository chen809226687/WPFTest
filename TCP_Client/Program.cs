using System;
using System.Net.Sockets;
using System.Text;

TcpClient client = null;
try
{
    // 设置服务器的IP地址和端口号
    //string serverIP = "127.0.0.1";
    string serverIP = "192.168.3.1";
    int port = 9999;

    // 创建 TcpClient 对象并连接到服务器
    client = new TcpClient(serverIP, port);
    // 设置接收超时时间为10秒
    client.ReceiveTimeout = 100000;

    // 设置发送超时时间为10秒
    client.SendTimeout = 100000;

    Console.WriteLine("Connected to server...");

    // 获取客户端的网络流

    byte[] data = new byte[40000];
    byte[] responseData = new byte[40000];
    NetworkStream stream = client.GetStream();
    int re = 1;
    for (int i = 0; i < 100; i++)
    {
        Thread.Sleep(100);

        string str = "HelloClient" + re.ToString();
        var aaa = Encoding.ASCII.GetBytes(str);
        aaa.CopyTo(data, 0);

        DateTime time = DateTime.Now;
        stream.Write(data, 0, data.Length);
        int bytes = stream.Read(responseData, 0, responseData.Length);
        var ts = DateTime.Now.Subtract(time);

        Console.WriteLine("sent number" + re);
        Console.WriteLine($"Received:" + Encoding.ASCII.GetString(responseData, 0, 20));
        re++;

        Console.WriteLine($"time: {ts.TotalMilliseconds}");
    }
    client.Close();
}
finally
{


}
