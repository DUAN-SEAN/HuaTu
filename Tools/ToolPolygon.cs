using DrawWork;
using HuaTuDemo.Tools;
using SVGHelper.Base;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace HuaTuDemo
{
    /// <summary>
    /// Polygon tool
    /// </summary>
    public class ToolPolygon : ToolObject
    {
        #region 字段

        private const int MinDistance = 15 * 15;

        private int _lastX, _lastY;
        private DrawPolygonObject _newPolygon;

        #endregion 字段

        #region 构造器

        public ToolPolygon()
        {
            //Cursor = new Cursor(GetType(), "Pencil.cur");
            Cursor = new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("HuaTuDemo.Resources.Pencil.cur"));
        }

        #endregion 构造器

        #region 函数

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            // 创建新多边形，将其添加到列表中
            // 并保留对它的引用
            _newPolygon = new DrawPolygonObject(e.X, e.Y, e.X + 1, e.Y + 1);
            AddNewObject(drawArea, _newPolygon);
            _lastX = e.X;
            _lastY = e.Y;
        }

        /// <summary>
        /// 鼠标-调整新多边形的大小
        /// </summary>
        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            drawArea.Cursor = Cursor;

            if (e.Button != MouseButtons.Left)
                return;

            if (_newPolygon == null)
                return;                 // precaution

            var point = new Point(e.X, e.Y);
            int distance = (e.X - _lastX) * (e.X - _lastX) + (e.Y - _lastY) * (e.Y - _lastY);
            try
            {
                if (distance < MinDistance)
                {
                    //最后两点之间的距离小于最小值-
                    //移动最后一个点
                    _newPolygon.MoveHandleTo(point, _newPolygon.HandleCount);
                }
                else
                {
                    // 添加新点
                    _newPolygon.AddPoint(point);
                    _lastX = e.X;
                    _lastY = e.Y;
                }
                drawArea.Refresh();
            }
            catch (Exception ex)
            {
                SVGErr.Log("ToolPolygon", "OnMouse", ex.ToString(), SVGErr._LogPriority.Info);
            }
        }

        public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
            _newPolygon = null;
            IsComplete = true;

            base.OnMouseUp(drawArea, e);
        }

        #endregion 函数
    }
}