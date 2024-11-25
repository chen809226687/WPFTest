using sinsegye.acpsharp.acp;
using sinsegye.acpsharp.plcopen;


namespace AcpTest_Nugetwin32
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AcpClient acpClient = new AcpClient("192.168.110.8");

            STRING sTRING = new STRING();
            string str;
            var error12 = acpClient.ReadVar("Application.GVL.string1", sTRING, out str);
        }
    }
}
