using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.Animation
{
    /// <summary>
    /// 动画工厂，用来生成动画
    /// </summary>
    public class AnimationFactory
    {
        public object CreateAnimation<T>()
        {
            var o =  Activator.CreateInstance<T>();
       
            return o;
        }
    }
}
