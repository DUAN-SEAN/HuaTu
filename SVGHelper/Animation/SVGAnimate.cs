using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVGHelper.Base;

namespace SVGHelper
{
    public class SVGAnimate:SVGUnit
    {

        #region 属性

        public string Begin
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrAnimTiming_Begin);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrAnimTiming_Begin, value);
            }
        }

        public string Dur
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrAnimTiming_Dur);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrAnimTiming_Dur, value);
            }

        }
        public string End
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrAnimTiming_End);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrAnimTiming_End, value);
            }

        }
        public string Restart
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrAnimTiming_Restart);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrAnimTiming_Restart, value);
            }
        }

        public string RepeatCount
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrAnimTiming_RepeatCount);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrAnimTiming_RepeatCount, value);
            }
        }
        public string RepeatDur {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrAnimTiming_RepeatDur);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrAnimTiming_RepeatDur, value);
            }
        }

        public string Fill
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrAnimTiming_Fill);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrAnimTiming_Fill, value);
            }
        }



        public string AttributeName
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrAnim_AttributeName);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrAnim_AttributeName, value);
            }
        }

        public string AttributeType
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrAnim_AttributeType);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrAnim_AttributeType, value);
            }
        }//css|xml|auto def auto 


        public string Additive
        {

            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrAnim_Additive);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrAnim_Additive, value);
            }
        }

        public string Accumulate
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrAnim_Accumulate);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrAnim_Accumulate, value);
            }
        }

        //animTargetAttr // 目标文档，一般默认父文档
        public string TargetElement
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrAnim_TargetElement);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrAnim_TargetElement, value);
            }
        }

        //animLinkAttrs 一般用不上
        public string Type
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrAnim_Type);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrAnim_Type, value);
            }
        } //simple|extended|locator|arc def simple

        public string Show
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrAnim_Show);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrAnim_Show, value);
            }
        } //new|embed|replace def embed

        public string Actuate
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrAnim_Actuate);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrAnim_Actuate, value);
            }
        } //user|auto def auto

        private string Href
        {
            get
            {
                return GetAttributeStringValue(SVGAttribute._SvgAttribute.attrAnim_Href);
            }

            set
            {
                SetAttributeValue(SVGAttribute._SvgAttribute.attrAnim_Href, value);
            }
        }

        #endregion


        public SVGAnimate(SVGWord doc) : base(doc)
        {

            Init();
            

        }

        private void Init()
        {
            m_sElementName = "animate";
            m_ElementType = SVGUnitType.typeAnimate;

            AddAttr(SVGAttribute._SvgAttribute.attrAnimTiming_Begin, "");
            AddAttr(SVGAttribute._SvgAttribute.attrAnimTiming_Dur, "");
            AddAttr(SVGAttribute._SvgAttribute.attrAnimTiming_End, "");
            AddAttr(SVGAttribute._SvgAttribute.attrAnimTiming_Restart, "always");
            AddAttr(SVGAttribute._SvgAttribute.attrAnimTiming_RepeatDur, "indefinite");
            AddAttr(SVGAttribute._SvgAttribute.attrAnimTiming_RepeatCount, "indefinite");
            AddAttr(SVGAttribute._SvgAttribute.attrAnimTiming_Fill, "remove");
            AddAttr(SVGAttribute._SvgAttribute.attrAnim_AttributeName, "");
            AddAttr(SVGAttribute._SvgAttribute.attrAnim_AttributeType, "");
            AddAttr(SVGAttribute._SvgAttribute.attrAnim_Additive, "replace");
            AddAttr(SVGAttribute._SvgAttribute.attrAnim_Accumulate, "none");
            AddAttr(SVGAttribute._SvgAttribute.attrAnim_TargetElement, "");
            AddAttr(SVGAttribute._SvgAttribute.attrAnim_Type, "");
            AddAttr(SVGAttribute._SvgAttribute.attrAnim_Show, "");
            AddAttr(SVGAttribute._SvgAttribute.attrAnim_Actuate, "auto");
            AddAttr(SVGAttribute._SvgAttribute.attrAnim_Href, "");
            AddAttr(SVGAttribute._SvgAttribute.attrAnim_CalcMode, "linear");
            AddAttr(SVGAttribute._SvgAttribute.attrAnim_Values, "");
            AddAttr(SVGAttribute._SvgAttribute.attrAnim_KeySplines, "");
            AddAttr(SVGAttribute._SvgAttribute.attrAnim_KeyTimes, "");
            AddAttr(SVGAttribute._SvgAttribute.attrAnim_From, "");
            AddAttr(SVGAttribute._SvgAttribute.attrAnim_To, "");
            AddAttr(SVGAttribute._SvgAttribute.attrAnim_By, "");

        }
    }
}
