using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using SVGHelper;
using SVGHelper.Base;

namespace DrawWork.Symbol
{
    public class SVGFactory
    {
        /// <summary>
        /// 根据读取的xml生成项目
        /// </summary>
        /// <param name="svg"></param>
        public static void CreateProjectFromXML(SVGUnit svg)
        {
            while (svg != null)
            {
                switch (svg.getElementType())
                {
                    case SVGUnit.SVGUnitType.def://将所有symbol读取
                        SVGUnit defchild = svg.getChild();
                        while (defchild != null)
                        {
                            if (defchild.getElementType() == SVGUnit.SVGUnitType.symbol)
                            {
                                CreateSysmbolUnit(defchild as SVGSymbol);
                            }

                            defchild = defchild.getNext();
                        }
                        break;
                    case SVGUnit.SVGUnitType.typeGroup:
                        SVGGroup group = svg.getChild() as SVGGroup;
                        while (group != null)
                        {
                            
                            //获取每一个实例化的设备
                            SVGUse use = group.getChild() as SVGUse;
                            if (use != null)
                            {
                                string id = group.Id;//获取实体Id
                                var x = use.X;
                                var y = use.Y;
                                var w = use.Width;
                                var h = use.Height;
                                var symbol = use.HRef;//设备定义引用 指明使用的哪一种设备类型

                                //TODO:后期设备实体需要带数据放入元数据中metadata
                                //生成设备实体 由吴悠提供


                            }
                            group = group.getNext() as SVGGroup;//获取下一个
                        }

                        break;
                    default:break;
                        
                        
                }

                svg = svg.getNext();
            }
        }

        public static void CreateSysmbolUnit(SVGSymbol svg)
        {
            SymbolUnit.Create(svg);
        }
    }
}
