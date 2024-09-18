using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MessageBus
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Class1 class1 = new Class1();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ss = "中ss";

            var bbb = Encoding.UTF8.GetByteCount(ss);

            char[] a = new char[2];
            a[0] = '是';
            var bbb1 = Encoding.UTF8.GetByteCount(a);

            // 原始字符串使用系统默认编码（通常是ANSI编码）
            string originalString = "你好，世界！";

            // 将原始字符串使用默认编码转换为字节数组
            byte[] defaultBytes = Encoding.Default.GetBytes(originalString);

            var aaaa = Encoding.Convert(Encoding.Default, Encoding.UTF8, defaultBytes);


            // 将原始字符串使用默认编码转换为字节数组
            byte[] utf8Bytes = Encoding.UTF8.GetBytes(originalString);


            string aa = Encoding.Default.GetString(defaultBytes);


            // 将原始字符串使用默认编码转换为字节数组
            byte[] defaultBytes1 = Encoding.Default.GetBytes(aa);

            // 将原始字符串使用默认编码转换为字节数组
            byte[] utf8Bytes1 = Encoding.UTF8.GetBytes(aa);


            string bb = Encoding.UTF8.GetString(utf8Bytes);

            // 将使用默认编码编码的字节数组转换为UTF-8编码的字节数组
            //byte[] utf8Bytes = Encoding.Convert(Encoding.Default, Encoding.UTF8, defaultBytes);

            // 将UTF-8编码的字节数组转换为字符串
            string utf8String = Encoding.UTF8.GetString(utf8Bytes);
        }

        int i = 0;
        int j = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add("aa"+i++.ToString());

        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = -1;
           // listBox1.Items.Remove(listBox1.SelectedItem);
        }
    }

    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }

        
    }
}
