using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVGHelper;
using SVGHelper.Base;
using SVGHelper.Device;

namespace DrawWork.Symbol
{
    public class SVGDrawFactory
    {
        public static DrawObject CreateDrawObject(SVGUnit svge)
        {
            DrawObject o = null;
            switch (svge.getElementType())
            {
                case SVGUnit.SVGUnitType.typeLine:
                    o = DrawLineObject.Create((SVGLine)svge);
                    break;
                case SVGUnit.SVGUnitType.typeRect:
                    o = DrawRectangleObject.Create((SVGRect)svge);
                    break;
                case SVGUnit.SVGUnitType.typeEllipse:
                    o = DrawEllipseObject.Create((SVGEllipse)svge);
                    break;
                case SVGUnit.SVGUnitType.typePolyline:
                    o = DrawPolygonObject.Create((SVGPolyline)svge);
                    break;
                case SVGUnit.SVGUnitType.typeImage:
                    o = DrawImageObject.Create((SVGImage)svge);
                    break;
                case SVGUnit.SVGUnitType.typeText:
                    o = DrawTextObject.Create((SVGText)svge);
                    break;
                case SVGUnit.SVGUnitType.typePath:
                    o = DrawPathObject.Create((SVGPath)svge);

                    break;
                case SVGUnit.SVGUnitType.typeCircle:
                    o = DrawCircleObject.Create((SVGCircle) svge);
                    break;
                default:
                    break;
            }
            return o;
        }
    }
}
