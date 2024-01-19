using Sinsegye.PLCHandler;
using Sinsegye.PLCHandler.Models;

namespace PLCHandle
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string mesg = "";
        PLCHandler handler = new PLCHandler();
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void _sinsegyeClient_VariableChangedEvent(object? sender, VariableChangedArgs e)
        {

            this.Invoke(new Action(() =>
            {
                textBox4.Text = e.Value.ToString();
            }));

            //订阅变量处理逻辑
        }

        Type Type;
        private void CheckChange(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    Type = typeof(bool);
                    break;
                case 1:
                    Type = typeof(Int16);
                    break;
                case 2:
                    Type = typeof(byte);
                    break;
                case 3:
                    Type = typeof(float);
                    break;
                case 4:
                    Type = typeof(string);
                    break;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Type != null)
            {
                try
                {
                    object obj = null;
                    bool flag = handler.RegVariable(textBox1.Text, Type, ref mesg);

                }
                catch { }
            }
            else
            {
                MessageBox.Show("选择类型");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (Type != null)
            {
                try
                {
                    object obj = null;
                    bool flag = handler.ReadPlc(textBox1.Text, Type, out obj);
                    textBox2.Text = obj.ToString();

                }
                catch { }
            }
            else
            {
                MessageBox.Show("选择类型");
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                object obj = null;
                bool flag = handler.WritePlc(textBox1.Text, textBox3.Text, Type, ref mesg);
            }
            catch { }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                var aaa = handler.ReadVariableListAsync();
                foreach (var x in aaa)
                {

                    listBox1.Items.Add(x.Name);
                }
            }
            catch { }


        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = listBox1.SelectedItem.ToString();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                bool flag = handler.Init("192.168.110.202");
                flag = handler.Connect(ref mesg);
                handler.VariableChangedEvent += _sinsegyeClient_VariableChangedEvent;

            }
            catch
            {

            }

        }

        bool flag;
        private void button8_Click(object sender, EventArgs e)
        {

          IList<BatchReadValueArgs>  batchReadValueArgs = new List<BatchReadValueArgs>()
          {
              //new BatchReadValueArgs
              //{
              //    Name = "Application.GVL_PARAM.DustClear_DelaySet",
              //    Type = typeof(short),
              //},
               new BatchReadValueArgs
              {
                  Name = "Application.GVL_PARAM.TCP_IPAdr",
                  Type = typeof(string),
              }
          };

            IList<Variable> variables = new List<Variable>();
           flag = handler.BatchReadPlc(batchReadValueArgs,out variables, ref mesg);
        }

        private void button7_Click(object sender, EventArgs e)
        {
        }
    }
}
