using HuaTu.Controls.Public.UnitPagePackage.Data;
using System.Windows.Forms;

namespace HuaTu.Controls.Public.UnitPagePackage.UnitPage
{
    public partial class UnitPage : UserControl
    {
        #region 字段

        private UnitButton _button = null;

        #endregion 字段.

        #region 构造器.

        public UnitPage()
        {
            InitializeComponent();

            _button = new UnitButton(this);
        }

        public UnitPage(string text) : this()
        {
            Text = text;
        }

        #endregion 构造器.

        #region 公开函数

        /// <summary>
        /// 与页面关联的按钮
        /// </summary>
        internal UnitButton Button
        {
            get { return _button; }
        }

        #endregion 公开函数

        private void UnitPage_Load(object sender, System.EventArgs e)
        {

        }
    }
}
