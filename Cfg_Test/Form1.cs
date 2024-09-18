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
            // 获取当前工作目录
            string root = AppDomain.CurrentDomain.BaseDirectory;
            root = root + "Cfgs\\";
            //用户配置
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
                // 在这里编写你要执行的代码
                Thread.Sleep(500); // 假设这里有一个耗时操作

            }, cts.Token);

            if (Task.WaitAny(new[] { task }, TimeSpan.FromSeconds(1)) < 0)
            {
                // 如果超过1秒还没有完成，就取消任务
                cts.Cancel();
                Console.WriteLine("Task timed out");
            }
            else
            {
                Console.WriteLine("Task completed successfully");
            }
        }

        // 获取当前工作目录
        string root = AppDomain.CurrentDomain.BaseDirectory + "Csv\\";


        private void button5_Click(object sender, EventArgs e)
        {

            //归档csv
            MyCsvHelper.WriteCsv<Model>(models, root + "excel1.csv");


        }

        List<Model> models = new List<Model>();
        int i = 0;
        DataValue dataValue;
        private void button6_Click(object sender, EventArgs e)
        {
            models.Add(new Model()
            {
                Name = i.ToString() + "水水水水水水水水水水水水水水水水水水顶顶顶顶顶顶顶顶顶顶顶顶顶顶",
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
                //归档csv
                MyCsvHelper.AppendCsv<Model>(models, root + "excel1.csv");
                Thread.Sleep(10);
            }

        }
    }
}
