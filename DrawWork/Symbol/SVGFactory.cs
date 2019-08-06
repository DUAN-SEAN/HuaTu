using System;
using System.Collections.Generic;
using System.Drawing;
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
                            SVGUse use = group.getChild() as SVGUse;
                            var gDevice = CreateDeviceDrawObjectBase(use, group.Id);//生成一个实体
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

        public static DeviceDrawObjectBase CreateDeviceDrawObjectBase(SVGUse use,string entityId)
        {
            //获取每一个实例化的设备
            DeviceDrawObjectBase vBase = null;
            List<DeviceDrawObjectBase> deviceDrawObjectBases =null;
            List<DrawObject> drawObjects = new List<DrawObject>();
            if (use != null)
            {
                string id = use.Id;//获取实体Id
                var x = float.Parse(use.X);
                var y = float.Parse(use.Y);
                var w = float.Parse(use.Width);
                var h = float.Parse(use.Height);
                var symbol = use.HRef;//设备定义引用 指明使用的哪一种设备类型

                SymbolUnit._Dic.TryGetValue(id, out SymbolUnit value);
                if (value != null)
                {
                    foreach (var unit in value._symbolChildSvgs)
                    {
                        if (unit.getElementType() == SVGUnit.SVGUnitType.use)
                        {
                            
                            var symbolChild = SymbolUnit._Dic[unit.Id];
                            var entityLInk =  CreateDeviceDrawObjectBase(unit as SVGUse, entityId);
                            if (deviceDrawObjectBases == null) deviceDrawObjectBases = new List<DeviceDrawObjectBase>();
                            deviceDrawObjectBases.Add(entityLInk);
                        }
                        else
                        {
                            var o =  SVGDrawFactory.CreateDrawObject(unit);
                            if (o != null)
                            {
                                drawObjects.Add(o);
                            }
                        }
                    }
                    vBase = new DeviceDrawObjectBase(x, y, w, h, entityId, drawObjects, deviceDrawObjectBases, value.SymbolId);

                }



            }

            return vBase;

        }

        public static string GenerateSVGXml(SizeF scale ,List<DeviceDrawObjectBase> devices)
        {
            string s = "";
            s+=GetDefXML();//得到定义

            s += GetGroupXML(scale,devices);//得到生成的设备实体

            return s;
        }

        private static string GetGroupXML(SizeF scale ,List<DeviceDrawObjectBase> devices)
        {
            string s ="";
            s += "<g>";
            foreach (var device in devices)
            {
                s += device.GetXmlStr(scale,true);
                s += "\r\n";
            }

            s += "</g>";
            return s;
        }

        private static string GetDefXML()
        {
            string s = "";
            s += "<defs>";
            foreach (var symBolUnit in SymbolUnit._Dic.Values)
            {
                symBolUnit.GetSymbolXml();
                s += "\r\n";
            }

            s += "</defs>";
            return s;
        }
    }
}
