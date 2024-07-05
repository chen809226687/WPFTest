using System.Net.Sockets;
using System.Text;

namespace WinFormsTCP_Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        TcpClient client = null;
        private void button1_Click(object sender, EventArgs e)
        {


            // ���÷�������IP��ַ�Ͷ˿ں�
            // string serverIP = "127.0.0.1";
            string serverIP = "192.168.110.8";
            int port = 9999;

            // ���� TcpClient �������ӵ�������
            client = new TcpClient(serverIP, port);
            // ���ý��ճ�ʱʱ��Ϊ10��
            client.ReceiveTimeout = 30000;

            // ���÷��ͳ�ʱʱ��Ϊ10��
            client.SendTimeout = 10000;

            Console.WriteLine("Connected to server...");
        }
        int re = 1;
        private void button2_Click(object sender, EventArgs e)
        {
            byte[] data = new byte[40000];
            byte[] responseData = new byte[40000];
            NetworkStream stream = client.GetStream();



            string str = "HelloClient" + re.ToString();
            var aaa = Encoding.ASCII.GetBytes(str);
            aaa.CopyTo(data, 0);

            DateTime time = DateTime.Now;
            stream.Write(data, 0, data.Length);

            int bytes = stream.Read(responseData, 0, responseData.Length);
            var ts = DateTime.Now.Subtract(time);

            textBox1.Text += "sent number" + re.ToString() + "\n";
            textBox1.Text += "Received:" + Encoding.ASCII.GetString(responseData, 0, 20).ToString() + "\n";
            textBox1.Text += "time:" + ts.TotalMilliseconds.ToString() + "\n";
            re++;


        }
    }
}
