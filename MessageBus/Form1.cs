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
            MessageBus.SendMessage("aaa", "Data to send");
            MessageBus.SendMessage("bbb", "Data to send");
        }
    }
}
