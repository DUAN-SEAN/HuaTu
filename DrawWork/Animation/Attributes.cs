using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DrawWork.Animation
{
    public interface IXMLSupprot
    {
        string GetXMLStr();
    }


    public class TimingAttribute:IXMLSupprot
    {

        #region 属性


        public int Begin { set; get; }
        public int Dur { set; get; }
        public int End { set; get; }
        public string Restart
        {
            set
            {
                if (value.Equals("always") || value.Equals("never") || value.Equals("whenNotActive"))
                {
                    _restart = value;
                }
            }
            get { return _restart; }
        }// always|never|whenNotActive  def = always

        public object RepeatCount
        {
            set { _repeatCount = value; }
            get { return _repeatCount; }
        }

        public object RepeatDur
        {
            set { _repeatDur = value; }
            get { return _repeatDur; }
        }

        public string Fill
        {
            set
            {
                if (value.Equals("remove") || value.Equals("freeze"))
                {
                    _fill = value;
                }

            }
            get { return _fill; }
        }

        


        #endregion

        #region 字段

        private string _restart = "always";//默认
        private object _repeatCount = "indefinite";//默认
        private object _repeatDur = "indefinite";//默认
        private string _fill = "remove";//默认 //remove|freeze def remove


        #endregion


        #region 函数
        /// <summary>
        /// 获取属性的XML
        /// </summary>
        /// <returns></returns>
        public string GetXMLStr()
        {
            return " begin=\"" + Begin + "\" " + " dur=\"" + Dur + "\" " +
                   " end=\"" + End + "\" " + " restart=\"" + Restart + "\" " +
                   " repeatCount=\"" + RepeatCount + "\" " + " repeatDur=\"" + RepeatDur + "\" " +
                   " fill=\"" + Fill + "\" ";
        }


        #endregion
    }
    /// <summary>
    /// animation 属性
    /// </summary>
    public class AnimationAttribute
    {
        #region 属性

        public string AttributeName
        {
            set => _attributeName = value;
            get => _attributeName;
        }

        public string AttributeType
        {
            set => _attributeType = value;
            get =>  _attributeType;
        }//css|xml|auto def auto 


        public string Additive
        {

            set => _additive = value;
            get => _additive;
        }

        public string Accumulate
        {
            set => _accumulate = value;
            get => _accumulate;
        }

        //animTargetAttr // 目标文档，一般默认父文档
        public string TargetElement
        {
            set => _targetElement = value;
            get => _targetElement;
        }

        //animLinkAttrs 一般用不上
        public string Type
        {
            set => _type = value;
            get => _type;
        } //simple|extended|locator|arc def simple

        public string Show
        {
            set => _show = value;
            get => _show;
        } //new|embed|replace def embed

        public string Actuate
        {
            set => _actuate = value;
            get => _actuate;
        } //user|auto def auto

        private string Href
        {
            set => _href = value;
            get => _href;
        }

        #endregion

        #region 字段

        private string _attributeName = "";//属性名称
        private string _attributeType = "";//css|xml|auto def auto 
        private string _additive = "";//replace|sum def replace 默认动画结束后是替换还是递增
        private string _accumulate = "";//none|sum def none

        //animTargetAttr // 目标文档，一般默认父文档
        private string _targetElement = "";
        //animLinkAttrs 一般用不上
        private string _type = "";//simple|extended|locator|arc def simple
        private string _show = "";//new|embed|replace def embed
        private string _actuate = "";//user|auto def auto
        private string _href = "";


        #endregion



        #region 函数

        /// <summary>
        /// 返回属性的xml构造但不
        /// </summary>
        /// <returns></returns>
        public string GetXMLStr()
        {
            return " attributeName=\"" + AttributeName + "\" " + " attributeType=\"" + AttributeType + "\" " +
                   " additive=\"" + Additive + "\" " + " accumulate=\"" + Accumulate + "\" " +
                   " targetElement=\"" + TargetElement + "\" " + " type=\"" + Type + "\" " +
                   " show=\"" + Show + "\" " + " actuate=\"" + Actuate + "\" " + " href=\"" + Href + "\" ";

        }

        #endregion

    }
}
