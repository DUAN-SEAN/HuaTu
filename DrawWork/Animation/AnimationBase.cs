using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVGHelper;

namespace DrawWork.Animation
{


    public enum AnimationType
    {
        None,
        Animation,
        AnimationColor,
        AnimationPath

    }

    public interface ICheckValue
    {
        bool CheckValue(object o);

    }
    /// <summary>
    /// 所有动画的基类，简单图形上的动画信息
    /// </summary>
    public abstract class AnimationBase:ICheckValue
    {
        public AnimationAttribute AnimationAttr
        {
            set => _animationAttribute = value;
            get => _animationAttribute;
        }

        public TimingAttribute TimingAttr
        {
            set => _timeTimingAttribute = value;
            get => _timeTimingAttribute;
        }

        public virtual AnimationType AnimationType { get; }

        public AnimationType _animationType;//动画类型
        protected TimingAttribute _timeTimingAttribute;//时间属性
        protected AnimationAttribute _animationAttribute;//动画属性

        public AnimationBase(AnimationType type)
        {
            _animationType = type;
            _timeTimingAttribute = new TimingAttribute();
            _animationAttribute = new AnimationAttribute();
        }

        public AnimationBase()
        {
            _animationType = AnimationType.None;
            _timeTimingAttribute = new TimingAttribute();
            _animationAttribute = new AnimationAttribute();
        }

        public virtual string GetXmlStr()
        {
            return _timeTimingAttribute.GetXMLStr()+_animationAttribute.GetXMLStr();
        }

        public bool CheckValue(object o)
        {
            if (o != null && o.ToString() != "")
            {
                return true;
            }

            return false;
        }
    }
    //动画基础 控制颜色和一般的动画
    public class Animation : AnimationBase
    {
        public string CalcMode
        {
            get => _calcMode;
            set => _calcMode = value;
        }

        public string Values
        {
            get => _values;
            set => _values = value;
        }

        public string From
        {
            get => _from;
            set => _from = value;
        }

        public string To
        {
            get => _to;
            set => _to = value;
        }
        public string By
        {
            get => _by;
            set => _by = value;
        }

        public string KeyTimes
        {
            get => _keyTimes;
            set => _keyTimes = value;
        }

        public string KeySplines
        {
            get => _keySplines;
            set => KeySplines = value;
        }

        public override AnimationType AnimationType
        {
            get => AnimationType.Animation;
        }

        protected string _calcMode = "linear";//discrete|linear|paced|spline
        protected string _values ;//值列表
        protected string _from;//从某个值开始
        protected string _to;//加减到某个值
        protected string _by;//加减某个值
        protected string _keyTimes;//时间列表 控制步调
        protected string _keySplines;//控制贝塞尔曲线

        /// <summary>
        /// 从svg读取
        /// </summary>
        /// <param name="svgAnimate"></param>
        public Animation(SVGAnimate svgAnimate)
        {
            TimingAttr.Begin = svgAnimate.Begin;
            TimingAttr.Dur = svgAnimate.Dur;
            TimingAttr.End = svgAnimate.End;
            TimingAttr.Fill = svgAnimate.Fill;
            TimingAttr.RepeatCount = svgAnimate.RepeatCount;
            TimingAttr.RepeatDur = svgAnimate.RepeatDur;
            TimingAttr.Restart = svgAnimate.Restart;


            _calcMode = svgAnimate.CalcMode;
            _values = svgAnimate.Values;
            _from = svgAnimate.From;
            _to = svgAnimate.To;
            _by = svgAnimate.By;
            _keyTimes = svgAnimate.KeyTimes;
            _keySplines = svgAnimate.KeySplines;


        }
        public Animation() { }

        public override string GetXmlStr()
        {
            string s = base.GetXmlStr();

            if (CheckValue(CalcMode))
                s += " calcMode=\"" + CalcMode + "\" ";
            if (CheckValue(Values))
                s += " values=\"" + Values + "\" ";
            if (CheckValue(From))
                s += " from=\"" + From + "\" ";
            if (CheckValue(To))
                s += " to=\"" + To + "\" ";
            if (CheckValue(By))
                s += " by=\"" + By + "\" ";
            if (CheckValue(KeyTimes))
                s += " keyTimes=\"" + KeyTimes + "\" ";
            if (CheckValue(KeySplines))
                s += " keySplines=\"" + KeySplines + "\" ";

            return s;





        }

       
    }
    //动画路径
    public class AnimationPath : Animation
    {

        private string _additive;//-replace|sum
        private string _accumulate;//-none|sum
        private PointF[] _path;//路径
        private string origin;//default
        public override AnimationType AnimationType
        {
            get => AnimationType.AnimationPath;
        }
        public AnimationPath(PointF[] path)
        {
            _path = path;
        }
        public AnimationPath(SVGAnimate svgAnimate) : base(svgAnimate)
        {
        }

        public override string GetXmlStr()
        {


            string s =" path=\"M";
            foreach (var p in _path)
            {
                s += " " + p.X + " " + p.Y;
            }

            s += "Z\" ";


            if (CheckValue(CalcMode))
                s += " calcMode=\"" + CalcMode + "\" ";
            if (CheckValue(Values))
                s += " values=\"" + Values + "\" ";
            if (CheckValue(From))
                s += " from=\"" + From + "\" ";
            if (CheckValue(To))
                s += " to=\"" + To + "\" ";
            if (CheckValue(By))
                s += " by=\"" + By + "\" ";
            if (CheckValue(KeyTimes))
                s += " keyTimes=\"" + KeyTimes + "\" ";
            if (CheckValue(KeySplines))
                s += " keySplines=\"" + KeySplines + "\" ";

            return s;
        }
    }

    public class AnimationSet : AnimationBase
    {
        private object _to;
    }



}
