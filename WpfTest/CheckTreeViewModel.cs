using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace WpfTest
{
    // 自定义的 TreeViewModel 类
    public class CheckTreeViewModel : INotifyPropertyChanged
    {
        private static readonly DummyCheckTreeViewModel DummyItem = new DummyCheckTreeViewModel();

        private class DummyCheckTreeViewModel : CheckTreeViewModel
        {
            public DummyCheckTreeViewModel()
                : base()
            {
            }
        }


        public CheckTreeViewModel()
        {
            ChildrenView = new ObservableCollection<CheckTreeViewModel>();
        }



        // 子节点集合
        public ObservableCollection<CheckTreeViewModel> ChildrenView { get; set; }

        // 父节点
        public CheckTreeViewModel Parent { get; set; }

        // 节点名称
        private string _viewName;
        public string ViewName
        {
            get => _viewName;
            set
            {
                _viewName = value;
                NotifyPropertyChanged(nameof(ViewName));
            }
        }






        // 实现 INotifyPropertyChanged 接口
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }















        // 三态布尔，支持部分选中
        private bool? _viewChecked = false;
        public bool? ViewChecked
        {
            get => _viewChecked;
            set
            {
                if (!value.HasValue)
                    throw new ArgumentException(@"Cannot set to null directly", "value");

                // Do nothing if the child is unchanged.
                if (_viewChecked == value)
                    return;

                _viewChecked = value;

                UpdateCheckedChildren();

                if (Parent != null)
                    Parent.UpdateCheckedSiblingsAndParent(this, this);
                else
                    RaiseCheckedChanged(this);


                NotifyPropertyChanged(nameof(ViewChecked));
            }
        }

        public event EventHandler<CheckedChangedEventArgs> CheckedChanged;
        private void RaiseCheckedChanged(CheckTreeViewModel item)
        {
            var handler = CheckedChanged;
            if (handler != null)
                handler(item, new CheckedChangedEventArgs(item.ViewName, item.ViewChecked));
        }


        private void UpdateCheckedChildren()
        {
            UpdateChildren(x => x._viewChecked, (x, y) => x._viewChecked = y, _viewChecked, x => x.NotifyPropertyChanged(nameof(ViewChecked)));
        }
        private void UpdateCheckedSiblingsAndParent(CheckTreeViewModel sender, CheckTreeViewModel child)
        {
            UpdateSiblingsAndParent(child, x => x._viewChecked, (x, y) => x._viewChecked = y, _viewChecked, x => x.NotifyPropertyChanged(nameof(ViewChecked)), x => x.NotifyPropertyChanged(nameof(sender)));
        }

        private void UpdateSiblingsAndParent(CheckTreeViewModel child, Func<CheckTreeViewModel, bool?> getter, Action<CheckTreeViewModel, bool?> setter, bool? value, Action<CheckTreeViewModel> notify, Action<CheckTreeViewModel> notifySender)
        {
            if (value != getter(child))
            {
                // If the child has a value and all children share the same value then that is our state, otherwise
                // the state is null.
                var state =
                    (getter(child).HasValue && ChildrenView.All(x => getter(x) == getter(child)))
                        ? getter(child)
                        : null;

                // Does this change the state of this node?
                if (value != state)
                {
                    // Set the new state and raise an event.
                    setter(this, state);
                    notify(this);
                }
            }

            if (Parent != null)
                Parent.UpdateSiblingsAndParent(this, getter, setter, value, notify, notifySender);
            else
                notifySender(this);
        }


        private void UpdateChildren(Func<CheckTreeViewModel, bool?> getter, Action<CheckTreeViewModel, bool?> setter, bool? value, Action<CheckTreeViewModel> notify)
        {
            // If we haven't dot a definate selection state, or we haven't yet loaded the children, go no further.
            if (!value.HasValue || ChildrenView.Contains(DummyItem)) return;

            // Find children with a different selection state from ourself.
            foreach (var child in ChildrenView.Where(child => getter(child) != value))
            {
                // Set the child state and raise the event.
                setter(child, value);
                notify(child);

                // Update the childs children.
                child.UpdateChildren(getter, setter, value, notify);
            }
        }

    }
}
