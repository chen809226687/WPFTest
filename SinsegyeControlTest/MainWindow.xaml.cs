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

namespace SinsegyeControlTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private bool _isDarkTheme = true;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _isDarkTheme = !_isDarkTheme;
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            ResourceDictionary newTheme = new ResourceDictionary();
            ResourceDictionary newTheme1 = new ResourceDictionary();

            if (_isDarkTheme)
            {
                newTheme.Source = new Uri("pack://application:,,,/Sinsegye.Ide.Resources;component/Themes/Basic/Dark.Color.xaml");
                newTheme1.Source = new Uri("pack://application:,,,/Sinsegye.Ide.Resources;component/Themes/Basic/Light.Color.xaml");

            }
            else
            {
                newTheme.Source = new Uri("pack://application:,,,/Sinsegye.Ide.Resources;component/Themes/Basic/Light.Color.xaml");
                newTheme1.Source = new Uri("pack://application:,,,/Sinsegye.Ide.Resources;component/Themes/Basic/Dark.Color.xaml");
            }

            // 清除现有资源字典
            // this.Resources.MergedDictionaries.Clear();
            this.Resources.MergedDictionaries.Remove(newTheme1);

            this.Resources.MergedDictionaries.Add(newTheme);
        }

        private void CloseableTabItem_Close(object sender, RoutedEventArgs e)
        {

        }

        private void CloseTabItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
