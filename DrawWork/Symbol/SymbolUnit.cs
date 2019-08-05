using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVGHelper;

namespace DrawWork.Symbol
{

    public class UseUnit
    {
        /// <summary>
        /// 设备viewBox-x
        /// </summary>
        private float _x;
        /// <summary>
        /// 设备viewBox-y
        /// </summary>
        private float _y;
        /// <summary>
        /// 设备viewBox-width
        /// </summary>
        private float _width;
        /// <summary>
        /// 设备viewBox-height height=""
        /// </summary>
        private float _height;
        /// <summary>
        /// 引用的Id  xlink:href="#Port"
        /// </summary>
        private string _symbolHrefId;

        public UseUnit(float x, float y, float w, float h, string href)
        {
            _x = x;
            _y = y;
            _width = w;
            _height = h;
        }
    }




    public class SymbolUnit
    {
        /// <summary>
        /// 设备id与设备实体的映射
        /// </summary>
        public static Dictionary<string, SymbolUnit> _Dic;
        /// <summary>
        /// 设备类型id
        /// </summary>
        private string _symbolId;
        /// <summary>
        /// 设备viewBox-x
        /// </summary>
        private float x;
        /// <summary>
        /// 设备viewBox-y
        /// </summary>
        private float y;
        /// <summary>
        /// 设备viewBox-width
        /// </summary>
        private float width;
        /// <summary>
        /// 设备viewBox-height
        /// </summary>
        private float height;

        private List<SVGUnit> _symbolChildSvgs;


        public SymbolUnit()
        {

        }

        public SymbolUnit(SVGSymbol svg)
        {
            _symbolChildSvgs = new List<SVGUnit>();

            string viewBox = svg.ViewBox;
            var arr = viewBox.Split(',');
            x = float.Parse(arr[0]);
            y = float.Parse(arr[1]);
            width = float.Parse(arr[2]);
            height = float.Parse(arr[3]);

            SVGUnit unit = svg.getChild();
            while (unit != null)
            {
                _symbolChildSvgs.Add(unit);

                unit = unit.getNext();
            }

        }

        /// <summary>
        /// 根据序列化的信息
        /// </summary>
        /// <param name="svg"></param>
        /// <returns></returns>
        public static SymbolUnit Create(SVGSymbol svg)
        {
            SymbolUnit symbolUnit = null;
            if (_Dic.ContainsKey(svg.Id))
            {
                return null;
            }
            symbolUnit = new SymbolUnit(svg);
            _Dic.Add(svg.Id,symbolUnit);
            return symbolUnit;
        }
    }
}
