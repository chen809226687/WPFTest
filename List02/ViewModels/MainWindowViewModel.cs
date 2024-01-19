using ReactiveUI;
using System.Collections.ObjectModel;

namespace List02.ViewModels
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

        private Formula _Fo=new Formula();
        public Formula Fo
        {
            get => _Fo;
            set => this.RaiseAndSetIfChanged(ref _Fo, value);
        }
    }
}
