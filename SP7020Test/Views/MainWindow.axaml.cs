using Avalonia.Controls;
using System.Security.Policy;
using System;
using ZKSD.Utils;

namespace SP7020Test.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        //{
        //    OPCUAHelper _API = new OPCUAHelper();
        //    try
        //    {
        //        _API.OpenConnectOfAnonymous(text.Text);
        //        if (_API.ConnectStatus)
        //        {
        //            lab.Content = "连接成功！";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lab.Content = "连接失败！";
        //    }

        //}
        //Window1 window1 = new Window1();
        //private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        //{
       
        //    window1.Show();
        //}
    }
}