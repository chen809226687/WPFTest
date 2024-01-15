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

namespace AbstractFactory
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
            //首先创建一种口味工厂
            RelishFactory relishFactory = new AmericanRelishFactory();
            //然后对应口味工厂中生产出对应口味的不同产品。
            Coffee coffee = relishFactory.GetCoffee();
            coffee.GetName();
            Dessert dessert = relishFactory.GetDessert();
            dessert.GetName();

            Console.WriteLine("换一种口味");
            relishFactory = new ItalyRelishFactory();
            coffee = relishFactory.GetCoffee();
            coffee.GetName();
            dessert = relishFactory.GetDessert();
            dessert.GetName();
        }
    }
}