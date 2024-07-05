
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using System.Threading;

string serverIp = "192.168.1.8";
int serverPort = 4841;

UdpClient client = new UdpClient();


IPAddress serverAddress = IPAddress.Parse(serverIp);
IPEndPoint endPoint = new IPEndPoint(serverAddress, serverPort);
byte[] data = new byte[40000];
int timeout = 100; // 设置超时时间为5秒
int re = 1;
for (int i = 0; i < 100; i++)
{
    Thread.Sleep(100);

    string str = "hello" + i.ToString();
    var aaa = Encoding.ASCII.GetBytes(str);
    aaa.CopyTo(data, 0);

    string end = "world" + i.ToString();
    var bbb = Encoding.ASCII.GetBytes(end);
    bbb.CopyTo(data, 3991);

    var result = client.Send(data, data.Length, endPoint);
    Console.WriteLine("number " + re);
    re++;

    // 异步接收数据
    Task<UdpReceiveResult> receiveTask = client.ReceiveAsync();
    if (Task.WaitAny(new[] { receiveTask }, timeout) == -1)
    {
         Console.WriteLine("Receive operation timed out.");
    }
    else
    {
        UdpReceiveResult result1 = receiveTask.Result;
        string response = Encoding.ASCII.GetString(result1.Buffer);
        Console.WriteLine("Response from server: " + response);
    }

}

