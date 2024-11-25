using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFTreeViewTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<CheckTreeView> ChildrenView { get; set; } = new List<CheckTreeView>();
        public MainWindow()
        {
            // 创建根节点
            CheckTreeView root = new CheckTreeView() { ViewName = "根节点" };

            // 创建四层结构
            root.ChildrenView = CreateChildren(4, 3); // 4层，每层3个子节点

            // 将根节点添加到 ChildrenView
            ChildrenView.Add(root);




            InitializeComponent();

            checkView.ItemsSource = ChildrenView;


        }

        // 递归创建子节点
        private List<CheckTreeView> CreateChildren(int level, int count)
        {
            if (level <= 0)
            {
                return null;
            }

            List<CheckTreeView> children = new List<CheckTreeView>();
            for (int i = 1; i <= count; i++)
            {
                CheckTreeView child = new CheckTreeView() { ViewName = $"子节点{i}" };
                child.ChildrenView = CreateChildren(level - 1, count);
                children.Add(child);
            }

            return children;
        }


        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox ck = (CheckBox)sender;
            try
            {
                int fid = Convert.ToInt32(ck.Tag);
                //新建存储过程 修改物标显示DISPLAY字段 fid 为对应物标id
            }
            catch (FormatException es)
            {
                //说明是群组id
                string ids = ck.Tag.ToString().Trim('.');
                //新建存储过程 修改物标显示DISPLAY字段 ids 为对应群组 id
            }

        }
    }
}
