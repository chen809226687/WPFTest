
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
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
using System.Xml.Linq;

namespace WPF_DITest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            var build = new ServiceCollection();
            build.AddSingleton<ICar, Car>();
            build.AddSingleton<Service>();
            var servicess = build.BuildServiceProvider();
            var classssss = servicess.GetService<ICar>();
            var Serviceclass = servicess.GetService<Service>();
            Serviceclass.fangfafafa();

            classssss.aaa ();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {



        }

    }
}