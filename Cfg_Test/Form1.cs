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
                PassWord = "1"
            });
            CfgHelper.Set(data);
        }
    }
}
