using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfTest
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            People = new ObservableCollection<Person>
            {
                new Person { Name1 = "内容", Name2 = "内容",Name3="内容",Name4="内容" },
                new Person { Name1 = "内容", Name2 = "内容",Name3="内容",Name4="内容" },
                new Person { Name1 = "内容", Name2 = "内容",Name3="内容",Name4="内容" },
                new Person { Name1 = "内容", Name2 = "内容",Name3="内容",Name4="内容" },


            };
            //CheckTreeViewModel checkTreeViewModel1 = new CheckTreeViewModel() { ViewName = "11111" };
            //CheckTreeViewModel checkTreeViewModel2 = new CheckTreeViewModel() { ViewName = "22222", Parent = checkTreeViewModel1 };
            //CheckTreeViewModel checkTreeViewModel3 = new CheckTreeViewModel() { ViewName = "33333", Parent = checkTreeViewModel1 };
            //CheckTreeViewModel checkTreeViewModel4 = new CheckTreeViewModel() { ViewName = "44444", Parent = checkTreeViewModel1 };
            //CheckTreeViewModel checkTreeViewModel5 = new CheckTreeViewModel() { ViewName = "55555", Parent = checkTreeViewModel2 };

            //CheckTreeViewModel checkTreeViewModel6 = new CheckTreeViewModel() { ViewName = "66666", Parent = checkTreeViewModel2 };

            //CheckTreeViewModel checkTreeViewModel7 = new CheckTreeViewModel() { ViewName = "7777", Parent = checkTreeViewModel2 };





            //checkTreeViewModel1.ChildrenView.Add(checkTreeViewModel2);
            //checkTreeViewModel1.ChildrenView.Add(checkTreeViewModel3);
            //checkTreeViewModel1.ChildrenView.Add(checkTreeViewModel4);

            //checkTreeViewModel2.ChildrenView.Add(checkTreeViewModel5);
            //checkTreeViewModel2.ChildrenView.Add(checkTreeViewModel6);
            //checkTreeViewModel2.ChildrenView.Add(checkTreeViewModel7);





            //CheckTreeModel.Add(checkTreeViewModel1);
            ////CheckTreeModel.Add(checkTreeViewModel2);


            //UpdateCommand = new CommandBase(CalcResult);
        }






        string name = "chen123456";

        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }

        string visibility = "Collapsed";
        public string Visibility
        {
            get { return visibility; }
            set { visibility = value; OnPropertyChanged(nameof(Visibility)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }



        public ICommand UpdateCommand { get; }

        //真正需要执行的计算逻辑
        private void CalcResult(object obj)
        {

            // 反向遍历选中的项，避免修改集合时的索引问题
            for (int i = ReadVariableList.Count - 1; i >= 0; i--)
            {
                var selectedItem = ReadVariableList[i];

                // 从 ReadVariableLists 中移除选中项
                ReadVariableLists.Remove(selectedItem);
            }

            ReadVariableList.Clear();
        }

        private ObservableCollection<CheckTreeViewModel> checkTreeModel = new ObservableCollection<CheckTreeViewModel>();

        public ObservableCollection<CheckTreeViewModel> CheckTreeModel
        {
            get { return checkTreeModel; }
            set { checkTreeModel = value; OnPropertyChanged(nameof(CheckTreeModel)); }
        }



        private ObservableCollection<string> readVariableLists = new ObservableCollection<string>();

        public ObservableCollection<string> ReadVariableLists
        {
            get { return readVariableLists; }
            set { readVariableLists = value; OnPropertyChanged(nameof(ReadVariableLists)); }
        }

        private ObservableCollection<string> readVariableList = new ObservableCollection<string>();

        public ObservableCollection<string> ReadVariableList
        {
            get { return readVariableList; }
            set { readVariableList = value; OnPropertyChanged(nameof(ReadVariableList)); }
        }

        private string color = "Red";
        public string Color
        {
            get { return color; }
            set { color = value; OnPropertyChanged(nameof(Color)); }
        }
        public ObservableCollection<Person> People { get; set; }
    }
    public class Person
    {
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Name3 { get; set; }
        public string Name4 { get; set; }
    }
}
