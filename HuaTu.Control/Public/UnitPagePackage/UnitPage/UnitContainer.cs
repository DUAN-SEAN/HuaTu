using HuaTu.Controls.Public.UnitPagePackage.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuaTu.Controls.Public.UnitPagePackage.UnitPage
{
    public partial class UnitContainer : ButtonContainer
    {
        #region 构造器.

 
        public UnitContainer()
        {
            InitializeComponent();
        }

        #endregion 构造器.

        #region 公开函数

        /// <summary>
        /// 委托通知所选内容已更改
        /// </summary>
        public delegate void OnPageSelectionMade(Object sender, EventArgs e);

        /// <summary>
        /// 通知所选内容已更改的事件
        /// </summary>
        public event OnPageSelectionMade PageSelectionMade;


        /// <summary>
        /// 获取可用页的计数
        /// </summary>
        public int Count
        {
            get { return base.ButtonsCount; }
        }

        /// <summary>
        /// 添加新选项卡页
        /// </summary> 
        public void Add(UnitPage page)
        {
            page.SetBounds(-15000, 0, _pagesPanel.Width, _pagesPanel.Height);
            page.Button.Text = page.Name;
            _pagesPanel.Controls.Add(page);
            AddButton(page.Button);
        }

        /// <summary>
        /// 删除现有选项卡页
        /// </summary>
        public void Remove(UnitPage page)
        {
            _pagesPanel.Controls.Remove(page);
            RemoveButton(page.Button);
        }

        /// <summary>
        /// 在给定索引处获取页面
        /// </summary>
        public UnitPage GetPageAt(int pageIndex)
        {
            UnitButton buton = GetButtonAt(pageIndex);
            return (UnitPage)buton.Page;
        }

        #endregion 公开函数

        #region 可继承函数

        
        protected override void OnPagesPanelBoundsChanged(Rectangle bounds)
        {
            if (_pagesPanel != null)
            {
                _pagesPanel.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
            }

            base.OnPagesPanelBoundsChanged(bounds);
        }

        protected override void OnSelectedIndexSet(EventArgs e)
        {
            if (Count > 0)
            {
                for (int index = 0; index < Count; index++)
                {
                    UnitPage page = GetPageAt(index);

                    if (index == SelectedIndex)
                    {
                        page.SetBounds(0, 0, _pagesPanel.Width, _pagesPanel.Height);
                        page.Visible = true;
                    }
                    else
                    {
                        page.SetBounds(-110, 110, _pagesPanel.Width, _pagesPanel.Height);
                        page.Visible = false;
                    }
                }
                PageSelectionMade(GetPageAt(SelectedIndex), e);
            }

            base.OnSelectedIndexSet(e);
        }

        #endregion 可继承函数


        private void UnitContainer_Load(object sender, EventArgs e)
        {

        }
    }
}
