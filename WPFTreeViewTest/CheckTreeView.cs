using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFTreeViewTest
{//自定义的treeView类
    public class CheckTreeView : INotifyPropertyChanged
    {
        public Boolean? viewChecked;


        public string ViewName { get; set; }

        public string ID { get; set; }
        public List<CheckTreeView> ChildrenView { get; set; } 
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

        //获取框选或改变框选时调用
        public Boolean? ViewChecked
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
                    if (ChildrenView != null && ChildrenView.Count > 0)
                    {
                        if (this.viewChecked == true)
                        {
                            foreach (CheckTreeView item in ChildrenView)
                            {
                                item.ViewChecked = true;

                            }
                        }
                        if (this.viewChecked == false)
                        {
                            foreach (CheckTreeView item in ChildrenView)
                            {
                                item.ViewChecked = false;
                            }

                        }

                    }
                    else
                    {
                        if (this.Parent != null)
                        {
                            int trueCount = 0;
                            int falseCount = 0;
                            foreach (CheckTreeView item in this.Parent.ChildrenView)
                            {
                                if (item.viewChecked == true)
                                {
                                    trueCount++;
                                }

                                if (item.viewChecked == false)
                                {
                                    falseCount++;
                                }
                            }
                            if (trueCount == 0 && falseCount == 0)
                            {
                                this.Parent.ViewChecked = false;
                            }
                            if (trueCount > 0 && falseCount > 0)
                            {
                                this.Parent.ViewChecked = null;
                            }
                            if (trueCount > 0 && falseCount == 0)
                            {
                                this.Parent.ViewChecked = true;
                            }
                            if (trueCount == 0 && falseCount > 0)
                            {
                                this.Parent.ViewChecked = false;
                            }

                        }

                    }
                }
            }
        }

        //初始化数据源
        public List<CheckTreeView> GetAllViewItemFromDataTable(DataTable dataGroup, DataTable dataInfo)
        {

            for (int i = 0; i < dataGroup.Rows.Count + 1; i++)
            {
                CheckTreeView ck = new CheckTreeView();
                if (i < dataGroup.Rows.Count)
                {
                    ck.ViewName = dataGroup.Rows[i]["FgroupName"].ToString();//所有群组名称默认为FgroupName 报错请改数据库字段

                    ck.ID = dataGroup.Rows[i]["ID"].ToString() + ".";
                    DataRow[] onGroup = dataInfo.Select(" GroupId like '" + dataGroup.Rows[i]["fid"].ToString() + "'");//所有群组内物标名称默认为GroupId 报错请改数据库字段

                    Boolean? isCheck = false;
                    for (int j = 0; j < onGroup.Length; j++)
                    {
                        CheckTreeView ckChil = new CheckTreeView();
                        ckChil.viewChecked = Convert.ToInt32(onGroup[j]["IsCheck"].ToString()) == 1 ? true : false;
                        ckChil.Parent = ck;
                        if (ckChil.viewChecked == true)
                        {
                            isCheck = true;

                        }
                        else
                        {
                            if (isCheck == true)
                            {
                                isCheck = null;
                            }
                        }
                        ckChil.ViewName = onGroup[j]["Fobjname"].ToString();
                        ckChil.ID = onGroup[j]["ID"].ToString();
                        if (ck.ChildrenView == null)
                        {
                            ck.ChildrenView = new List<CheckTreeView>();
                        }
                        ck.ChildrenView.Add(ckChil);
                    }
                    ck.ViewChecked = isCheck;

                }
                else
                {
                    ck.ViewName = "未分组标绘";

                    ck.ID = null;
                    DataRow[] onGroup = dataInfo.Select(" GroupId = ''");//所有群组内物标名称默认为GroupId 报错请改数据库字段

                    Boolean? isCheck = false;
                    for (int j = 0; j < onGroup.Length; j++)
                    {
                        CheckTreeView ckChil = new CheckTreeView();
                        ckChil.viewChecked = Convert.ToInt32(onGroup[j]["IsCheck"].ToString()) == 1 ? true : false;
                        ckChil.Parent = ck;
                        if (ckChil.viewChecked == true)
                        {
                            isCheck = true;

                        }
                        else
                        {
                            if (isCheck == true)
                            {
                                isCheck = null;
                            }
                        }
                        ckChil.ViewName = onGroup[j]["Fobjname"].ToString();
                        ckChil.ID = onGroup[j]["ID"].ToString();
                        if (ck.ChildrenView == null)
                        {
                            ck.ChildrenView = new List<CheckTreeView>();
                        }
                        ck.ChildrenView.Add(ckChil);
                    }
                    ck.ViewChecked = isCheck;
                }
                AllView.Add(ck);

            }
            return AllView;
        }


    }

}
