using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OPCUA_codesysTest
{
    /// <summary>
    /// <see cref="TreeView"/> 扩展
    /// </summary>
    public class TreeViewEx : TreeView
    {
        public TreeViewEx()
        {
            base.HideSelection = false; //重绘必须保证失焦时选中树节点背景色高亮
            base.DrawMode = TreeViewDrawMode.OwnerDrawText; //重绘树节点文本部分
        }

        private const int TreeNodeLeftPadding = 2; //绘制树节点文本时左边缘偏移量
        private Color focusedSelectedNodeForeColor = SystemColors.HighlightText;
        private Color focusedSelectedNodeBackColor = SystemColors.Highlight;
        private Color unfocusedSelectedNodeForeColor = SystemColors.InactiveCaptionText;
        private Color unfocusedSelectedNodeBackColor = SystemColors.InactiveCaption;
        private Color highlightForeColor = Color.Red;

        /// <summary>
        /// 获取或设置聚焦时选定树节点的前景色
        /// </summary>
        [DefaultValue(typeof(Color), "HighlightText")]
        [Description("聚焦时选定树节点的前景色")]
        public Color FocusedSelectedNodeForeColor
        {
            get => this.focusedSelectedNodeForeColor;
            set
            {
                if (!value.IsEmpty)
                    this.focusedSelectedNodeForeColor = value;
            }
        }

        /// <summary>
        /// 获取或设置聚焦时选定树节点的背景色
        /// </summary>
        [DefaultValue(typeof(Color), "Highlight")]
        [Description("聚焦时选定树节点的背景色")]
        public Color FocusedSelectedNodeBackColor
        {
            get => this.focusedSelectedNodeBackColor;
            set
            {
                if (!value.IsEmpty)
                    this.focusedSelectedNodeBackColor = value;
            }
        }

        /// <summary>
        /// 获取或设置失焦时选定树节点的前景色
        /// </summary>
        [DefaultValue(typeof(Color), "InactiveCaptionText")]
        [Description("失焦时选定树节点的前景色")]
        public Color UnfocusedSelectedNodeForeColor
        {
            get => this.unfocusedSelectedNodeForeColor;
            set
            {
                if (!value.IsEmpty)
                    this.unfocusedSelectedNodeForeColor = value;
            }
        }

        /// <summary>
        /// 获取或设置失焦时选定树节点的背景色
        /// </summary>
        [DefaultValue(typeof(Color), "InactiveCaption")]
        [Description("失焦时选定树节点的背景色")]
        public Color UnfocusedSelectedNodeBackColor
        {
            get => this.unfocusedSelectedNodeBackColor;
            set
            {
                if (!value.IsEmpty)
                    this.unfocusedSelectedNodeBackColor = value;
            }
        }

        /// <summary>
        /// 获取或设置树节点高亮部分文本，一般是文本搜索框中的文本，已移除前后空白
        /// </summary>
        /// <remarks>当本属性不为空白字符串时，会不区分大小将树节点文本匹配的子串部分文本你的前景色设置为 <see cref="NodeHighlightForeColor"/></remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string NodeHighlightText { get; set; }

        /// <summary>
        /// 获取或设置绘制控件的模式
        /// </summary>
        /// <remarks>不要随意改变绘制模式。一般当需要进行大量重绘时，会先切换成 <see cref="TreeViewDrawMode.Normal"/> 加载绘制速度</remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new TreeViewDrawMode DrawMode
        {
            get => base.DrawMode;
            set => base.DrawMode = value;
        }

        /// <summary>
        /// 获取或设置一个值，用以指示选定的树节点是否即使在树视图已失去焦点时仍会保持突出显示。此时 <see cref="TreeView.HideSelection"/> 无效
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool HideSelection { get; set; }

        /// <summary>
        /// 获取或设置树节点高亮部分前景色
        /// </summary>
        [DefaultValue(typeof(Color), "Red")]
        [Description("树节点高亮部分前景色")]
        public Color NodeHighlightForeColor
        {
            get => this.highlightForeColor;
            set
            {
                if (!value.IsEmpty)
                    this.highlightForeColor = value;
            }
        }

        protected override void WndProc(ref Message m)
        {
            //若显示复选框，则屏蔽鼠标左键双击事件
            if (!(this.CheckBoxes && m.Msg == 0x0203))
                base.WndProc(ref m);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams p = base.CreateParams;
                if (!this.ShowNodeToolTips)
                    p.Style |= 0x80; //屏蔽工具提示文本

                p.ExStyle |= 0x02000000; //解决闪烁
                return p;
            }
        }

        protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
        {
            //先选中打开点击的书节点，再响应事件
            if (e.Button == MouseButtons.Right)
                this.SelectedNode = e.Node;

            base.OnNodeMouseClick(e);
        }

        protected override void OnAfterCheck(TreeViewEventArgs e)
        {
            //若存在复选框，且是由鼠标点击的，则先选中打开点击的书节点，改变关联的父子节点的复选框勾选状态，再响应事件
            if (this.CheckBoxes)
            {
                if (e.Action == TreeViewAction.ByMouse || e.Action == TreeViewAction.ByKeyboard)
                {
                    this.ChangeChildrenCheckedState(e.Node, e.Node.Checked); //改变子孙树节点选中状态
                    this.ChangeParentCheckedState(e.Node); //改变父爷树节点选中状态
                    this.SelectedNode = e.Node;
                }
            }

            base.OnAfterCheck(e);
        }

        private void ChangeChildrenCheckedState(TreeNode f_TreeNode, bool f_Checked)
        {
            f_TreeNode.Checked = f_Checked;
            foreach (TreeNode node in f_TreeNode.Nodes)
            {
                this.ChangeChildrenCheckedState(node, f_Checked);
            }
        }

        private void ChangeParentCheckedState(TreeNode f_TreeNode)
        {
            //若兄弟节点只要有一个被选，则其父节点也被选，反之，兄弟节点全没选，则其父节点也不选
            TreeNode tmpParentNode = f_TreeNode?.Parent;
            if (tmpParentNode != null)
            {
                tmpParentNode.Checked = tmpParentNode.Nodes.Cast<TreeNode>().Any(node => node.Checked);
                this.ChangeParentCheckedState(tmpParentNode);
            }
        }

        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            base.OnDrawNode(e);

            //若本树节点处于编辑模式，则将背景色设置为树视图背景色，且不绘制文本，防止当前编辑的文本过短时会显示之前的文本
            if (e.Node.IsEditing)
            {
                using (var tmpBrush = new SolidBrush(this.BackColor))
                    e.Graphics.FillRectangle(tmpBrush, e.Node.Bounds);

                return;
            }

            //注意节点的字体或颜色可能未初始化，一般若没有显式设置都未初始化，此时使用树视图的
            Font font = e.Node.NodeFont ?? this.Font;
            Color foreColor;
            Color backColor;

            //选中但未聚焦表示上一次选中的树节点；未选中但已聚焦表示即将要选中的树节点
            //优先采用节点自身的颜色，当显式设置节点颜色时，才不为空，目前显式设置存在拖拽或未标记表
            if ((e.State & TreeNodeStates.Focused) > 0)
            {
                //表示即将选中的树节点，这里一般可能是拖拽中
                foreColor = e.Node.BackColor.IsEmpty ? this.FocusedSelectedNodeForeColor : e.Node.ForeColor;
                backColor = e.Node.BackColor.IsEmpty ? this.FocusedSelectedNodeBackColor : e.Node.BackColor;
            }
            else if ((e.State & TreeNodeStates.Selected) > 0 && !this.Focused)
            {
                //表示上次选中的树节点，选中树节点改变或失焦时都会进来
                foreColor = e.Node.BackColor.IsEmpty ? this.UnfocusedSelectedNodeForeColor : e.Node.ForeColor;
                backColor = e.Node.BackColor.IsEmpty ? this.UnfocusedSelectedNodeBackColor : e.Node.BackColor;
            }
            else
            {
                foreColor = e.Node.ForeColor.IsEmpty ? this.ForeColor : e.Node.ForeColor;
                backColor = e.Node.BackColor.IsEmpty ? this.BackColor : e.Node.BackColor;
            }

            Rectangle bounds = e.Node.Bounds;
            using (var tmpBrush = new SolidBrush(backColor))
                e.Graphics.FillRectangle(tmpBrush, bounds);

            int x = bounds.X + TreeNodeLeftPadding;
            int y = bounds.Y + (bounds.Height - font.Height) / 2;
            var tmpBounds = new Rectangle(x, y, bounds.Right - x, bounds.Bottom - y);
            TextRenderer.DrawText(e.Graphics, e.Node.Text, font, tmpBounds, foreColor, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
        }
    }
}
