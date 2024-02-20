using Avalonia.Controls;
using Avalonia.Interactivity;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;

namespace ListTest.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public class MyItemModel
        {
            public string Content { get; set; }
            // 其他属性
        }
        public ObservableCollection<MyItemModel> Items { get; set; }

        public MainWindowViewModel()
        {
            Items = new ObservableCollection<MyItemModel>();
            // 添加示例数据
            Items.Add(new MyItemModel { Content = "Item 1" });
            Items.Add(new MyItemModel { Content = "Item 2" });
            Items.Add(new MyItemModel { Content = "Item 3" });
            Items.Add(new MyItemModel { Content = "Item 4" });
            Items.Add(new MyItemModel { Content = "Item 5" });
            Items.Add(new MyItemModel { Content = "Item 6" });
            Items.Add(new MyItemModel { Content = "Item 7" });
            Items.Add(new MyItemModel { Content = "Item 8" });

            // 更多项...
        }


       

    }
}
