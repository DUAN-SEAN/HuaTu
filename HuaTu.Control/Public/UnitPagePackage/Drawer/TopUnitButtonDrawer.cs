using HuaTu.Controls.Public.UnitPagePackage.Enums;
using HuaTu.Controls.Public.UnitPagePackage.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuaTu.Controls.Public.UnitPagePackage.Drawer
{
    public class TopUnitButtonDrawer : UnitButtonDrawer
    {
        #region 字段

        private const int ButtonTopMargin = 2;
        private const int PanelMargin = 0;
        private const int ButtonHorizontalMargins = 16;
        private const int ButtonHeight = 16;

        #endregion 字段

        #region 构造器

      
        public TopUnitButtonDrawer()
        {
        }

        #endregion 构造器

        #region 公开函数

        /// <summary>
        /// 画按钮
        /// </summary>
        public override void Draw(Rectangle bounds, string text, bool selected, Font font, Icon icon, Graphics graphics)
        {
            using (GraphicsPath button = new GraphicsPath())
            {
                int x1 = bounds.X - (int)(bounds.Height * 0.75);
                int y1 = bounds.Bottom;

                int x2 = bounds.X + (int)(bounds.Height * 0.25) - 2;
                int y2 = bounds.Y + 2;

                int x3 = bounds.X + (int)(bounds.Height * 0.25) + 1;
                int y3 = bounds.Y;

                int x4 = bounds.Right - 2;
                int y4 = bounds.Y;

                int x5 = bounds.Right;
                int y5 = bounds.Y + 2;

                int x6 = bounds.Right;
                int y6 = bounds.Bottom;

                button.AddLine(x1, y1 + 1, x2, y2);
                button.AddLine(x2, y2, x3, y3);
                button.AddLine(x3, y3, x4, y4);
                button.AddLine(x4, y4, x5, y5);
                button.AddLine(x5, y5, x6, y6 + 1);
                button.CloseFigure();

                bounds.Height++;
                using (LinearGradientBrush backBrush = new LinearGradientBrush(bounds, GetBackGradient1Color(selected), GetBackGradient2Color(selected), BackGradientMode))
                {
                    graphics.FillPath(backBrush, button);

                    using (Pen pen = new Pen(GetBorder2Color(selected)))
                    {
                        graphics.DrawLine(pen, x1, y1, x2, y2);
                        graphics.DrawLine(pen, x2, y2, x3, y3);
                        graphics.DrawLine(pen, x3, y3, x4, y4);
                        graphics.DrawLine(pen, x4, y4, x5, y5);
                        graphics.DrawLine(pen, x5, y5, x6, y6 - 1);
                    }

                    using (Pen pen = new Pen(GetBorder1Color(selected)))
                    {
                        graphics.DrawLine(pen, x1 + 1, y1, x2 + 1, y2);
                    }
                }

                if (selected == false)
                {
                    using (Pen pen = new Pen(GetBorder2Color(true)))
                    {
                        graphics.DrawLine(pen, x1, y1 - 1, x6, y6 - 1);
                    }
                }
            }

            Rectangle textBounds = bounds;
            textBounds.X += 10;
            textBounds.Y += 2;

            if (icon != null)
            {
                graphics.DrawIcon(icon, new Rectangle(textBounds.X + 2, bounds.Y + 2, ButtonHeight - 4, ButtonHeight - 4));
                textBounds.X += ButtonHeight;
            }

            using (Brush textBrush = new SolidBrush(TextColor))
            {
                graphics.DrawString(text, font, textBrush, textBounds);
            }
        }

       
        public override void DrawButtonsLine(Graphics graphics, Rectangle panelBounds, Rectangle buttonsPanelBounds)
        {
            using (Pen pen = new Pen(GetBorder2Color(true)))
            {
                graphics.DrawLine(pen, panelBounds.X, buttonsPanelBounds.Bottom - 2, panelBounds.Width, buttonsPanelBounds.Bottom - 2);
            }
        }

       
        public override void DrawScrollBackButton(Rectangle bounds, UnitButtonState state, Graphics graphics)
        {
            int x1 = bounds.Left + 5;
            int y1 = bounds.Top + bounds.Height / 2 + 1;

            int x2 = x1 + 4;
            int y2 = y1 - 4;

            int x3 = x2;
            int y3 = y1 + 4;

            using (GraphicsPath arrowHeadPath = new GraphicsPath())
            {
                arrowHeadPath.AddLine(x1, y1, x2, y2);
                arrowHeadPath.AddLine(x2, y2, x3, y3);
                arrowHeadPath.CloseFigure();

                switch (state)
                {
                    case UnitButtonState.Normal:
                        graphics.FillPath(Brushes.LightSteelBlue, arrowHeadPath);

                        graphics.DrawLine(Pens.DarkBlue, x1, y1 - 1, x2, y2 - 1);
                        graphics.DrawLine(Pens.DarkBlue, x2, y2 - 1, x3, y3 + 1);
                        graphics.DrawLine(Pens.White, x3, y3 + 1, x1, y1 + 1);

                        break;

                    case UnitButtonState.Focus:
                        graphics.FillPath(Brushes.LightGreen, arrowHeadPath);

                        graphics.DrawLine(Pens.Blue, x1, y1 - 1, x2, y2 - 1);
                        graphics.DrawLine(Pens.Blue, x2, y2 - 1, x3, y3 + 1);
                        graphics.DrawLine(Pens.White, x3, y3 + 1, x1, y1 + 1);

                        break;

                    case UnitButtonState.Pressed:
                        graphics.FillPath(Brushes.LightSteelBlue, arrowHeadPath);

                        graphics.DrawLine(Pens.DarkBlue, x3, y3 + 1, x1, y1 + 1);
                        graphics.DrawLine(Pens.DarkBlue, x2, y2 - 1, x3, y3 + 1);
                        graphics.DrawLine(Pens.WhiteSmoke, x1, y1 - 1, x2, y2 - 1);

                        break;

                    case UnitButtonState.UnderMouseCursor:
                        graphics.FillPath(Brushes.LightSteelBlue, arrowHeadPath);

                        graphics.DrawLine(Pens.Blue, x1, y1 - 1, x2, y2 - 1);
                        graphics.DrawLine(Pens.Blue, x2, y2 - 1, x3, y3 + 1);
                        graphics.DrawLine(Pens.White, x3, y3 + 1, x1, y1 + 1);

                        break;

                    case UnitButtonState.Disabled:
                        graphics.FillPath(Brushes.Gray, arrowHeadPath);

                        graphics.DrawLine(Pens.DarkGray, x1, y1 - 1, x2, y2 - 1);
                        graphics.DrawLine(Pens.DarkGray, x2, y2 - 1, x3, y3 + 1);
                        graphics.DrawLine(Pens.White, x3, y3 + 1, x1, y1 + 1);

                        break;
                }
            }
        }


        public override void DrawScrollNextButton(Rectangle bounds, UnitButtonState state, Graphics graphics)
        {
            int x1 = bounds.Left + 5;
            int y1 = bounds.Top + bounds.Height / 2 + 2;

            int x2 = x1 - 5;
            int y2 = y1 - 5;

            int x3 = x2;
            int y3 = y1 + 4;

            using (GraphicsPath arrowHeadPath = new GraphicsPath())
            {
                arrowHeadPath.AddLine(x1, y1 - 1, x2, y2 - 1);
                arrowHeadPath.AddLine(x2, y2 - 1, x3, y3 + 1);
                arrowHeadPath.CloseFigure();

                switch (state)
                {
                    case UnitButtonState.Normal:
                        graphics.FillPath(Brushes.LightSteelBlue, arrowHeadPath);

                        graphics.DrawLine(Pens.DarkBlue, x1, y1 - 1, x2, y2 - 1);
                        graphics.DrawLine(Pens.DarkBlue, x2, y2 - 1, x3, y3 + 1);
                        graphics.DrawLine(Pens.White, x3, y3 + 1, x1, y1 + 1);

                        break;

                    case UnitButtonState.Focus:
                        graphics.FillPath(Brushes.LightGreen, arrowHeadPath);

                        graphics.DrawLine(Pens.Blue, x1, y1 - 1, x2, y2 - 1);
                        graphics.DrawLine(Pens.Blue, x2, y2 - 1, x3, y3 + 1);
                        graphics.DrawLine(Pens.White, x3, y3 + 1, x1, y1 + 1);

                        break;

                    case UnitButtonState.Pressed:
                        graphics.FillPath(Brushes.LightSteelBlue, arrowHeadPath);

                        graphics.DrawLine(Pens.DarkBlue, x3, y3 + 1, x1, y1 + 1);
                        graphics.DrawLine(Pens.DarkBlue, x2, y2 - 1, x3, y3 + 1);
                        graphics.DrawLine(Pens.WhiteSmoke, x1, y1 - 1, x2, y2 - 1);

                        break;

                    case UnitButtonState.UnderMouseCursor:
                        graphics.FillPath(Brushes.LightSteelBlue, arrowHeadPath);

                        graphics.DrawLine(Pens.Blue, x1, y1 - 1, x2, y2 - 1);
                        graphics.DrawLine(Pens.Blue, x2, y2 - 1, x3, y3 + 1);
                        graphics.DrawLine(Pens.White, x3, y3 + 1, x1, y1 + 1);

                        break;

                    case UnitButtonState.Disabled:
                        graphics.FillPath(Brushes.Gray, arrowHeadPath);

                        graphics.DrawLine(Pens.DarkGray, x1, y1 - 1, x2, y2 - 1);
                        graphics.DrawLine(Pens.DarkGray, x2, y2 - 1, x3, y3 + 1);
                        graphics.DrawLine(Pens.White, x3, y3 + 1, x1, y1 + 1);

                        break;
                }
            }
        }


        public override bool HasScroll(Rectangle panelBounds, Rectangle lastButtonBounds)
        {
            return lastButtonBounds.Right >= panelBounds.Right;
        }

        public override Rectangle GetScrollBackButtonBounds(Rectangle panelBounds)
        {
            return new Rectangle(panelBounds.Left - ButtonHeight, panelBounds.Top, ButtonHeight - ButtonTopMargin, ButtonHeight - ButtonTopMargin);
        }


        public override Rectangle GetScrollNextButtonBounds(Rectangle panelBounds)
        {
            return new Rectangle(panelBounds.Right + 2 * 4, panelBounds.Top, ButtonHeight - ButtonTopMargin, ButtonHeight - ButtonTopMargin);
        }


        public override Rectangle GetFirstButtonBounds(Rectangle panelBounds, bool hasScroll, int scrolPos, string text, Font font, Icon icon, Graphics graphics)
        {
            int dX = 5 + PanelMargin + (int)(ButtonHeight * 0.75);

            if (hasScroll)
            {
                dX += ButtonHeight - ButtonTopMargin;
            }

            return GetButtonBounds(-scrolPos + dX, panelBounds.Top + ButtonTopMargin, text, font, icon, graphics);
        }


        public override Rectangle GetNextButtonBounds(Rectangle previousBounds, string text, Font font, Icon icon, Graphics graphics)
        {
            return GetButtonBounds(previousBounds.Right + 2, previousBounds.Top, text, font, icon, graphics);
        }

 
        public override Rectangle GetButtonsClipRectangle(Rectangle panelBounds, bool hasScroll, int captionButtonsCount)
        {
            int dX = 5 + PanelMargin;

            if (hasScroll)
            {
                dX += ButtonHeight - ButtonTopMargin;
            }

            int dw = ButtonHeight * captionButtonsCount;

            return new Rectangle(dX, panelBounds.Top + ButtonTopMargin, panelBounds.Right - 2 * dX - PanelMargin - dw, ButtonHeight + 1);
        }


        public override Rectangle GetClientRectangle(Rectangle panelBounds)
        {
            Rectangle clientRectangle = new Rectangle();

            clientRectangle.X = panelBounds.X;
            clientRectangle.Y = panelBounds.Top + ButtonTopMargin + ButtonHeight;
            clientRectangle.Width = panelBounds.Width;
            clientRectangle.Height = panelBounds.Height - clientRectangle.Y - 2;

            return clientRectangle;
        }


        public override Rectangle GetCaptionButtonsRectangle(Rectangle panelBounds, int captionButtonsCount)
        {
            int width = ButtonHeight * captionButtonsCount;
            int height = ButtonHeight - ButtonTopMargin;

            return new Rectangle(panelBounds.Right - width - PanelMargin, panelBounds.Top + ButtonTopMargin, width, height);
        }


        public override int GetCaptionButtonIndex(Rectangle captionButtonsBounds, Point point)
        {
            if (captionButtonsBounds.Contains(point) == false)
            {
                return -1;
            }

            return (point.X - captionButtonsBounds.Left) / ButtonHeight;
        }


        internal override bool CanUndoDisplaceNext(UnitButton displacedButton, UnitButton selectedButton, Point mouseLocation)
        {
            return true;
        }


        internal override bool CanUndoDisplaceBack(UnitButton displacedButton, UnitButton selectedButton, Point mouseLocation)
        {
            return mouseLocation.X < displacedButton.Left + selectedButton.Width;
        }

        internal override bool CanScrollNext(UnitButton lastButton, Rectangle scrollNextBounds)
        {
            return lastButton.Left + lastButton.Width > scrollNextBounds.Left;
        }

   
        internal override int GetScrollPos(IList<UnitButton> buttons, int firstShownButtonIndex)
        {
            int scrollPos = 0;
            for (int buttonIndex = 0; buttonIndex < firstShownButtonIndex; buttonIndex++)
            {
                scrollPos += buttons[buttonIndex].Width;
            }

            return scrollPos;
        }

        #endregion 公开函数

        #region 私有函数

        /// <summary>
        /// 获得按钮边界
        /// </summary>
        /// <param name="xPos">按钮x坐标</param>
        /// <param name="yPos">按钮y坐标</param>
        /// <param name="text">按钮文本</param>
        /// <param name="font">按钮字体</param>
        /// <param name="icon">按钮图标</param>
        /// <param name="graphics">绘制对象</param>
        private Rectangle GetButtonBounds(int xPos, int yPos, string text, Font font, Icon icon, Graphics graphics)
        {
            int width = ButtonHorizontalMargins + (int)graphics.MeasureString(text, font).Width;
            if (width > 80)
                width = 80;

            Rectangle bounds = new Rectangle();

            bounds.X = xPos;
            bounds.Y = yPos;
            bounds.Width = width;
            bounds.Height = ButtonHeight;

            if (icon != null)
            {
                bounds.Width += ButtonHeight;
            }

            return bounds;
        }

        #endregion 私有函数
    }
}
