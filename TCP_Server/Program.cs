using System.Net.Sockets;
using System.Net;
using System.Text;

TcpListener server = null;

// 设置服务器的IP地址和端口号
//IPAddress ipAddress = IPAddress.Parse("0.0.0.0");
IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
int port = 9999;

// 创建 TcpListener 对象
server = new TcpListener(ipAddress, port);
// 开始监听传入的连接请求
server.Start();
Console.WriteLine("Server started...");
// 接受客户端连接
TcpClient client = server.AcceptTcpClient();
Console.WriteLine("Client connected...");
// 获取客户端的网络流
NetworkStream stream = client.GetStream();
int i = 1;

// 设置接收超时时间为10秒
client.ReceiveTimeout = 100000;

// 设置发送超时时间为10秒
client.SendTimeout = 100000;


// 判断客户端是否关闭连接
bool clientClosed = false;

while (!clientClosed)
{
    if (client.Client.Poll(10, SelectMode.SelectRead) && (client.Client.Available == 0))
    {
        clientClosed = true;
    }
    if (clientClosed)
    {
        Console.WriteLine("Client has closed the connection");
    }
    else
    {
        //接收客户端发送的消息
        byte[] data = new byte[40000];
        int bytesRead = stream.Read(data, 0, data.Length);
        Console.WriteLine($"Received:" + Encoding.ASCII.GetString(data, 0, 20) + "\r\n" + "size:" + bytesRead);
        // 发送响应消息给客户端
        byte[] responseMessage = new byte[40000];
        string str = "HelloServer" + i.ToString();
        var aaa = Encoding.ASCII.GetBytes(str);
        aaa.CopyTo(responseMessage, 0);
        stream.Write(responseMessage, 0, responseMessage.Length);
        Console.WriteLine("sent number:" + i);
        i++;
    }
}
stream.Close();
client.Close();
server.Stop();





