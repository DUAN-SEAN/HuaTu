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
                            var gchild =  group.getChild();
                            switch (gchild.getElementType())
                            {

                                case SVGUnit.SVGUnitType.use:
                                    SVGUse use = gchild as SVGUse;
                                    var gDevice = CreateDeviceDrawObjectBase(use, group.Id);//TODO 后期添加到工作组中
                                 
                                    break;
                                default:
                                    //TODO 未编排为设备的图素集合，暂时用临时分组表示

                                    //vBase = new DeviceDrawObjectBase(0f, 0f, 0f, 0f, group.Id, drawObjects, null, "");
                                    break;

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

        /// <summary>
        /// 手动
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="id"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public static DeviceDrawObjectBase CreateDeviceDrawObjectBase(float x, float y, float w, float h, string id,string entityId)
        {

            //获取每一个实例化的设备
            DeviceDrawObjectBase vBase = null;
            List<DeviceDrawObjectBase> deviceDrawObjectBases = null;
            List<DrawObject> drawObjects = new List<DrawObject>();
        

            var symbol = id;//设备定义引用 指明使用的哪一种设备类型
            SymbolUnit._Dic.TryGetValue(symbol, out SymbolUnit value);
            if (value != null)
            {
                foreach (var unit in value._symbolChildSvgs)
                {
                    if (unit.getElementType() == SVGUnit.SVGUnitType.use)
                    {

                        var symbolChild = SymbolUnit._Dic[unit.Id];
                        var entityLInk = CreateDeviceDrawObjectBase(unit as SVGUse, entityId);
                        if (deviceDrawObjectBases == null) deviceDrawObjectBases = new List<DeviceDrawObjectBase>();
                            deviceDrawObjectBases.Add(entityLInk);
                    }
                    else
                    {
                        var o = SVGDrawFactory.CreateDrawObject(unit);
                        if (o != null)
                        {
                            drawObjects.Add(o);
                        }
                    }
                }
                vBase = new DeviceDrawObjectBase(x, y, w, h, entityId, drawObjects, deviceDrawObjectBases, value.SymbolId);

            }



            

            return vBase;

        }


        public static DeviceDrawObjectBase CreateDeviceDrawObjectBase(SVGUse use,string entityId)
        {
            //获取每一个实例化的设备
            DeviceDrawObjectBase vBase = null;
            List<DeviceDrawObjectBase> deviceDrawObjectBases =null;
            List<DrawObject> drawObjects = new List<DrawObject>();
            if (use != null)
            {
                string id = use.HRef;//获取实体Id
                var x = string.IsNullOrEmpty(use.X)?0f:float.Parse(use.X);
                var y = string.IsNullOrEmpty(use.Y) ? 0f : float.Parse(use.Y);
                var w = string.IsNullOrEmpty(use.Width) ? 0f : float.Parse(use.Width);
                var h = string.IsNullOrEmpty(use.Height) ? 0f : float.Parse(use.Height);
                var symbol = use.HRef.Substring(1);//设备定义引用 指明使用的哪一种设备类型

                SymbolUnit._Dic.TryGetValue(symbol, out SymbolUnit value);
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
