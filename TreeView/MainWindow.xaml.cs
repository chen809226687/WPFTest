using sinsegye.ide.opcuaeditor.Model;
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

namespace TreeView
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<CheckTreeViewModel> checkTreeView = new List<CheckTreeViewModel>();
        CheckTreeViewModel checkTreeViewModel = new CheckTreeViewModel { ViewName = "root" }; // 根节点  
        public MainWindow()
        {
            InitializeComponent();

          
            var checkTreeViewModel1 = new CheckTreeViewModel() { ViewName = "111" };
            var checkTreeViewModel2 = new CheckTreeViewModel() { ViewName = "222" };
            var checkTreeViewModel3 = new CheckTreeViewModel() { ViewName = "333" };
            var checkTreeViewModel4 = new CheckTreeViewModel() { ViewName = "444" };

         

  


            // 将子节点添加到根节点  
            checkTreeViewModel.ChildrenView.Add(checkTreeViewModel1);
            checkTreeViewModel.ChildrenView.Add(checkTreeViewModel2);
            checkTreeViewModel.ChildrenView.Add(checkTreeViewModel3);
            checkTreeViewModel.ChildrenView.Add(checkTreeViewModel4);
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task task = Task.Run(() =>
            {
                checkTreeView.Add(checkTreeViewModel);

                checkTreeView.Add(checkTreeViewModel);

                checkTreeView.Add(checkTreeViewModel);
            });
          //  treeViewData.ItemsSource = checkTreeView;
        }
    }
}
