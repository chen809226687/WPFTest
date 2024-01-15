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

namespace FactoryTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //通过CoffeeFactory产品工厂创建了AmericanCoffee产品实例
            Coffee coffee = CoffeeFactory.OrderCoffe(CoffeeFactory.CoffeeEnum.AmericanCoffee);
            coffee.GetName();

            //通过CoffeeFactory产品工厂创建了LatterCoffe产品实例
            coffee = CoffeeFactory.OrderCoffe(CoffeeFactory.CoffeeEnum.LatterCoffe);
            coffee.GetName();
            coffee.AddSugar();
        }
    }
}