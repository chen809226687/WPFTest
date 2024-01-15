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

namespace FactoryMethod
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
            //首先创建一个美式咖啡工厂，只负责生产美式咖啡产品
            CoffeeFactory coffeeFactory = new AmericanFactory();
            //在美式咖啡工厂中生产一个美式咖啡产品
            Coffee coffee = coffeeFactory.GetCoffee();
            coffee.GetName();
            coffee.AddSugar();

            //创建一个拿铁咖啡工厂，只负责生产拿铁咖啡产品
            coffeeFactory = new LatterFactory();
            //在工厂中生产一个拿铁咖啡产品
            coffee = coffeeFactory.GetCoffee();
            coffee.GetName();
            //咖啡中加糖
            coffee.AddSugar();
        }
    }
}