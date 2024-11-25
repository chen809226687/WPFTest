using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Sinsegye.Ide.Resources;
using Sinsegye.Ide.Resources.Helpers;
using Sinsegye.Ide.Resources.Input;

namespace SinsegyeControlTest
{
    public class TabItemModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ICommand closecommand { get; } = new RelayCommand<object>(OnCloseCommand);
        private static void OnCloseCommand(object aaa)
        {


        }
    }

    internal class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TabItemModel> TabItems { get; set; } = new ObservableCollection<TabItemModel> {
            new TabItemModel{ Name ="111"},
             new TabItemModel{ Name ="211"},
              new TabItemModel{ Name ="121"},
        };
        public MainViewModel()
        {

            //PeopleList = new ObservableCollection<Person>
            //{
            //    new Person { ID = "001", Name = "张三",Sex=true,Age="18",Children=new ObservableCollection<Person>(),
            //    new Person { ID = "002", Name = "李四",Sex=true,Age="19" },
            //    new Person { ID = "003", Name = "王二",Sex=true,Age="28" },
            //    new Person { ID = "004", Name = "麻子",Sex=true,Age="17" },
            //    new Person { ID = "005", Name = "陈五",Sex=true,Age="19" },
            //};

        }


        public ICommand closecommand { get; set; } = new RelayCommand<object>(OnCloseCommand);
        private static void OnCloseCommand(object aaa)
        {


        }


        private ObservableCollection<Person> peopleList;

        public ObservableCollection<Person> PeopleList
        {
            get { return peopleList; }
            set { peopleList = value; OnPropertyChanged(nameof(PeopleList)); }
        }










        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }

    public class Person
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public bool Sex { get; set; }
        public string Age { get; set; }

        public ObservableCollection<Person> Children { get; set; }
    }
}
