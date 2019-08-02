using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.Animation
{


    public enum AnimationType
    {
        Animation,
        AnimationColor

    }
    /// <summary>
    /// 所有动画的基类，简单图形上的动画信息
    /// </summary>
    public abstract class AnimationBase
    {
        protected AnimationType _animationType;//动画类型
        protected TimingAttribute _timeTimingAttribute;//时间属性
        protected AnimationAttribute _animationAttribute;//动画属性


        public virtual string GetXmlStr()
        {
            return "";
        }
    }
    //动画基础 控制颜色和一般的动画
    public class Animation : AnimationBase
    {
        protected string _calcMode;//discrete|linear|paced|spline
        protected object _values;//值列表
        protected object _from;//从某个值开始
        protected object _to;//加减到某个值
        protected object _by;//加减某个值
        protected object _keyTimes;//时间列表 控制步调
        protected object _keySplines;//控制贝塞尔曲线

        public override string GetXmlStr()
        {
            return "";
        }
    }
    //动画路径
    public class AnimationPath : Animation
    {
        private string _additive;//-replace|sum
        private string _accumulate;//-none|sum
        private object _path;//路径
        private string origin;//default

    }

    public class AnimationSet : AnimationBase
    {
        private object _to;
    }

}
