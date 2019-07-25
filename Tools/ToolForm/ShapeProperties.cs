using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuaTuDemo.Tools.ToolForm
{
    public partial class ShapeProperties : Form
    {
        public delegate void OnPropertyChanged(object sender, System.Windows.Forms.PropertyValueChangedEventArgs e);
        public event OnPropertyChanged PropertyChanged;

        public ShapeProperties()
        {
            InitializeComponent();
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(s, e);
        }

        private void PropertyGrid_Click(object sender, EventArgs e)
        {

        }
    }
}
