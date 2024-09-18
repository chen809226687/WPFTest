using Opc.Ua;
using ZKSD.Utils;
using static ZKSD.Utils.CfgHelper;

namespace Cfg_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // ��ȡ��ǰ����Ŀ¼
            string root = AppDomain.CurrentDomain.BaseDirectory;
            root = root + "Cfgs\\";
            //�û�����
            LoadCfgModel cfg = new LoadCfgModel();
            cfg.Type = typeof(List<Model>);
            cfg.Path = root + "UserCfg.json";
            CfgHelper.RegisterCfgModel(cfg);
        }

        List<Model> data;
        private void button2_Click(object sender, EventArgs e)
        {
            data = (List<Model>)CfgHelper.Get(typeof(List<Model>));

            label1.Text = data.Count.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            data.Add(new Model()
            {
                Name = "1",
                PassWord = "1",


            });
            CfgHelper.Set(data);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var cts = new CancellationTokenSource();
            var task = Task.Run(() =>
            {
                // �������д��Ҫִ�еĴ���
                Thread.Sleep(500); // ����������һ����ʱ����

            }, cts.Token);

            if (Task.WaitAny(new[] { task }, TimeSpan.FromSeconds(1)) < 0)
            {
                // �������1�뻹û����ɣ���ȡ������
                cts.Cancel();
                Console.WriteLine("Task timed out");
            }
            else
            {
                Console.WriteLine("Task completed successfully");
            }
        }

        // ��ȡ��ǰ����Ŀ¼
        string root = AppDomain.CurrentDomain.BaseDirectory + "Csv\\";


        private void button5_Click(object sender, EventArgs e)
        {

            //�鵵csv
            MyCsvHelper.WriteCsv<Model>(models, root + "excel1.csv");


        }

        List<Model> models = new List<Model>();
        int i = 0;
        DataValue dataValue;
        private void button6_Click(object sender, EventArgs e)
        {
            models.Add(new Model()
            {
                Name = i.ToString() + "ˮˮˮˮˮˮˮˮˮˮˮˮˮˮˮˮˮˮ����������������������������",
                PassWord = "pp" + i.ToString(),
                value = new DataValue()
                {
                    Value = i.ToString()
                }
            });
            i++;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            while (true)
            {
                //foreach(var model in models)
                //{
                //    if (model.value is DataValue b)
                //    {
                //        model.value = b.Value.ToString();
                //    }
                //}
                //�鵵csv
                MyCsvHelper.AppendCsv<Model>(models, root + "excel1.csv");
                Thread.Sleep(10);
            }

        }
    }
}
