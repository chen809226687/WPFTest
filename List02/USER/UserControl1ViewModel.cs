using List02.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace List02.USER
{
    public class UserControl1ViewModel: ViewModelBase
    {

        private string _Parameter1 = "123456789";
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

    }
}
