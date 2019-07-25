using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HuaTu.Controls.Internal
{
    public sealed class GraphicsOrder
    {
        #region 构造器

       
        private GraphicsOrder()
        {
        }

        #endregion 构造器

        #region 公开函数

        
        public static GraphicsPath CreateRoundRectPath(int x, int y, int width, int height, int round)
        {
            return CreateRoundRectPath(x, y, width, height, round, round, round, round);
        }

       
        public static GraphicsPath CreateRoundRectPath(int x, int y, int width, int height,
           int roundLeftTop, int roundRightTop, int roundLeftBottom, int roundRightBottom)
        {
            GraphicsPath roundRectPath = new GraphicsPath();

            Point[] lines = new Point[]
            {
            new Point(x,                            y + roundLeftTop),
            new Point(x + roundLeftTop,             y),
            new Point(x + width - roundRightTop,    y),
            new Point(x + width,                    y + roundRightTop),
            new Point(x + width,                    y + height - roundRightBottom),
            new Point(x + width - roundRightBottom, y + height),
            new Point(x + roundLeftBottom,          y + height),
            new Point(x,                            y + height - roundLeftBottom)
            };

            roundRectPath.AddLines(lines);
            roundRectPath.CloseFigure();

            return roundRectPath;
        }

       
        public static void DrawRoundRect(Rectangle bounds, int roundX, int roundY, Pen pen, Graphics graphics)
        {
            IntPtr hdc = graphics.GetHdc();
            try
            {
                int argb = ColorToRGB(pen.Color);

                IntPtr hPen = CreatePen(GetPenStyle(pen.DashStyle), (int)pen.Width, argb);
                LogBrush brushData = new LogBrush();
                brushData.lbColor = ColorToRGB(Color.Transparent);
                brushData.lbStyle = W32BrushStyle.Null;
                IntPtr hBrush = CreateBrushIndirect(ref brushData);

                IntPtr oldPen = SelectObject(hdc, hPen);
                IntPtr oldBrush = SelectObject(hdc, hBrush);

                RoundRect(hdc, bounds.X, bounds.Y, bounds.Right, bounds.Bottom, roundX, roundY);

                DeleteObject(SelectObject(hdc, oldPen));
                DeleteObject(SelectObject(hdc, oldBrush));
            }
            finally
            {
                graphics.ReleaseHdc(hdc);
            }
        }

       
        public static void FillRoundRect(Rectangle bounds, int roundX, int roundY, Color backColor, Graphics graphics)
        {
            IntPtr hdc = graphics.GetHdc();
            try
            {
                int argb = ColorToRGB(backColor);

                IntPtr hPen = CreatePen(W32PenStyle.Null, 1, 0);
                LogBrush brushData = new LogBrush();
                brushData.lbColor = argb;
                brushData.lbStyle = W32BrushStyle.Solid;
                IntPtr hBrush = CreateBrushIndirect(ref brushData);

                IntPtr oldPen = SelectObject(hdc, hPen);
                IntPtr oldBrush = SelectObject(hdc, hBrush);

                RoundRect(hdc, bounds.X, bounds.Y, bounds.Right, bounds.Bottom, roundX, roundY);

                DeleteObject(SelectObject(hdc, oldPen));
                DeleteObject(SelectObject(hdc, oldBrush));
            }
            finally
            {
                graphics.ReleaseHdc(hdc);
            }
        }

        #endregion 公开函数

        #region 私有函数

        private static int ColorToRGB(Color color)
        {
            return (color.B << 16) + (color.G << 8) + color.R;
        }

        [DllImport("Gdi32")]
        private static extern bool RoundRect(
             IntPtr hdc,         // 句柄 DC
             int nLeftRect,   // 矩形左上角的X坐标
             int nTopRect,    // 矩形左上角的Y坐标
             int nRightRect,  // 矩形右下角的X坐标
             int nBottomRect, // 矩形右下角的Y坐标
             int nWidth,      // 椭圆宽度
             int nHeight      // 椭圆高度
           );

        [DllImport("Gdi32")]
        private static extern IntPtr SelectObject(
             IntPtr hdc,          // 句柄 DC
             IntPtr hgdiobj   // 句柄 对象 
           );


        [DllImport("Gdi32")]
        private static extern bool DeleteObject(
             IntPtr hgdiobj   // 句柄 对象
           );


        private enum W32PenStyle : int
        {
            Solid = 0,
            Dash = 1,
            Dot = 2,
            DashDot = 3,
            DashDotDot = 4,
            Null = 5,
            InsideFrame = 6,
            UserStyle = 7,
            Alternate = 8,
            StyleMask = 0x0000000F,
            EndCapSquare = 0x00000100,
            EndCapFlat = 0x00000200,
            EndCapMask = 0x00000F00,
            JoinBevel = 0x00001000,
            JoinMiter = 0x00002000,
            JoinMask = 0x0000F000,
            Geometric = 0x00010000,
            TypeMask = 0x000F0000,
        };


        [DllImport("Gdi32")]
        private static extern IntPtr CreatePen(
             W32PenStyle fnPenStyle,    // pen style
             int nWidth,        // pen width
             int crColor   // pen color
           );

        private static W32PenStyle GetPenStyle(DashStyle dashStyle)
        {
            switch (dashStyle)
            {
                case DashStyle.Custom:
                    return W32PenStyle.UserStyle;

                case DashStyle.Dash:
                    return W32PenStyle.Dash;

                case DashStyle.DashDot:
                    return W32PenStyle.DashDot;

                case DashStyle.DashDotDot:
                    return W32PenStyle.DashDotDot;

                case DashStyle.Dot:
                    return W32PenStyle.Dot;

                case DashStyle.Solid:
                    return W32PenStyle.Solid;
            }

            return W32PenStyle.Solid;
        }


        private enum W32BrushStyle : int
        {
            Solid = 0,
            Null = 1,
            Hatched = 2,
            Pattern = 3,
            Indexed = 4,
            DIBPattern = 5,
            DIBPatternPt = 6,
            Pattern8X8 = 7,
            DIBPattern8X8 = 8,
            MonOPattern = 9,
        };

        [StructLayout(LayoutKind.Sequential)]
        struct LogBrush
        {
            public W32BrushStyle lbStyle;
            public int lbColor;
            public int lbHatch;
        };

        [DllImport("Gdi32")]
        private static extern IntPtr CreateBrushIndirect(
           ref LogBrush lplb   // 笔刷 信息
           );

        #endregion 私有函数
    }
}
