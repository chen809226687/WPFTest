using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OPC_UA_WPF
{//自定义的treeView类
    public class CheckTreeView : INotifyPropertyChanged
    {
        public bool viewChecked = false;


        private string _labelText;

        public string LabelText
        {
            get { return _labelText; }
            set
            {
                _labelText = value;
                NotifyPropertyChanged(nameof(LabelText));
            }
        }


        public string ViewName { get; set; }

        // 递归方法获取节点的完整路径
        public string GetFullPath()
        {
            return GetFullPathRecursive(this);
        }

        private string GetFullPathRecursive(CheckTreeView node)
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

        public bool IsChecked { get; set; }

        public object Tag { get; set; }
        public List<CheckTreeView> ChildrenView { get; set; } = new List<CheckTreeView>();
        public List<CheckTreeView> AllView = new List<CheckTreeView>();

        public CheckTreeView Parent;
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }

        }
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
                foreach (CheckTreeView item in ChildrenView)
                {
                    item.ViewChecked = value;
                }
            }
        }

        private void UpdateParentNode(CheckTreeView node, bool value)
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
