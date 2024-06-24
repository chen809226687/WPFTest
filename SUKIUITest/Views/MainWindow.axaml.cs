using Avalonia.Controls;
using Avalonia.Media;

namespace SUKIUITest.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
        }

        private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            SukiUI.MessageBox.MessageBox.Info(this, "sss", "sass");

        }

        private void Image_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            Image clickedImage = (Image)sender;

            // 创建一个新窗口用于显示放大后的图片
            Window zoomWindow = new Window
            {
                Width = 800,
                Height = 600,
                Content = new Image
                {
                    Source = clickedImage.Source,
                    Stretch = Stretch.Uniform
                }
            };

            zoomWindow.ShowDialog(this);
        }

        private void MenuItem_Click_2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
    
        }

        private void MenuItem_Click_3(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
        }
    }
}