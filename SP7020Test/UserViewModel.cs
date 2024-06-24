
using ReactiveUI;
using SP7020Test.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xx.vv.Views.UserMgr
{
    public class UserViewModel : ViewModelBase
    {

        private string _Name;
        public string Name
        {
            get => _Name;
            set => this.RaiseAndSetIfChanged(ref _Name, value);
        }


        private bool _Isused;
        public bool Isused
        {
            get => _Isused;
            set => this.RaiseAndSetIfChanged(ref _Isused, value);
        }


    }
}
