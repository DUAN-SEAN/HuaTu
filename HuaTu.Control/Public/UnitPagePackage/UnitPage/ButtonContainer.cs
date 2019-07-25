using HuaTu.Controls.Internal;
using HuaTu.Controls.Public.UnitPagePackage.Drawer;
using HuaTu.Controls.Public.UnitPagePackage.Enums;
using HuaTu.Controls.Public.UnitPagePackage.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuaTu.Controls.Public.UnitPagePackage.UnitPage
{
    public partial class ButtonContainer : UserControl
    {

        #region 字段

        private const int ButtonMargin = 16;
        private const int ButtonHeight = 16;

        private Timer _scrollMoveTimer = new Timer();

        private int _selectedIndex = -1;
        private List<UnitButton> _buttons = new List<UnitButton>();

        private UnitButtonDrawer _buttonsRenderer = null;

        private Color _backGradient1 = SystemColors.Control;
        private Color _backGradient2 = SystemColors.ControlLightLight;
        private LinearGradientMode _backGradientMode = LinearGradientMode.Horizontal;

        private bool _cutRoundRect = true;

        private bool _isMouseDownInScrollBackButton = false;
        private bool _isMouseDownInScrollNextButton = false;
        private bool _isMouseOverScrollBackButton = false;
        private bool _isMouseOverScrollNextButton = false;

        private bool _isMouseDownInTabButton = false;
        private bool _isMovingTabButton = false;
        private bool _isDraggingTabButton = false;
        private UnitButton _buttonDisplaced = null;

        private Rectangle _clientBounds = new Rectangle();
        private Rectangle _buttonsPanelBounds = new Rectangle();
        private Rectangle _pagesPanelBounds = new Rectangle();
        private bool _updatePositionsOnDraw = false;
        private bool _hasScrolls = false;
        private bool _canScrollNext = false;
        private int _firstShownButtonIndex = 0;

        private Rectangle _scrollBackBounds = new Rectangle();
        private Rectangle _scrollNextBounds = new Rectangle();

        private Rectangle _captionButtonsBounds = new Rectangle();
        private int _captionButtonIndexUnderMouse = -1;

        private bool _showOneTabButton = true;

        #endregion 字段

        #region 构造器
        public ButtonContainer()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.UserPaint, true);
        }
        #endregion

        #region 公开函数

        /// <summary>
        /// 按钮绘制器
        /// </summary>
        public UnitButtonDrawer ButtonsRenderer
        {
            get
            {
                if (_buttonsRenderer == null)
                {
                    _buttonsRenderer = new TopUnitButtonDrawer();
                    OnTabButonRendererChanged();
                }

                return _buttonsRenderer;
            }
            set
            {
                if (_buttonsRenderer != value)
                {
                    _buttonsRenderer = value;
                    OnTabButonRendererChanged();
                    Invalidate();
                }
            }
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
        /// 背景渐变模式
        /// </summary>
        public LinearGradientMode BackGradientMode
        {
            get { return _backGradientMode; }
            set { _backGradientMode = value; }
        }


        /// <summary>
        /// 如果只有一个按钮，则将此值设置为true以显示选项卡按钮
        /// </summary>
        public bool ShowOneTabButton
        {
            get { return _showOneTabButton; }
            set
            {
                if (_showOneTabButton != value)
                {
                    _showOneTabButton = value;
                    _updatePositionsOnDraw = true;
                }
            }
        }


        /// <summary>
        /// 基于零的选定按钮索引
        /// </summary>
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (_selectedIndex != value)
                {
                    if ((value < 0 || value >= _buttons.Count) && _buttons.Count > 0)
                    {
                        throw new IndexOutOfRangeException("Valid values are between 0 and " + _buttons.Count.ToString() + ".");
                    }

                    _selectedIndex = value;

                    Invalidate();
                }

                OnSelectedIndexSet(EventArgs.Empty);
            }
        }

        #endregion 公开函数

        #region 可继承函数

        /// <summary> 
        /// 清除所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_scrollMoveTimer != null)
                {
                    _scrollMoveTimer.Enabled = false;
                    _scrollMoveTimer.Tick -= OnScrollMoveTimeOccurence;
                    _scrollMoveTimer.Dispose();
                    _scrollMoveTimer = null;
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// 在需要绘制时发生
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_clientBounds != ClientRectangle || _updatePositionsOnDraw)
            {
                _clientBounds = ClientRectangle;
                _updatePositionsOnDraw = false;

                UpdatePositions(e.Graphics, false);
            }

            OnPaintPanelBackground(e);


            if (CaptionButtonsCount > 0)
            {
                OnDrawCaptionButtons(_captionButtonsBounds, e.Graphics);
            }

            DrawRoundBorder(e);

            int buttonsCount = _buttons.Count;
            if (buttonsCount == 1 && ShowOneTabButton == false)
            {
                buttonsCount = 0;
            }

            if (buttonsCount > 0)
            {
                DrawButtonsLine(e);

                RectangleF clip = e.Graphics.ClipBounds;

                e.Graphics.SetClip(_buttonsPanelBounds);

                for (int index = buttonsCount - 1; index >= 0; index--)
                {
                    if (index != SelectedIndex)
                    {
                        _buttons[index].Draw(ButtonsRenderer, false, Font, e.Graphics);
                    }
                }

                _buttons[SelectedIndex].Draw(ButtonsRenderer, true, Font, e.Graphics);

                e.Graphics.SetClip(clip);
            }


            if (_hasScrolls && buttonsCount > 1)
            {
                UnitButtonState stateBack = UnitButtonState.Normal;
                UnitButtonState stateNext = UnitButtonState.Normal;
                if (_firstShownButtonIndex == 0)
                {
                    stateBack = UnitButtonState.Disabled;
                }
                else if (IsMouseDownInScrollBackButton)
                {
                    stateBack = UnitButtonState.Pressed | UnitButtonState.UnderMouseCursor;
                }
                else if (IsMouseOverScrollBackButton)
                {
                    stateBack = UnitButtonState.UnderMouseCursor;
                }
                else if (IsMouseDownInScrollNextButton)
                {
                    stateNext = UnitButtonState.Pressed | UnitButtonState.UnderMouseCursor;
                }
                else if (IsMouseOverScrollNextButton)
                {
                    stateNext = UnitButtonState.UnderMouseCursor;
                }

                if (_canScrollNext == false)
                {
                    stateNext = UnitButtonState.Disabled;
                }

                ButtonsRenderer.DrawScrollBackButton(_scrollBackBounds, stateBack, e.Graphics);
                ButtonsRenderer.DrawScrollNextButton(_scrollNextBounds, stateNext, e.Graphics);
            }

            base.OnPaint(e);
        }

        /// <summary>
        /// 绘制圆边框
        /// </summary>
        protected virtual void DrawRoundBorder(PaintEventArgs e)
        {
            int round = RoundRadius;
            int width = Width - 1;
            int height = Height - 1;

            using (GraphicsPath roundRectPath = GraphicsOrder.CreateRoundRectPath(0, 0, width, height, round))
            {
                using (Brush backBrush = new SolidBrush(ButtonsRenderer.SelectedBackGradient2))
                {
                    e.Graphics.FillPath(backBrush, roundRectPath);
                }

                using (Pen borderPen = new Pen(ButtonsRenderer.SelectedBorder2))
                {
                    e.Graphics.DrawPath(borderPen, roundRectPath);

                    if (CutRoundRect)
                    {
                        e.Graphics.DrawLine(borderPen, 0, height - 1, width, height - 1);

                        using (GraphicsPath roundRect = GraphicsOrder.CreateRoundRectPath(0, 0, width + 1, height, round))
                        {
                            SetControlRegion(roundRect, this);
                        }
                    }
                    else
                    {
                        SetControlRegion(null, this);
                    }
                }
            }
        }

        /// <summary>
        /// 绘制按钮线条
        /// </summary>
        protected virtual void DrawButtonsLine(PaintEventArgs e)
        {
            ButtonsRenderer.DrawButtonsLine(e.Graphics, _clientBounds, _buttonsPanelBounds);
        }

        /// <summary>
        /// 在油漆面板背景上
        /// </summary>
        protected virtual void OnPaintPanelBackground(PaintEventArgs e)
        {
            using (LinearGradientBrush backBrush = new LinearGradientBrush(_clientBounds, BackGradient1, BackGradient2, BackGradientMode))
            {
                e.Graphics.FillRectangle(backBrush, _clientBounds);
            }
        }

        /// <summary>
        /// 在此控件上更改大小时发生
        /// </summary>
        protected override void OnSizeChanged(EventArgs e)
        {
            if (ClientRectangle != _clientBounds)
            {
                _clientBounds = ClientRectangle;

                UpdateSize();
            }

            base.OnSizeChanged(e);
        }

        /// <summary>
        /// 在鼠标向下时发生
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            _isMouseDownInTabButton = false;
            _isMovingTabButton = false;

            if (IsMouseOverScrollBackButton)
            {
                IsMouseDownInScrollBackButton = true;
                ScrollBack();
            }
            else if (IsMouseOverScrollNextButton)
            {
                IsMouseDownInScrollNextButton = true;
                ScrollNext();
            }
            else if (_captionButtonsBounds.Contains(e.Location))
            {
                int captionIndex = ButtonsRenderer.GetCaptionButtonIndex(_captionButtonsBounds, e.Location);
                OnMouseDownInCaptionButton(captionIndex, e);
            }
            else
            {
                int index = 0;
                UnitButton button = GetButtonFromPoint(e.Location, out index);
                if (button != null)
                {
                    SelectedIndex = index;

                    Invalidate();

                    _isMouseDownInTabButton = e.Button == MouseButtons.Left;

                    OnMouseDownInTabButton(button);
                }
            }


            base.OnMouseDown(e);
        }

        /// <summary>
        /// 当鼠标光标移到面板上时发生
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            Cursor cursor = Cursors.Default;

            if (_isMouseDownInTabButton && e.Button == MouseButtons.Left)
            {
                cursor = Cursors.Hand;

                if (_isMovingTabButton)
                {
                    UnitButton selected = _buttons[SelectedIndex];
                    if (selected.Contains(e.Location) == false)
                    {
                        if (_isDraggingTabButton)
                        {
                            ContinueButtonDrag(e.Location, ref cursor);
                        }
                        else
                        {
                            int index = -1;
                            UnitButton underMouse = GetButtonFromPoint(e.Location, true, out index);
                            if (underMouse != null)
                            {
                                bool displace = false;
                                if (underMouse == _buttonDisplaced)
                                {
                                    if (index < SelectedIndex)
                                    {
                                        displace = ButtonsRenderer.CanUndoDisplaceBack(underMouse, selected, e.Location);
                                    }
                                    else if (index > SelectedIndex)
                                    {
                                        displace = ButtonsRenderer.CanUndoDisplaceNext(underMouse, selected, e.Location);
                                    }
                                }

                                _buttonDisplaced = underMouse;

                                if (displace)
                                {
                                    _buttons[index] = selected;
                                    _buttons[SelectedIndex] = underMouse;
                                    _updatePositionsOnDraw = true;
                                    SelectedIndex = index;
                                }
                            }
                            else
                            {
                                _isDraggingTabButton = BeginButtonDrag(selected, e.Location, ref cursor);
                            }
                        }
                    }
                }
                else
                {
                    _isMovingTabButton = _buttons[SelectedIndex].Contains(e.Location);
                    _isDraggingTabButton = false;
                }
            }
            else if (GetButtonFromPoint(e.Location) != null)
            {
                cursor = Cursors.Hand;
            }

            CheckIfIsMouseOverScrollButtons(e.Location);
            if (IsMouseOverScrollBackButton && _firstShownButtonIndex > 0)
            {
                cursor = Cursors.Hand;
            }
            else if (IsMouseOverScrollNextButton && _canScrollNext)
            {
                cursor = Cursors.Hand;
            }

            int captionIndex = ButtonsRenderer.GetCaptionButtonIndex(_captionButtonsBounds, e.Location);
            if (captionIndex < 0 || captionIndex >= ButtonsCount)
            {
                captionIndex = -1;
            }
            else
            {
                cursor = Cursors.Hand;
            }

            if (_captionButtonIndexUnderMouse != captionIndex)
            {
                _captionButtonIndexUnderMouse = captionIndex;
                OnMouseMoveOverCaptionButton(captionIndex, e);

                Invalidate();
            }

            int buttonIndex = -1;
            UnitButton buttonUnderMouse = GetButtonFromPoint(e.Location, false, out buttonIndex);
            if (buttonUnderMouse != null)
            {
                OnMouseMoveOverTabButton(buttonUnderMouse);
            }


            Cursor = cursor;

            base.OnMouseMove(e);
        }

        /// <summary>
        /// 释放鼠标光标时发生
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            EndMouseAction(false);

            base.OnMouseUp(e);
        }

        /// <summary>
        /// 进程对话框键
        /// </summary>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                EndMouseAction(true);
            }

            return base.ProcessDialogKey(keyData);
        }

        /// <summary>
        /// 在此控件内按下键时发生
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                EndMouseAction(true);
            }

            base.OnKeyDown(e);
        }

        /// <summary>
        /// 在更改页面面板大小后发生
        /// </summary>
        protected virtual void OnPagesPanelBoundsChanged(Rectangle bounds)
        {
        }

        /// <summary>
        /// 在设置所选索引时发生
        /// </summary>
        /// <param name="e">event argument</param>
        protected virtual void OnSelectedIndexSet(EventArgs e)
        {
        }

        /// <summary>
        /// 当鼠标移到标题按钮上时发生
        /// </summary>
        protected virtual void OnMouseMoveOverCaptionButton(int captionIndex, MouseEventArgs e)
        {
        }

        /// <summary>
        /// 当鼠标按钮在标题按钮内放下时发生
        /// </summary>
        protected virtual void OnMouseDownInCaptionButton(int captionIndex, MouseEventArgs e)
        {
        }

        /// <summary>
        /// 在将鼠标按钮移到选项卡按钮上时发生
        /// </summary>
        protected virtual void OnMouseMoveOverTabButton(UnitButton buttonUnderMouse)
        {
        }

        /// <summary>
        /// 在光标位于选项卡按钮上并且按下鼠标按钮时发生
        /// </summary>
        protected virtual void OnMouseDownInTabButton(UnitButton buttonUnderMouse)
        {
        }

        /// <summary>
        /// 在删除选项卡按钮时发生
        /// </summary>
        /// <param name="button">button removed</param>
        protected virtual void OnButtonRemoved(UnitButton button)
        {
        }

        /// <summary>
        /// 添加选项卡按钮时发生
        /// </summary>
        /// <param name="button">button added</param>
        protected virtual void OnButtonAdded(UnitButton button)
        {
        }

        /// <summary>
        /// 当选项卡按钮呈现器更改时调用
        /// </summary>
        protected virtual void OnTabButonRendererChanged()
        {
        }

        /// <summary>
        /// 获取“页边距”面板
        /// </summary>
        protected virtual int PagesPanelMargins
        {
            get { return 4; }
        }

        /// <summary>
        /// 获取圆角矩形的半径
        /// </summary>
        protected virtual int RoundRadius
        {
            get { return 3; }
        }

        /// <summary>
        /// 标题按钮计数
        /// </summary>
        protected virtual int CaptionButtonsCount
        {
            get { return 0; }
        }

        /// <summary>
        /// 绘制标题按钮
        /// </summary>
        protected virtual void OnDrawCaptionButtons(Rectangle captionButtonsBounds, Graphics graphics)
        {
        }

        /// <summary>
        /// 更新绘图位置
        /// </summary>
        protected void UpdatePositions()
        {
            _updatePositionsOnDraw = true;
            Invalidate();
        }

        /// <summary>
        /// 获取按钮计数
        /// </summary>
        protected int ButtonsCount
        {
            get { return _buttons.Count; }
        }

        /// <summary>
        /// 添加按钮
        /// </summary>
        protected void AddButton(UnitButton button)
        {
            InsertButton(button, _buttons.Count);
        }

        /// <summary>
        /// 插入按钮
        /// </summary>
        protected void InsertButton(UnitButton button, int insertIndex)
        {
            if (button != null && insertIndex >= 0 && insertIndex <= _buttons.Count)
            {
                _buttons.Insert(insertIndex, button);
                OnButtonAdded(button);

                button.TextChanged += OnButtonTextChanged;
                button.ExplicitDisposing += OnButtonDisposing;

                SelectedIndex = insertIndex;

                if (_buttons.Count == 1)
                {
                    using (Graphics graphics = CreateGraphics())
                    {
                        UpdatePositions(graphics, true);
                    }
                }
                else
                {
                    _updatePositionsOnDraw = true;
                }

                UpdateSize();
                Invalidate();
            }
        }

        /// <summary>
        /// 删除按钮
        /// </summary>
        protected bool RemoveButton(UnitButton button)
        {
            if (_buttons.Contains(button) == false)
            {
                return false;
            }

            // If the selected index is the last button index, must decrement the selected index
            // to prevent index out of range exception after the count of buttons is updated.
            if (SelectedIndex > 0 && SelectedIndex == ButtonsCount - 1)
            {
                SelectedIndex--;
            }

            _buttons.Remove(button);
            OnButtonRemoved(button);

            button.TextChanged -= OnButtonTextChanged;
            button.ExplicitDisposing -= OnButtonDisposing;

            if (_buttons.Count == 0)
            {
                if (Disposing == false && IsDisposed == false)
                {
                    using (Graphics graphics = CreateGraphics())
                    {
                        UpdatePositions(graphics, false);
                    }
                }
            }
            else
            {
                _updatePositionsOnDraw = true;
            }

            if (Disposing == false && IsDisposed == false)
            {
                UpdateSize();
                Invalidate();
            }

            return true;
        }

        /// <summary>
        /// 在给定索引处获取按钮
        /// </summary>
        /// <param name="buttonIndex">zero based button index</param>
        /// <returns>button at given index</returns>
        protected UnitButton GetButtonAt(int buttonIndex)
        {
            if (buttonIndex == -1)
            {
                return null;
            }

            return _buttons[buttonIndex];
        }

        /// <summary>
        /// 所选按钮
        /// </summary>
        protected UnitButton SelectedButton
        {
            get { return GetButtonAt(SelectedIndex); }
            set { SelectedIndex = _buttons.IndexOf(value); }
        }

        /// <summary>
        /// 页边距
        /// </summary>
        protected Rectangle PagesBounds
        {
            get { return _pagesPanelBounds; }
        }

        /// <summary>
        /// 验证当前实例是否未释放
        /// </summary>
        protected void ValidateNotDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        /// <summary>
        /// 设置控件的区域
        /// </summary>
        protected static void SetControlRegion(GraphicsPath newRegionPath, Control control)
        {
            Region oldRegion = control.Region;
            if (newRegionPath != null)
            {
                control.Region = new Region(newRegionPath);
            }
            else
            {
                if (control.Region == null)
                {
                    return;
                }
                control.Region = null;
            }
            if (oldRegion != null)
            {
                oldRegion.Dispose();
            }
        }

        /// <summary>
        /// 切圆矩形
        /// </summary>
        protected bool CutRoundRect
        {
            get { return _cutRoundRect; }
            set
            {
                if (_cutRoundRect != value)
                {
                    _cutRoundRect = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 更新面板尺寸
        /// </summary>
        protected void UpdateSize()
        {
            if (IsDisposed)
            {
                return;
            }

            if (CutRoundRect)
            {
                using (GraphicsPath roundRect = GraphicsOrder.CreateRoundRectPath(0, 0, Width, Height - 1, RoundRadius))
                {
                    SetControlRegion(roundRect, this);
                }
            }
            else
            {
                SetControlRegion(null, this);
            }

            using (Graphics graphics = CreateGraphics())
            {
                UpdatePositions(graphics, false);
            }
        }


        /// <summary>
        /// 开始按钮拖动
        /// </summary>
        /// <returns>true if button drag is started</returns>
        protected virtual bool BeginButtonDrag(UnitButton selected, Point mousePosition, ref Cursor cursor)
        {
            return false;
        }

        /// <summary>
        /// 继续按钮拖动
        /// </summary>
        protected virtual void ContinueButtonDrag(Point mousePosition, ref Cursor cursor)
        {
        }

        /// <summary>
        /// 结束按钮拖动
        /// </summary>
        protected virtual void EndButtonDrag(bool cancel)
        {
        }


        #endregion 可继承函数

        #region 私有函数
        #region 收到事件

        /// <summary>
        /// 按钮释放时收到的事件
        /// </summary>
        private void OnButtonDisposing(object sender, EventArgs e)
        {
            RemoveButton((UnitButton)sender);
        }

        /// <summary>
        /// 按钮文本更改时收到的事件
        /// </summary>
        private void OnButtonTextChanged(object sender, EventArgs e)
        {
            _updatePositionsOnDraw = true;
            Invalidate();
        }

        /// <summary>
        /// 发生滚动移动时间时接收到的事件
        /// </summary>
        private void OnScrollMoveTimeOccurence(object sender, EventArgs e)
        {
            if (_hasScrolls == false)
            {
                return;
            }


            if (IsMouseDownInScrollBackButton && _firstShownButtonIndex > 0)
            {
                _firstShownButtonIndex--;
                Invalidate();
            }
            else if (IsMouseDownInScrollNextButton && _canScrollNext)
            {
                _firstShownButtonIndex++;
                Invalidate();
            }
        }

        #endregion 收到事件

        /// <summary>
        /// 在给定的点获取按钮
        /// </summary>
        private UnitButton GetButtonFromPoint(Point point, bool permisive, out int buttonIndex)
        {
            if (_buttonsPanelBounds.Contains(point))
            {
                if (permisive)
                {
                    int x = _buttonsPanelBounds.Left;
                    if (_buttons.Count > 0)
                    {
                        x = _buttons[0].Left - 4;
                    }

                    Rectangle bounds = new Rectangle();
                    bounds.Y = _buttonsPanelBounds.Y - 4;
                    bounds.Height = _buttonsPanelBounds.Height + 8;
                    for (int index = 0; index < _buttons.Count; index++)
                    {
                        bounds.X = x;
                        bounds.Width = _buttons[index].Width + 4;

                        if (bounds.Contains(point))
                        {
                            buttonIndex = index;
                            return _buttons[index];
                        }

                        x = bounds.Right;
                    }
                }
                else
                {
                    for (int index = 0; index < _buttons.Count; index++)
                    {
                        if (_buttons[index].Contains(point))
                        {
                            buttonIndex = index;
                            return _buttons[index];
                        }
                    }
                }
            }

            buttonIndex = -1;
            return null;
        }

        /// <summary>
        /// 在给定的点获取按钮
        /// </summary>
        private UnitButton GetButtonFromPoint(Point point, out int buttonIndex)
        {
            return GetButtonFromPoint(point, false, out buttonIndex);
        }

        /// <summary>
        /// 在给定的点获取按钮
        /// </summary>
        private UnitButton GetButtonFromPoint(Point point)
        {
            int buttonIndex = -1;
            return GetButtonFromPoint(point, out buttonIndex);
        }

        /// <summary>
        /// 检查鼠标是否在滚动按钮上
        /// </summary>
        private void CheckIfIsMouseOverScrollButtons(Point location)
        {
            if (_scrollBackBounds.Contains(location))
            {
                IsMouseOverScrollBackButton = true;
                IsMouseOverScrollNextButton = false;
            }
            else if (_scrollNextBounds.Contains(location))
            {
                IsMouseOverScrollBackButton = false;
                IsMouseOverScrollNextButton = true;
            }
            else
            {
                IsMouseOverScrollBackButton = false;
                IsMouseOverScrollNextButton = false;
            }
        }

        /// <summary>
        /// 更新面板上的位置
        /// </summary>
        private void UpdatePositions(Graphics graphics, bool forceChangeNotification)
        {
            Rectangle bounds = new Rectangle();
            Rectangle pagesPanelBounds = _clientBounds;

            int buttonsCount = _buttons.Count;
            if (buttonsCount == 1 && ShowOneTabButton == false)
            {
                buttonsCount = 0;
            }

            if (buttonsCount != 0)
            {
                bounds = ButtonsRenderer.GetFirstButtonBounds(_clientBounds, false, 0, _buttons[0].Text, Font, _buttons[0].PageIcon, graphics);
                _buttons[0].SetBounds(bounds);

                for (int index = 1; index < _buttons.Count; index++)
                {
                    bounds = ButtonsRenderer.GetNextButtonBounds(bounds, _buttons[index].Text, Font, _buttons[index].PageIcon, graphics);
                    _buttons[index].SetBounds(bounds);
                }

                _buttonsPanelBounds = ButtonsRenderer.GetButtonsClipRectangle(_clientBounds, false, CaptionButtonsCount);

                _hasScrolls = ButtonsRenderer.HasScroll(_buttonsPanelBounds, bounds);
                if (_hasScrolls)
                {
                    _buttonsPanelBounds = ButtonsRenderer.GetButtonsClipRectangle(_clientBounds, true, CaptionButtonsCount);
                }

                if (_hasScrolls)
                {
                    int scrollPos = ButtonsRenderer.GetScrollPos(_buttons, _firstShownButtonIndex);

                    bounds = ButtonsRenderer.GetFirstButtonBounds(_clientBounds, true, scrollPos, _buttons[0].Text, Font, _buttons[0].PageIcon, graphics);
                    _buttons[0].SetBounds(bounds);

                    for (int index = 1; index < _buttons.Count; index++)
                    {
                        bounds = ButtonsRenderer.GetNextButtonBounds(bounds, _buttons[index].Text, Font, _buttons[index].PageIcon, graphics);
                        _buttons[index].SetBounds(bounds);
                    }

                    _scrollBackBounds = ButtonsRenderer.GetScrollBackButtonBounds(_buttonsPanelBounds);
                    _scrollNextBounds = ButtonsRenderer.GetScrollNextButtonBounds(_buttonsPanelBounds);

                    _canScrollNext = ButtonsRenderer.CanScrollNext(_buttons[ButtonsCount - 1], _scrollNextBounds) && _firstShownButtonIndex < ButtonsCount - 1;
                }

                pagesPanelBounds = ButtonsRenderer.GetClientRectangle(pagesPanelBounds);
            }

            _captionButtonsBounds = ButtonsRenderer.GetCaptionButtonsRectangle(_clientBounds, CaptionButtonsCount);

            if (_pagesPanelBounds != pagesPanelBounds || forceChangeNotification)
            {
                _pagesPanelBounds = pagesPanelBounds;
                pagesPanelBounds.Height++;

                //pagesPanelBounds.Inflate(-PagesPanelMargins, -PagesPanelMargins);

                OnPagesPanelBoundsChanged(pagesPanelBounds);
            }
        }

        /// <summary>
        /// 指示鼠标光标是否在后滚按钮上的标志的访问器
        /// </summary>
        private bool IsMouseOverScrollBackButton
        {
            get { return _isMouseOverScrollBackButton; }
            set
            {
                if (_isMouseOverScrollBackButton != value)
                {
                    _isMouseOverScrollBackButton = value;

                    if (IsMouseOverScrollBackButton == false)
                    {
                        IsMouseDownInScrollBackButton = false;
                    }

                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 指示鼠标光标是否在滚动下一个按钮上的标志的访问器
        /// </summary>
        private bool IsMouseOverScrollNextButton
        {
            get { return _isMouseOverScrollNextButton; }
            set
            {
                if (_isMouseOverScrollNextButton != value)
                {
                    _isMouseOverScrollNextButton = value;

                    if (IsMouseOverScrollNextButton == false)
                    {
                        IsMouseDownInScrollNextButton = false;
                    }

                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 指示光标位于后滚按钮上时鼠标左键是否向下的标志的访问器
        /// </summary>
        private bool IsMouseDownInScrollBackButton
        {
            get { return _isMouseDownInScrollBackButton; }
            set
            {
                if (_isMouseDownInScrollBackButton != value)
                {
                    _isMouseDownInScrollBackButton = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 指示光标位于上时鼠标左键是否向下的标志的访问器滚动下一个按钮
        /// </summary>
        private bool IsMouseDownInScrollNextButton
        {
            get { return _isMouseDownInScrollNextButton; }
            set
            {
                if (_isMouseDownInScrollNextButton != value)
                {
                    _isMouseDownInScrollNextButton = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 滚动到下一个
        /// </summary>
        private void ScrollNext()
        {
            if (_hasScrolls == false)
            {
                return;
            }

            if (IsMouseDownInScrollNextButton && _canScrollNext)
            {
                _firstShownButtonIndex++;
                _updatePositionsOnDraw = true;
                Invalidate();
            }
        }

        /// <summary>
        /// 向后滚动
        /// </summary>
        private void ScrollBack()
        {
            if (_hasScrolls == false)
            {
                return;
            }


            if (IsMouseDownInScrollBackButton && _firstShownButtonIndex > 0)
            {
                _firstShownButtonIndex--;
                _updatePositionsOnDraw = true;
                Invalidate();
            }
        }

        /// <summary>
        /// 结束鼠标操作
        /// </summary>
        private void EndMouseAction(bool cancel)
        {
            _isMouseDownInTabButton = false;

            if (_isMovingTabButton)
            {
                if (_isDraggingTabButton)
                {
                    EndButtonDrag(cancel);
                }

                _isMovingTabButton = false;
                _buttonDisplaced = null;
            }

            IsMouseDownInScrollBackButton = false;
            IsMouseDownInScrollNextButton = false;
        }

        #endregion 私有函数
        private void ButtonContainer_Load(object sender, EventArgs e)
        {

        }
    }
}
