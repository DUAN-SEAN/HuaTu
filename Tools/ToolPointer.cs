using HuaTuDemo.Tools;

namespace HuaTuDemo
{
    using DrawWork;
    using System.Collections;
    using System.Drawing;
    using System.Windows.Forms;


    /// <summary>
    /// Pointer tool
    /// </summary>
    public class ToolPointer : Tool
    {
        #region 字段

        // 保持上一点和当前点的状态（用于移动和调整对象大小）
        private PointF _lastPoint = new PointF(0, 0);

        // 当前已调整大小的对象：
        private DrawObject _resizedObject;
        private int _resizedObjectHandle;

        private DrawObject _rerotateObject;
        private SelectionMode _selectMode = SelectionMode.None;
        private PointF _startPoint = new PointF(0, 0);

        #endregion 字段

        #region 构造器

        public ToolPointer()
        {
            IsComplete = true;
        }

        #endregion 构造器

        #region 枚举

        private enum SelectionMode
        {
            None,
            NetSelection,   // 组选择处于活动状态
            Move,           // 对象是移动的
            Size,            // 对象已调整大小
            Rotate          //对象正在旋转
        }

        #endregion 枚举

        #region 函数

        /// <summary>
        /// 按下鼠标左键
        /// </summary>
        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            _selectMode = SelectionMode.None;
            PointF point = new Point(e.X, e.Y);

            // 调整大小的测试（仅当选择了控件时，光标位于句柄上）
            int n = drawArea.GraphicsList.SelectionCount;

            for (int i = 0; i < n; i++)
            {
                DrawObject o = drawArea.GraphicsList.GetSelectedObject(i);
               
                int handleNumber = o.HitTest(point);
                bool hitOnOutline = o.HitOnCircumferance;

                if (handleNumber > 0)
                {
                    _selectMode = SelectionMode.Size;

                    if (o is DrawConnectLine && handleNumber != 2)
                    {
                        ((DrawConnectLine)o).SetFollowObjectNull(handleNumber);
                    }

                    // 在类成员中保留调整大小的对象
                    _resizedObject = o;
                    _resizedObjectHandle = handleNumber;

                    // 因为我们只想调整一个对象的大小，所以取消选择所有其他对象。
                    drawArea.GraphicsList.UnselectAll();
                    o.Selected = true;
                    o.MouseClickOnHandle(handleNumber);

                    break;
                }

                if (hitOnOutline && (n == 1)) 
                {
                    _selectMode = SelectionMode.Size;
                    o.MouseClickOnBorder();
                    o.Selected = true;
                }

            }


            // 移动测试（光标在对象上）
            if (_selectMode == SelectionMode.None)
            {
                int n1 = drawArea.GraphicsList.Count;
                DrawObject o = null;

                for (int i = 0; i < n1; i++)
                {
                    if (drawArea.GraphicsList[i].HitTest(point) == 0)
                    {
                        o = drawArea.GraphicsList[i];
                        break;
                    }
                }

                if (o != null)
                {
                    _selectMode = SelectionMode.Move;

                    // 如果未按ctrl且尚未选择单击的对象，则取消全选
                    if ((Control.ModifierKeys & Keys.Control) == 0 && !o.Selected)
                        drawArea.GraphicsList.UnselectAll();

                    // 选择单击的对象
                    o.Selected = true;

                    drawArea.Cursor = Cursors.SizeAll;
                }
            }

            //判断是否点击到旋钮
            if (_selectMode == SelectionMode.None)
            {
                for (int i = 0; i < n; i++)
                {
                    DrawObject o = drawArea.GraphicsList.GetSelectedObject(i);
                    bool hitonknob = o.HitKnobTest(point);
                    if (hitonknob)
                    {
                        _selectMode = SelectionMode.Rotate;

                        // 在类成员中保留调整角度的对象
                        _rerotateObject = o;

                        // 因为我们只想调整一个对象的大小，所以取消选择所有其他对象。
                        drawArea.GraphicsList.UnselectAll();
                        o.Selected = true;

                        break;
                    }


                }
            }

            // 净选择
            if (_selectMode == SelectionMode.None)
            {
                // 点击背景
                if ((Control.ModifierKeys & Keys.Control) == 0)
                    drawArea.GraphicsList.UnselectAll();

                _selectMode = SelectionMode.NetSelection;
                drawArea.DrawNetRectangle = true;
            }

            _lastPoint.X = e.X;
            _lastPoint.Y = e.Y;
            _startPoint.X = e.X;
            _startPoint.Y = e.Y;

            drawArea.Capture = true;
            drawArea.NetRectangle = DrawRectangleObject.GetNormalizedRectangle(_startPoint, _lastPoint);

            drawArea.Refresh();
        }

        /// <summary>
        /// 鼠标移动。
        /// 无按钮按下，OT左按钮按下。
        /// </summary>
        public override void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
            var point = new Point(e.X, e.Y);

            // 不按鼠标按钮时设置光标
            if (e.Button == MouseButtons.None)
            {
                Cursor cursor = null;

                for (int i = 0; i < drawArea.GraphicsList.Count; i++)
                {
                    int n = drawArea.GraphicsList[i].HitTest(point);

                    if (n > 0)
                    {
                        cursor = drawArea.GraphicsList[i].GetHandleCursor(n);
                        break;
                    }
                    if (drawArea.GraphicsList[i].HitOnCircumferance)
                    {
                        cursor = drawArea.GraphicsList[i].GetOutlineCursor(n);
                        break;
                    }

                    bool hitknob = drawArea.GraphicsList[i].HitKnobTest(point);
                    if (hitknob)
                    {
                        cursor = drawArea.GraphicsList[i].GetKnodCursor();
                    }
                }

                if (cursor == null)
                    cursor = Cursors.Default;

                drawArea.Cursor = cursor;

                return;
            }

            if (e.Button != MouseButtons.Left)
                return;


            // 查找上一个位置和当前位置之间的差异
            float dx = e.X - _lastPoint.X;
            float dy = e.Y - _lastPoint.Y;

            _lastPoint.X = e.X;
            _lastPoint.Y = e.Y;

            // resize
            if (_selectMode == SelectionMode.Size)
            {
                if (_resizedObject != null)
                {
                    _resizedObject.MoveHandleTo(point, _resizedObjectHandle);
                    drawArea.SetDirty();
                    drawArea.Refresh();
                }
            }

            // move
            if (_selectMode == SelectionMode.Move)
            {
                int n = drawArea.GraphicsList.SelectionCount;

                for (int i = 0; i < n; i++)
                {
                    drawArea.GraphicsList.GetSelectedObject(i).Move(dx, dy);

                }

                drawArea.Cursor = Cursors.SizeAll;
                drawArea.SetDirty();
                drawArea.Refresh();
            }

            if (_selectMode == SelectionMode.Rotate)
            {
                if (_rerotateObject != null)
                {
                    //_rerotateObject.RotateKnobTo(point);
                    _rerotateObject.Rotate((dx + dy) / 2);
                    drawArea.SetDirty();
                    drawArea.Refresh();
                }

            }

            if (_selectMode == SelectionMode.NetSelection)
            {
                drawArea.NetRectangle = DrawRectangleObject.GetNormalizedRectangle(_startPoint, _lastPoint);
                drawArea.Refresh();
                return;
            }
        }

        /// <summary>
        /// 释放鼠标右键
        /// </summary>
        public override void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
            if (_selectMode == SelectionMode.NetSelection)
            {
                // 分组选择
                drawArea.GraphicsList.SelectInRectangle(drawArea.NetRectangle);

                _selectMode = SelectionMode.None;
                drawArea.DrawNetRectangle = false;
            }

            if (_resizedObject != null)
            {
                if (_resizedObjectHandle != 2 && _resizedObject is DrawConnectLine)
                {
                    var line = (DrawConnectLine) _resizedObject;
                    for (int i = 0; i < drawArea.GraphicsList.Count; i++)
                    {
                        if (drawArea.GraphicsList[i] is DrawConnectLine) continue;
                        if (drawArea.GraphicsList[i] is DrawLineObject) continue;
                        if (drawArea.GraphicsList[i].HitTest(line.GetHandle(_resizedObjectHandle)) >= 0)
                        {
                            line.SetFollowObject(_resizedObjectHandle, drawArea.GraphicsList[i]);
                        }
                    }
                }

                // 调整大小后
                _resizedObject.Normalize();
                _resizedObject = null;
                drawArea.ResizeCommand(drawArea.GraphicsList.GetFirstSelected(),
                    new PointF(_startPoint.X, _startPoint.Y),
                    new PointF(e.X, e.Y),
                    _resizedObjectHandle);
            }

            if (_rerotateObject != null)
            {
                _rerotateObject.Normalize();
                _rerotateObject = null;
                drawArea.RerotateCommand(drawArea.GraphicsList.GetFirstSelected(),
                    new PointF(_startPoint.X,_startPoint.Y),
                    new PointF(e.X,e.Y));
            }

            drawArea.Capture = false;
            drawArea.Refresh();

            //立即按命令撤消/重做列表
            if (_selectMode == SelectionMode.Move)
            {
                var movedItemsList = new ArrayList();

                for (int i = 0; i < drawArea.GraphicsList.SelectionCount; i++)
                {
                    movedItemsList.Add(drawArea.GraphicsList.GetSelectedObject(i));
                }

                var delta = new PointF { X = e.X - _startPoint.X, Y = e.Y - _startPoint.Y };
                drawArea.MoveCommand(movedItemsList, delta);
            }

            IsComplete = true;
        }

        #endregion 函数
    }
}