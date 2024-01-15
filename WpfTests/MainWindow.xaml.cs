using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfTests
{

    public static class ExpandMethod
    {
        /// <summary>
        /// 将字符串转换为int
        /// </summary>
        /// <param name="str"></param>
        /// <returns>失败返回0</returns>
        public static int StringToInt(this string str)
        {
            int.TryParse(str, out var res);
            return res;
        }
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var a = "2".StringToInt();
            var b = a.GetType();

        }
    }
}