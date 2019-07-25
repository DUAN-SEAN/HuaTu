using SVGHelper;
using SVGHelper.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
namespace DrawWork
{
    public sealed class DrawImageObject : DrawRectangleObject
    {
        #region 字段

        public static string CurrentFileName = "";
        public static Image CurrentImage;

        private const string Tag = "image";

        private string _fileName = "";
        private string _id = "";
        private Image _image;
        private bool _reload;

        #endregion 字段

        #region 构造器

        public DrawImageObject()
        {
            SetRectangleF(0, 0, 1, 1);
            Initialize();
        }

        public DrawImageObject(float x, float y)
        {
            if (CurrentImage == null)
            {
                var bmp = new Bitmap(100, 100);
                CurrentImage = bmp;
            }

            InitBox();
            _image = CurrentImage;
            _fileName = CurrentFileName;
            RectangleF = new RectangleF(x, y, _image.Width, _image.Height);
            Initialize();
        }

        public DrawImageObject(string fileName, float x, float y, float width, float height)
        {
            InitBox();
            _fileName = fileName;
            try
            {
                _image = Image.FromFile(fileName);
            }
            catch (Exception ex)
            {
                SVGErr.Log("DrawArea", "DrawImageObject", ex.ToString(), SVGErr._LogPriority.Info);
            }

            RectangleF = new RectangleF(x, y, width, height);
            Initialize();
        }

        #endregion 构造器

        #region 属性

        [EditorAttribute(typeof(System.Windows.Forms.Design.FileNameEditor),
            typeof(System.Drawing.Design.UITypeEditor))]
        public String FileName
        {
            get { return _fileName; }
            set
            {
                _reload = true;
                _fileName = value;
            }
        }

        #endregion 属性


        #region Methods

        public static DrawObject Create(SVGImage svg)
        {
            try
            {
                DrawImageObject dobj;
                if (string.IsNullOrEmpty(svg.Id))
                    dobj = new DrawImageObject(svg.HRef, ParseSize(svg.X, Dpi.X),
                        ParseSize(svg.Y, Dpi.Y),
                        ParseSize(svg.Width, Dpi.X),
                        ParseSize(svg.Height, Dpi.Y));
                else
                {
                    var di = new DrawImageObject();
                    if (!di.FillFromSvg(svg))
                        return null;
                    dobj = di;
                }

                return dobj;
            }
            catch (Exception ex)
            {
                SVGErr.Log("DrawImageObject", "Create", ex.ToString(), SVGErr._LogPriority.Info);
                return null;
            }
        }

        /// <summary>
        /// 从字节数组获取图像对象
        /// </summary>
        public static Image ImageFromBytes(byte[] arrb)
        {
            if (arrb == null)
                return null;
            try
            {
                // Perform the conversion
                var ms = new MemoryStream();
                const int offset = 0;
                ms.Write(arrb, offset, arrb.Length - offset);
                Image im = new Bitmap(ms);
                ms.Close();
                return im;
            }
            catch (Exception ex)
            {
                SVGErr.Log("DrawImageObjecte", "ImageFromBytes", ex.ToString(), SVGErr._LogPriority.Info);
                return null;
            }
        }

        /// <summary>
        /// 将图像从文件加载到字节数组
        /// </summary>
        /// <param name="flnm">File name</param>
        /// <returns>byte array</returns>
        public static byte[] ReadPngMemImage(string flnm)
        {
            try
            {
                FileStream fs = new FileStream(flnm, FileMode.Open, FileAccess.Read);
                MemoryStream ms = new MemoryStream();
                Bitmap bm = new Bitmap(fs);
                bm.Save(ms, ImageFormat.Png);
                BinaryReader br = new BinaryReader(ms);
                ms.Position = 0;
                byte[] arrpic = br.ReadBytes((int) ms.Length);
                br.Close();
                fs.Close();
                ms.Close();
                return arrpic;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading file " + ex, "");
                SVGErr.Log("DrawImageObjecte", "ReadPngMemImage", ex.ToString(), SVGErr._LogPriority.Info);
                return null;
            }
        }

        public override void Draw(Graphics g)
        {
            try
            {
                if (_reload)
                {
                    _image = ImageFromBytes(ReadPngMemImage(_fileName));
                    Width = _image.Width;
                    Height = _image.Height;
                    _reload = false;
                }

                if (_image != null)
                    g.DrawImage(_image, RectangleF);
                else
                    base.Draw(g);
            }
            catch (Exception ex)
            {
                SVGErr.Log("DrawImageObject", "Draw", ex.ToString(), SVGErr._LogPriority.Info);
            }
        }

        [CLSCompliant(false)]
        public bool FillFromSvg(SVGImage svg)
        {
            try
            {
                float x = ParseSize(svg.X, Dpi.X);
                float y = ParseSize(svg.Y, Dpi.Y);
                float w = ParseSize(svg.Width, Dpi.X);
                float h = ParseSize(svg.Height, Dpi.Y);
                RectangleF = new RectangleF(x, y, w, h);
                _fileName = svg.HRef;
                _id = svg.Id;
                try
                {
                    _image = Image.FromFile(_fileName);
                    return true;
                }
                catch (Exception ex)
                {
                    SVGErr.Log("DrawImageObject", "FillFromSvg", ex.ToString(), SVGErr._LogPriority.Info);
                    return false;
                }
            }
            catch (Exception ex0)
            {
                SVGErr.Log("DrawImageObject", "FillFromSvg", ex0.ToString(), SVGErr._LogPriority.Info);
                return false;
            }
        }

        public override string GetXmlStr(SizeF scale)
        {
            string s = "<";
            s += Tag;
            if (_id.Length > 0)
            {
                s += " id= \"" + _id + "\" ";
            }

            s += GetRectStringXml(RectangleF, scale, "") + "\r\n";
            // trim directory name
            string flnm = _fileName;
            if (_fileName.IndexOf(":", 0) > 0)
            {
                string dir = Directory.GetCurrentDirectory();
                if (_fileName.IndexOf(dir, 0) == 0 && dir.Length < _fileName.Length)
                {
                    flnm = _fileName.Substring(dir.Length + 1, _fileName.Length - dir.Length - 1);
                }
            }

            s += " xlink:href = \"" + flnm + "\">" + "\r\n";
            s += "</" + Tag + ">" + "\r\n";
            return s;
        }

        void InitBox()
        {
            Stroke = Color.Red;
            StrokeWidth = 1;
        }

        #endregion Methods
    }
}
       
