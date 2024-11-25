using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace sinsegye.ide.opcuaeditor.Model
{
    //自定义的treeView类
    public class CheckTreeViewModel : INotifyPropertyChanged
    {
        public CheckTreeViewModel()
        {
            ChildrenView = new ObservableCollection<CheckTreeViewModel>();


            // 从资源创建 Icon 对象
            Icon myIcon = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("TreeView.OPCUA.ico"));
        }
        public bool viewChecked = false;

        public string ViewName { get; set; }

        public object Tag { get; set; }


        private string _loadingLabel;
        public string LoadingLabel
        {
            get { return _loadingLabel; }
            set
            {
                _loadingLabel = value;
                NotifyPropertyChanged(nameof(LoadingLabel));
            }
        }


        public ObservableCollection<CheckTreeViewModel> ChildrenView { get; set; }

        public CheckTreeViewModel Parent;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }


        // 递归方法获取节点的完整路径
        public string GetFullPath()
        {
            return GetFullPathRecursive(this);
        }

        private string GetFullPathRecursive(CheckTreeViewModel node)
        {
            if (node.Parent == null)
            {
                return node.ViewName;
            }
            else
            {
                return GetFullPathRecursive(node.Parent) + "/" + node.ViewName;
            }
        }
        public string FullPath
        {
            get
            {
                return GetFullPath();
            }
        }

        //获取框选或改变框选时调用
        public Boolean ViewChecked
        {
            get
            {
                return this.viewChecked;
            }
            set
            {
                if (viewChecked != value)
                {
                    this.viewChecked = value;
                    NotifyPropertyChanged("ViewChecked");

                    // 更新子节点的状态  
                    UpdateChildNodes(value);

                    // 更新父节点的状态  
                    //  UpdateParentNode(this, value);
                }
            }
        }



        private void UpdateChildNodes(bool value)
        {
            if (ChildrenView != null && ChildrenView.Count > 0)
            {
                foreach (CheckTreeViewModel item in ChildrenView)
                {
                    item.ViewChecked = value;
                }
            }
        }

        private void UpdateParentNode(CheckTreeViewModel node, bool value)
        {
            if (node.Parent != null)
            {
                node.Parent.ViewChecked = value; // 更新父节点的状态  
                                                 // 递归更新更高层的父节点  
                UpdateParentNode(node.Parent, value);
            }
        }
    }
}
