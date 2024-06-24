using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace treeviewTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();



        }


        private void AddNodes(TreeNode parentNode, List<string> data)
        {
            foreach (string item in data)
            {
                TreeNode newNode = new TreeNode(item);
                parentNode.Nodes.Add(newNode);

                // 在这里可以添加更多的数据或设置节点属性
                // newNode.Tag = 数据; // 可以将数据存储在节点的Tag属性中

                // 如果有子数据，递归调用AddNodes方法
                List<string> childData = new List<string> { };
                if (childData != null && childData.Count > 0)
                {
                    AddNodes(newNode, childData);
                }

                treeView1.Show();
            }
        }

        // 调用AddNodes方法来添加根节点和数据
        private void AddRootNodes(List<string> rootData)
        {
            foreach (string rootItem in rootData)
            {
                TreeNode rootNode = new TreeNode(rootItem);
                treeView1.Nodes.Add(rootNode);

                // 在这里可以添加更多的数据或设置节点属性
                // rootNode.Tag = 数据; // 可以将数据存储在节点的Tag属性中

                // 获取根节点的子数据并添加子节点
                List<string> childData = new List<string>()
                {
                   "111",
                   "222",
                   "333"
                };
                if (childData != null && childData.Count > 0)
                {
                    AddNodes(rootNode, childData);
                }
            }
        }

        // 在Form加载时调用AddRootNodes方法
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            List<string> rootData = new List<string>()
            {
                "aaa",
                "bbb",
                "ccc"


            };
            AddRootNodes(rootData);

        }
        static string ReplaceChinese(string input, string replacement)
        {
            // 使用正则表达式匹配中文字符
            string pattern = @"[\u4e00-\u9fa5]";
            Regex regex = new Regex(pattern);

            // 使用 MatchEvaluator 替换匹配到的中文字符
            string result = regex.Replace(input, match => replacement);

            return result;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Step = 10;

            for (int i = 0; i <= 100; i += 10)
            {
                progressBar1.Value = i;
                System.Threading.Thread.Sleep(500); // 模拟耗时操作
            }


            for (int i = 0; i <= 100; i += 10)
            {
                progressBar1.Value = i;
                System.Threading.Thread.Sleep(500); // 模拟耗时操作
            }


        }

    }
}
