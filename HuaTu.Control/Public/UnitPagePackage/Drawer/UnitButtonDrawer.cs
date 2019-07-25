
using HuaTu.Controls.Public.UnitPagePackage.Enums;
using HuaTu.Controls.Public.UnitPagePackage.Data;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace HuaTu.Controls.Public.UnitPagePackage.Drawer
{
    public abstract class UnitButtonDrawer
    {
        #region 字段.

        private Color _textColor = Color.Black;
        private Color _backGradient1 = Color.FromArgb(240, 240, 240);
        private Color _backGradient2 = Color.White;
        private Color _border1 = Color.White;
        private Color _border2 = Color.DarkGray;
        private Color _sbackGradient1 = Color.White;
        private Color _sbackGradient2 = Color.FromArgb(224, 223, 227);
        private Color _sborder1 = Color.White;
        private Color _sborder2 = Color.DarkGray;
        private LinearGradientMode _backGradientMode = LinearGradientMode.Vertical;

        #endregion 字段.

        #region 构造器.

        /// <summary>
        /// 构造器
        /// </summary>
        protected UnitButtonDrawer()
        {
        }

        #endregion 构造器.

        #region 公开函数

        /// <summary>
        /// 文字颜色
        /// </summary>
        public Color TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        /// <summary>
        /// 边框颜色1
        /// </summary>
        public Color Border1
        {
            get { return _border1; }
            set { _border1 = value; }
        }

        /// <summary>
        /// 边框颜色2
        /// </summary>
        public Color Border2
        {
            get { return _border2; }
            set { _border2 = value; }
        }

        /// <summary>
        /// 背景渐变颜色1
        /// </summary>
        public Color BackGradient1
        {
            get { return _backGradient1; }
            set { _backGradient1 = value; }
        }

        /// <summary>
        /// 背景渐变颜色2
        /// </summary>
        public Color BackGradient2
        {
            get { return _backGradient2; }
            set { _backGradient2 = value; }
        }

        /// <summary>
        /// 所选边框颜色1
        /// </summary>
        public Color SelectedBorder1
        {
            get { return _sborder1; }
            set { _sborder1 = value; }
        }

        /// <summary>
        /// 所选边框颜色2
        /// </summary>
        public Color SelectedBorder2
        {
            get { return _sborder2; }
            set { _sborder2 = value; }
        }

        /// <summary>
        /// 所选背景渐变颜色1
        /// </summary>
        public Color SelectedBackGradient1
        {
            get { return _sbackGradient1; }
            set { _sbackGradient1 = value; }
        }

        /// <summary>
        /// 所选背景渐变颜色2
        /// </summary>
        public Color SelectedBackGradient2
        {
            get { return _sbackGradient2; }
            set { _sbackGradient2 = value; }
        }


        /// <summary>
        /// 背景渐变模式
        /// </summary>
        public LinearGradientMode BackGradientMode
        {
            get { return _backGradientMode; }
            set { _backGradientMode = value; }
        }

        /// <summary>
        /// 画一个单元按钮
        /// </summary>
        /// <param name="bounds">按钮边界</param>
        /// <param name="text">按钮文本</param>
        /// <param name="selected">应用被选的标志</param>
        /// <param name="font">绘制文本字体</param>
        /// <param name="icon">按钮图标</param>
        /// <param name="graphics">绘图对象</param>
        public abstract void Draw(Rectangle bounds, string text, bool selected, Font font, Icon icon, Graphics graphics);

        /// <summary>
        /// 绘制按钮线条
        /// </summary>
        /// <param name="graphics">绘图对象</param>
        /// <param name="panelBounds">面板边界</param>
        /// <param name="buttonsPanelBounds">按钮面板边界</param>
        public abstract void DrawButtonsLine(Graphics graphics, Rectangle panelBounds, Rectangle buttonsPanelBounds);

        /// <summary>
        /// 绘制向后滚动按钮
        /// </summary>
        /// <param name="bounds">按钮边界</param>
        /// <param name="state">按钮状态标识符</param>
        /// <param name="graphics">绘画对象</param>
        public abstract void DrawScrollBackButton(Rectangle bounds, UnitButtonState state, Graphics graphics);

        /// <summary>
        /// 绘制下一个滚动按钮
        /// </summary>
        /// <param name="bounds">按钮边界</param>
        /// <param name="state">按钮状态标识符</param>
        /// <param name="graphics">绘画对象</param>
        public abstract void DrawScrollNextButton(Rectangle bounds, UnitButtonState state, Graphics graphics);

        /// <summary>
        /// 检查面板是否有滚动
        /// </summary>
        /// <param name="panelBounds">面板边界</param>
        /// <param name="lastButtonBounds">最后一个面板边界</param>
        /// <returns>如果面板滚动返回true</returns>
        public abstract bool HasScroll(Rectangle panelBounds, Rectangle lastButtonBounds);

        /// <summary>
        /// 获取向后滚动按钮的边界
        /// </summary>
        /// <param name="panelBounds">面板边界</param>
        public abstract Rectangle GetScrollBackButtonBounds(Rectangle panelBounds);

        /// <summary>
        /// 获取滚动下一步按钮的边界
        /// </summary>
        /// <param name="panelBounds">面板边界</param>
        /// <returns>滚动下一个按钮的边界</returns>
        public abstract Rectangle GetScrollNextButtonBounds(Rectangle panelBounds);

        /// <summary>
        /// 获得第一个按钮边界
        /// </summary>
        /// <param name="panelBounds">面板边界</param>
        /// <param name="hasScroll">指示面板有滚动的标志</param>
        /// <param name="scrolPos">滚动位置</param>
        /// <param name="text">按钮文本</param>
        /// <param name="font">按钮字体</param>
        /// <param name="icon">按钮图标</param>
        /// <param name="graphics">绘画对象</param>
        public abstract Rectangle GetFirstButtonBounds(Rectangle panelBounds, bool hasScroll, int scrolPos, string text, Font font, Icon icon, Graphics graphics);

        /// <summary>
        /// 获取下一个按钮边界
        /// </summary>
        /// <param name="previousBounds">上一个按钮的边界</param>
        /// <param name="text">按钮文本</param>
        /// <param name="font">按钮字体</param>
        /// <param name="icon">按钮图标</param>
        /// <param name="graphics">绘画对象</param>
        public abstract Rectangle GetNextButtonBounds(Rectangle previousBounds, string text, Font font, Icon icon, Graphics graphics);

        /// <summary>
        /// 获取面板边界的客户端矩形
        /// </summary>
        /// <param name="panelBounds">面板边界</param>
        public abstract Rectangle GetClientRectangle(Rectangle panelBounds);

        /// <summary>
        /// 获取按钮剪辑矩形
        /// </summary>
        /// <param name="panelBounds">面板边界</param>
        /// <param name="hasScroll">如果面板有滚动，则为真</param>
        /// <param name="captionButtonsCount">可在按钮面板上绘制的标题按钮数</param>
        public abstract Rectangle GetButtonsClipRectangle(Rectangle panelBounds, bool hasScroll, int captionButtonsCount);

        /// <summary>
        /// 获取将包含在面板按钮上绘制的标题按钮的矩形
        /// </summary>
        /// <param name="panelBounds">面板边界</param>
        /// <param name="captionButtonsCount">按钮标题数</param>
        public abstract Rectangle GetCaptionButtonsRectangle(Rectangle panelBounds, int captionButtonsCount);

        /// <summary>
        /// 获取标题按钮索引
        /// </summary>
        /// <param name="captionButtonsBounds">标题按钮块的边界</param>
        /// <param name="point">指向搜索按钮的位置</param>
        public abstract int GetCaptionButtonIndex(Rectangle captionButtonsBounds, Point point);

        /// <summary>
        /// 可以撤消下一个按钮的位移（下一个按钮之前已被替换）
        /// </summary>
        /// <param name="displacedButton">替代按钮</param>
        /// <param name="selectedButton">被选中按钮</param>
        /// <param name="mouseLocation">当前鼠标位置</param>
        internal abstract bool CanUndoDisplaceNext(UnitButton displacedButton, UnitButton selectedButton, Point mouseLocation);

        /// <summary>
        /// 可以撤消上一个按钮的位移（上一个按钮之前已被位移）
        /// </summary>
        /// <param name="displacedButton">替代按钮</param>
        /// <param name="selectedButton">被选中按钮</param>
        /// <param name="mouseLocation">当前鼠标位置</param>
        internal abstract bool CanUndoDisplaceBack(UnitButton displacedButton, UnitButton selectedButton, Point mouseLocation);

        /// <summary>
        /// 检查是否可以滚动到下一个
        /// </summary>
        /// <param name="lastButton">上一个按钮</param>
        /// <param name="scrollNextBounds">滚动下一个边界</param>
        /// <returns>true if can scroll next</returns>
        internal abstract bool CanScrollNext(UnitButton lastButton, Rectangle scrollNextBounds);

        /// <summary>
        /// 获取滚动位置
        /// </summary>
        /// <param name="buttons">按钮集合</param>
        /// <param name="firstShownButtonIndex">从零开始的第一个显示按钮索引</param>
        internal abstract int GetScrollPos(IList<UnitButton> buttons, int firstShownButtonIndex);

        #endregion 公开函数

        #region 可继承函数

        /// <summary>
        /// 获取边框颜色1
        /// </summary>
        /// <param name="selected">指示请求所选颜色的标志</param>
        protected Color GetBorder1Color(bool selected)
        {
            if (selected)
            {
                return SelectedBorder1;
            }

            return Border1;
        }

        /// <summary>
        /// 获得边框颜色2
        /// </summary>
        /// <param name="selected">指示请求所选颜色的标志</param>
        protected Color GetBorder2Color(bool selected)
        {
            if (selected)
            {
                return SelectedBorder2;
            }

            return Border2;
        }

        /// <summary>
        /// 返回渐变颜色1
        /// </summary>
        /// <param name="selected">指示请求所选颜色的标志</param>
        protected Color GetBackGradient1Color(bool selected)
        {
            if (selected)
            {
                return SelectedBackGradient1;
            }

            return BackGradient1;
        }

        /// <summary>
        /// 返回渐变色2
        /// </summary>
        /// <param name="selected">指示请求所选颜色的标志</param>
        protected Color GetBackGradient2Color(bool selected)
        {
            if (selected)
            {
                return SelectedBackGradient2;
            }

            return BackGradient2;
        }

        #endregion 可继承函数
    }
}
