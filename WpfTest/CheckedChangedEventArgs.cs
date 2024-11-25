using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTest
{
    public class CheckedChangedEventArgs
    {
        public CheckedChangedEventArgs(object value, bool? isChecked)
        {
            Value = value;
            IsChecked = isChecked;
        }

        public object Value { get; private set; }
        public bool? IsChecked { get; private set; }
    }
}
