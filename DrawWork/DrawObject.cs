using DrawWork.Properties;
using SVGHelper;
using SVGHelper.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using DrawWork.Animation;

//                        _ooOoo_
//                       o8888888o
//                       88" . "88
//                       (| -_- |)
//                        O\ = /O
//                    ____/`---'\____
//                  .   ' \\| |// `.
//                   / \\||| : |||// \
//                 / _||||| -:- |||||- \
//                   | | \\\ - /// | |
//                 | \_| ''\---/'' | |
//                  \ .-\__ `-` ___/-. /
//               ___`. .' /--.--\ `. . __
//            ."" '< `.___\_<|>_/___.' >'"".
//           | | : `- \`.;`\ _ /`;.`/ - ` : | |
//             \ \ `-. \_ __\ /__ _/ .-` / /
//     ======`-.____`-.___\_____/___.-`____.-'======
//                        `=---='
//
//     .............................................
//            佛祖保佑             永无BUG


namespace DrawWork
{
    /// <summary>
    /// 设备类型
    /// </summary>
    public enum DeviceType
    {
        SingleDisConnector,
        Port
    }


    public interface IUpdateSystem
    {
        void Update();
    }
    /// <summary>
	/// 可绘画物体基类
	/// </summary>
	public abstract class DrawObject : ICloneable, IUpdateSystem
    {
        public static PointF Dpi;
        public static int ObjectId;

        public DeviceType _type;//设备类型

        #region 字段
        public RectangleF _imageR = new RectangleF(9, 9, 20, 20);

        public List<Animation.Animation> AnimationBases = new List<Animation.Animation>();


        /// <summary>
        /// 与父级元素的长宽比例
        /// </summary>
        protected PointF proportion;
        #endregion 字段

        #region 属性

        public PointF Proportion
        {
            set => proportion = value;
            get => proportion;
        }


        [Browsable(false)]
        public bool HitOnCircumferance { get; set; }

        [Browsable(false)]
        public int Id { get; set; }

        /// <summary>
        /// 被选中标识
        /// </summary>
        [Browsable(false)]
        public bool Selected { get; set; }

        /// <summary>
        /// 填充颜色
        /// </summary>
        public Color Fill { get; set; }

        /// <summary>
        /// 笔画颜色
        /// </summary>
        public Color Stroke { get; set; }

        /// <summary>
        /// 笔宽
        /// </summary>
        [Browsable(false)]
        protected float StrokeWidth { get; set; }

        public int Thick
        {
            get
            {
                return (int)(StrokeWidth / Zoom);
            }
            set
            {
                StrokeWidth = (int)(value * Zoom);
            }
        }

        public static float Zoom = 1;

        /// <summary>
        /// 句柄数量
        /// </summary>
        [Browsable(false)]
        public virtual int HandleCount
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 上一个颜色
        /// </summary>
        public static Color LastUsedColor { get; set; }

        /// <summary>
        /// 上一个笔宽
        /// </summary>
        public static float LastUsedPenWidth { get; set; }

        public string Name { get; set; }

        #endregion

        #region 虚方法

        /// <summary>
        /// 画一个对象
        /// </summary>
        /// <param name="g"></param>
        public virtual void Draw(Graphics g)
        {
        }

        protected DrawObject()
        {
            Name = "";
            Fill = Color.Empty;
            Id = 0;
            proportion = new PointF(1,1);
            SetId();
        }

        protected DrawObject(PointF pro)
        {
            Name = "";
            Fill = Color.Empty;
            Id = 0;
            SetId();
            proportion = pro;
        }

        static DrawObject()
        {
            LastUsedPenWidth = 1;
            LastUsedColor = Color.Black;
        }

        private void SetId()
        {
            Id = ObjectId++;
        }

        /// <summary>
        /// 以1为基数获取句柄点
        /// </summary>
        public virtual PointF GetHandle(int handleNumber)
        {
            return new PointF(0, 0);
        }

        public virtual PointF GetKnobPoint()
        {
            return new PointF(-20,-20 );
        }
        /// <summary>
        /// 以1为基数获取句柄矩形
        /// </summary>
        public virtual RectangleF GetHandleRectangle(int handleNumber)
        {
            var point = GetHandle(handleNumber);

            return new RectangleF(point.X - 3, point.Y - 3, 7, 7);
        }


        public virtual RectangleF GetKnobRectangle()
        {
            var point = GetKnobPoint();
            return new RectangleF(point.X - 10, point.Y - 10, 20, 20);
        }

        /// <summary>
        /// 为选定对象绘制跟踪器
        /// </summary>
        public virtual void DrawTracker(Graphics g)
        {
            if (!Selected)
                return;

            var brush = new SolidBrush(Color.Black);

            for (int i = 1; i <= HandleCount; i++)
            {
                try
                {
                    g.FillRectangle(brush, GetHandleRectangle(i));
                }
                catch
                { }
            }

            brush.Dispose();
        }
        
        /// <summary>
        /// 为选定对象绘制旋转按钮
        /// </summary>
        /// <param name="g"></param>
        public virtual void DrawRotaryKnob(Graphics g)
        {
            //该函数在重绘控件时被调用
            //this.Paint 不清楚查询该事件
            if (!Selected) return;
            try
            {
                //获得当前角度
                float angel = GetAngle();
                
                Image image = Resources.knob;
                if (image != null)
                    g.DrawImage(image, GetKnobRectangle());
                else
                {
                    Brush brush = new SolidBrush(Color.Black);
                    g.FillEllipse(brush, GetKnobRectangle());
                }
            }
            catch (Exception ex)
            {
                SVGErr.Log("DrawArea", "DrawImageObject", ex.ToString(), SVGErr._LogPriority.Info);
            }

        }
        /// <summary>
        /// 命中测试
        /// Return value: -1 - 没命中
        ///                0 - 命中任何地方
        ///                > 1 - 句柄数量
        /// </summary>
        public virtual int HitTest(PointF point)
        {
            return -1;
        }

        /// <summary>
        /// 判断该点里哪个句柄近返回句柄
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual int HitAroundTest(PointF point)
        {
            if (HitTest(point) >= 0)
            {
                return 0;
            }
            return -1;
        }
        /// <summary>
        /// 命中旋钮测试
        /// 命中返回true
        /// 没命中返回false
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual bool HitKnobTest(PointF point)
        {
            if (Selected)
            {
                return GetKnobRectangle().Contains(point);
            }

            return false;
        }

        /// <summary>
        /// 测试点是否在对象内部
        /// </summary>
        protected virtual bool PointInObject(PointF point)
        {
            return false;
        }

        /// <summary>
        /// 获取句柄的光标
        /// </summary>
        public virtual Cursor GetHandleCursor(int handleNumber)
        {
            return Cursors.Default;
        }

        /// <summary>
		/// 为边界句柄获取curesor
		/// </summary>
        public virtual Cursor GetOutlineCursor(int handleNumber)
        {
            return Cursors.Cross;
        }

        /// <summary>
        /// 获取点击旋钮时光标
        /// </summary>
        /// <returns></returns>
        public virtual Cursor GetKnodCursor()
        {
            return Cursors.AppStarting;
        }

        /// <summary>
        /// 返回当前物体的角度
        /// </summary>
        /// <returns></returns>
        public virtual float GetAngle()
        {
            return 0;
        }

        /// <summary>
        /// 测试对象是否与矩形相交
        /// </summary>
        public virtual bool IntersectsWith(RectangleF rectangle)
        {
            return false;
        }

        /// <summary>
        /// 移动对象
        /// </summary>
        public virtual void Move(float deltaX, float deltaY)
        {
        }

        /// <summary>
        /// 将句柄移到该点
        /// </summary>
        public virtual void MoveHandleTo(PointF point, int handleNumber)
        {
        }

        /// <summary>
        /// 将旋钮移动到该点
        /// </summary>
        /// <param name="point"></param>
        public virtual void RotateKnobTo(PointF point)
        {
            _imageR.X = point.X - _imageR.Width / 2;
            _imageR.Y = point.Y - _imageR.Height / 2;
            //TODO 旋转照片
        }

        /// <summary>
        /// 鼠标单击句柄
        /// </summary>
        public virtual void MouseClickOnHandle(int handle)
        {
        }

        /// <summary>
        /// Dump (for debugging)
        /// </summary>
        public virtual void Dump()
        {
            Trace.WriteLine("");
            Trace.WriteLine(GetType().Name);
            Trace.WriteLine("Selected = " + Selected.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// 规范化对象。
        /// 在对象调整大小结束时调用此函数。
        /// </summary>
        public virtual void Normalize()
        {
        }

        /// <summary>
        /// 鼠标点击边界
        /// </summary>
        public virtual void MouseClickOnBorder()
        {

        }

        /// <summary>
        /// 将对象保存到序列化流
        /// </summary>
        public virtual void SaveToXml(XmlTextWriter writer, double scale)
        {
        }

        /// <summary>
        /// 从序列化流加载对象
        /// </summary>
        public virtual void LoadFromXml(XmlTextReader reader)
        {
        }

        public string GetAnimationXML()
        {
            string s = "";
            if (AnimationBases == null || AnimationBases.Count == 0)
                return s;
            foreach (var animation in AnimationBases)
            {
                s += "    ";
                //TODO:暂时先不处理其他类型的动画 
                //switch (animation._animationType)
                //{
                //    case AnimationType.None:
                //        break;
                //    case AnimationType.Animation:
                //        break;
                //    case AnimationType.AnimationColor:
                //        break;
                //    default:
                //        throw new ArgumentOutOfRangeException();
                //}
                s += "<animate ";
                s += animation.GetXmlStr();
                s += " />";
                s += "\r\n";
            }


            s += GetXmlEnd();
            return s;
        }

        /// <summary>
        /// 获取xml的结尾  例如</line>
        /// </summary>
        /// <returns></returns>
        public  virtual  string GetXmlEnd()
        {
            return "\r\n";
        }
        #region 段瑞 旋转
        /// <summary>
        /// 一个点顺时针绕着另一个点旋转angle度
        /// </summary>
        /// <param name="center">绕点坐标</param>
        /// <param name="p1">要旋转的点</param>
        /// <param name="angle">要旋转的角度，笛卡尔直角坐标系</param>
        /// <returns></returns>
        protected virtual PointF RotatePoint(PointF center, PointF p1, float angle)
        {
            float angleHude = angle * (float)Math.PI / 180;
            float x1 = (p1.X - center.X) * (float)Math.Cos(angleHude) +
                       (p1.Y - center.Y) * (float)Math.Sin(angleHude) + center.X;
            float y1 = -(p1.X - center.X) * (float)Math.Sin(angleHude) +
                        (p1.Y - center.Y) * (float)Math.Cos(angleHude) + center.Y;
            return new PointF(x1, y1);
        }
        /// <summary>
        /// 一个点逆时针绕着另一个点旋转angle度
        /// </summary>
        /// <param name="center"></param>
        /// <param name="p1"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        protected virtual PointF RotatePointReverse(PointF center, PointF p1, float angle)
        {
            angle =(-angle % 360 + 360) % 360;
            
            return RotatePoint(center, p1, angle);
        }
        /// <summary>
        /// 将图形绕中心点旋转
        /// </summary>
        /// <param name="angle"></param>
        public virtual void Rotate(float angle)
        {

        }
        /// <summary>
        /// 将图形绕指定中心点旋转
        /// </summary>
        /// <param name="angle">角度</param>
        /// <param name="center">中心点</param>
        public virtual void Rotate(float angle, PointF center)
        {

        }
        /// <summary>
        /// 将图形绕指定中心点旋转
        /// </summary>
        /// <param name="angle">角度</param>
        /// <param name="x">中心点x坐标</param>
        /// <param name="y">中心点y坐标</param>
        public virtual void Rotate(float angle, float x, float y)
        {

        }


        /// <summary>
        ///  点到线段最短距离的那条直线与线段的交点，{x=...,y=...}
        /// </summary>
        /// <param name="x">线段外的点的x坐标</param>
        /// <param name="y">线段外的点的y坐标</param>
        /// <param name="x1">线段顶点1的x坐标</param>
        /// <param name="y1">线段顶点1的y坐标</param>
        /// <param name="x2">线段顶点2的x坐标</param>
        /// <param name="y2">线段顶点2的y坐标</param>
        /// <returns></returns>
        public PointF PointForPointToABLine(float x, float y, float x1, float y1, float x2, float y2)
        {
            PointF reVal = new PointF();
            // 直线方程的两点式转换成一般式
            // A = Y2 - Y1
            // B = X1 - X2
            // C = X2*Y1 - X1*Y2
            float a1 = y2 - y1;
            float b1 = x1 - x2;
            float c1 = x2 * y1 - x1 * y2;
            float x3, y3;
            if (a1 == 0)
            {
                // 线段与x轴平行
                reVal = new PointF(x, y1);
                x3 = x;
                y3 = y1;
            }
            else if (b1 == 0)
            {
                // 线段与y轴平行
                reVal = new PointF(x1, y);
                x3 = x1;
                y3 = y;
            }
            else
            {
                // 普通线段
                float k1 = -a1 / b1;
                float k2 = -1 / k1;
                float a2 = k2;
                float b2 = -1;
                float c2 = y - k2 * x;
                // 直线一般式和二元一次方程的一般式转换
                // 直线的一般式为 Ax+By+C=0
                // 二元一次方程的一般式为 Ax+By=C
                c1 = -c1;
                c2 = -c2;

                // 二元一次方程求解(Ax+By=C)
                // a=a1,b=b1,c=c1,d=a2,e=b2,f=c2;
                // X=(ce-bf)/(ae-bd)
                // Y=(af-cd)/(ae-bd)
                x3 = (c1 * b2 - b1 * c2) / (a1 * b2 - b1 * a2);
                y3 = (a1 * c2 - c1 * a2) / (a1 * b2 - b1 * a2);
            }

            //// 点(x3,y3)作为点(x,y)到(x1,y1)和(x2,y2)组成的直线距离最近的点,那(x3,y3)是否在(x1,y1)和(x2,y2)的线段之内(包含(x1,y1)和(x2,y2))
            //if (((x3 > x1) != (x3 > x2) || x3 == x1 || x3 == x2) && ((y3 > y1) != (y3 > y2) || y3 == y1 || y3 == y2))
            //{
            //    // (x3,y3)在线段上
            //    reVal = new PointF(x3, y3);
            //}
            //else
            //{
            //    // (x3,y3)在线段外
            //    float d1_quadratic = (x - x1) * (x - x1) + (y - y1) * (y - y1);
            //    float d2_quadratic = (x - x2) * (x - x2) + (y - y2) * (y - y2);
            //    if (d1_quadratic <= d2_quadratic)
            //    {
            //        reVal = new PointF(x1, y1);
            //    }
            //    else
            //    {
            //        reVal = new PointF(x2, y2);
            //    }
            //}
            reVal = new PointF(x3, y3);

            return reVal;
        }

        #endregion
        #endregion

        protected float _angle //换算角度到一个周期
        {
            set
            {
                angle = value;


            }
            get
            {
                angle = (angle % 360 + 360) % 360;
                return angle;
            }
        }
        protected float angle = 0f;//当前旋转角度



        #region Other functions

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Initialize()
        {
            Stroke = LastUsedColor;
            StrokeWidth = LastUsedPenWidth * Zoom;
        }

        public static string Color2String(Color c)
        {
            if (c.IsNamedColor)
            {
                return c.Name;
            }

            byte[] bytes = BitConverter.GetBytes(c.ToArgb());

            string sColor = "#";
            sColor += BitConverter.ToString(bytes, 2, 1);
            sColor += BitConverter.ToString(bytes, 1, 1);
            sColor += BitConverter.ToString(bytes, 0, 1);

            return sColor;
        }

        public virtual string GetXmlStr(SizeF scale, bool noAnimation = true)
        {
            return "";
        }

        public string GetStrStyle(SizeF scale)
        {
            return GetStringStyle(Stroke, Fill, StrokeWidth, scale);
        }

        public static string GetStringStyle(Color color, Color fill, float strokewidth, SizeF scale)
        {
            float strokeWidth = strokewidth / scale.Width;
            string sfill = fill != Color.Empty ? Color2String(fill) : "none";
            string sc = " style = \"fill:" + sfill + "; stroke:" + Color2String(color) + "; stroke-width:" + strokeWidth.ToString(CultureInfo.InvariantCulture) + "\"";
            return sc;
        }

        public virtual void Resize(SizeF newscale, SizeF oldscale)
        {
        }

        public static PointF RecalcPoint(PointF pp, SizeF newscale, SizeF oldscale)
        {
            PointF p = pp;
            p.X = p.X / oldscale.Width;
            p.Y = p.Y / oldscale.Height;
            p.X = p.X * newscale.Width;
            p.Y = p.Y * newscale.Height;
            return p;
        }

        public static float RecalcFloat(float val, float newscale1, float oldscale1)
        {
            val = val / oldscale1;
            val = val * newscale1;
            return val;
        }

        public void RecalcStrokeWidth(SizeF newscale, SizeF oldscale)
        {
            StrokeWidth = RecalcFloat(StrokeWidth, newscale.Width, oldscale.Width);
        }
    
        public void SetStyleFromSvg(SVGBaseShape svg)
        {
            Stroke = svg.Stroke;
            StrokeWidth = ParseSize(svg.StrokeWidth, Dpi.X);
            Fill = svg.Fill != Color.Transparent ? svg.Fill : Color.Empty;
        }
        /// <summary>
        /// 从"rotate(-45.5, 143.5 141)"中解析出角度
        /// </summary>
        /// <param name="transform"></param>
        public static float ParseAngle(string transform)
        {
            var temp = transform.Split(',')[0].Substring(7);
            var angleResult = float.Parse(temp);
            return angleResult;
        }
        public static float ParseSize(string str, float dpi)
        {
            float koef = 1;
            int ind = str.IndexOf("pt");
            if (ind == -1)
                ind = str.IndexOf("px");
            if (ind == -1)
                ind = str.IndexOf("pc");
            if (ind == -1)
            {
                ind = str.IndexOf("cm");
                if (ind > 0)
                {
                    koef = dpi / 2.54f;
                }
            }
            if (ind == -1)
            {
                ind = str.IndexOf("mm");
                if (ind > 0)
                {
                    koef = dpi / 25.4f;
                }
            }
            if (ind == -1)
            {
                ind = str.IndexOf("in");
                if (ind > 0)
                {
                    koef = dpi;
                }
            }
            if (ind > 0)
                str = str.Substring(0, ind);
            str = RemoveAlphas(str);
            try
            {
                float res = float.Parse(str, CultureInfo.InvariantCulture);
                if (koef != 1.1)
                    res *= koef;
                return res;
            }
            catch (Exception ex)
            {
                SVGErr.Log("ParseFloat()", "DrawObject", ex.ToString(), SVGErr._LogPriority.Info);
                return 0;
            }
        }

        static string RemoveAlphas(string str)
        {
            string s = str.Trim();
            string res = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] < '0' || s[i] > '9')
                    if (s[i] != '.')
                        continue;
                res += s[i];
            }
            return res;
        }


        #endregion

        #region ICloneable Members

        public virtual object Clone()
        {
            return MemberwiseClone();
        }


        #endregion

        #region IUpdateSystem
        public virtual void Update()
        {
            
        }

        #endregion
    }
}
