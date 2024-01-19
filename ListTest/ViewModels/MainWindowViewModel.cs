using Avalonia.Controls;
using Avalonia.Interactivity;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;

namespace ListTest.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {



        public ObservableCollection<Formula> _Formula = new ObservableCollection<Formula>(){

            new Formula()
            {
                Name = "配方1",
                parameter1 = "1的参数1",
                parameter2 = "1的参数2"
            },
            new Formula()
            {
                Name = "配方2",
                parameter1 = "2的参数1",
                parameter2 = "2的参数2"
            },
            new Formula()
            {
                Name = "配方3",
                parameter1 = "3的参数1",
                parameter2 = "3的参数2"
            },


         };
        public ObservableCollection<Formula> Formula
        {
            get => _Formula;
            set => this.RaiseAndSetIfChanged(ref _Formula, value);
        }


        private Formula _Form = new Formula();
        public Formula Form
        {
            get => _Form;
            set
            {
                this.RaiseAndSetIfChanged(ref _Form, value);

                OnSelectionChanged();
            }
        }
        private void OnSelectionChanged()
        {
            Parameter1 = Form.parameter1;
            Parameter2 = Form.parameter2;
            // 这里处理选项改变后的逻辑
        }


        #region 绑定数据


        private string _Parameter1;
        public string Parameter1
        {
            get => _Parameter1;
            set => this.RaiseAndSetIfChanged(ref _Parameter1, value);
        }

        private string _Parameter2;
        public string Parameter2
        {
            get => _Parameter2;
            set => this.RaiseAndSetIfChanged(ref _Parameter2, value);
        }

        #endregion



        public void test()
        {

        }

    }
}
