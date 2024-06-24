using ReactiveUI;
using System.Collections.ObjectModel;
using xx.vv.Views.UserMgr;

namespace SP7020Test.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public MainWindowViewModel()
        {
            Users.Add(new UserViewModel()
            {
                Name = "Test",
                Isused = true,
            });
            Users.Add(new UserViewModel()
            {
                Name = "Test2",
                Isused = false,
            });
        }


        private ObservableCollection<UserViewModel> _Users = new ObservableCollection<UserViewModel>();
        public ObservableCollection<UserViewModel> Users
        {
            get => _Users;
            set => this.RaiseAndSetIfChanged(ref _Users, value);
        }
    }
}
