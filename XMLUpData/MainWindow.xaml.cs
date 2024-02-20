using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace XMLUpData
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string filePath = @"C:\Program Files\CODESYS 3.5.18.40\CODESYS\Profiles\CODESYS V4.0.profile.xml"; // XML文件路径
            string checksum = CalculateSHA256Checksum(filePath);

            // 假设你已经知道要更新的XML节点的位置
            UpdateXmlChecksum(filePath, "/Profile/Checksum", checksum);

        }


        static string CalculateSHA256Checksum(string filePath)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    byte[] checksum = sha256.ComputeHash(stream);
                    return BitConverter.ToString(checksum).Replace("-", String.Empty).ToLowerInvariant();
                }
            }
        }

        static void UpdateXmlChecksum(string xmlFilePath, string checksumNodePath, string newChecksum)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFilePath);

            // 选定XML中的校验和节点，这里需要根据实际的节点路径进行调整
            XmlNode checksumNode = doc.SelectSingleNode(checksumNodePath);
            if (checksumNode != null)
            {
                checksumNode.InnerText = newChecksum;

                // 保存对XML文件所做的更改
                doc.Save(xmlFilePath);
                Console.WriteLine("XML checksum updated.");
            }
            else
            {
                Console.WriteLine("Checksum node not found.");
            }
        }
    }
}