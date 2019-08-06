using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DrawWork.Symbol;

namespace HuaTuDemo.Tools.ToolForm
{
    public partial class ModelBox : Form
    {
        public String ToolSelection = "";

        // Declare the delegate (if using non-generic pattern).
        public delegate void ToolSelectionChangedEventHandler(object sender, EventArgs e);

        // Declare the event.
        public event ToolSelectionChangedEventHandler ToolSelectionChanged;

        private List<RadioButton> _modelList;

        public ModelBox()
        {
            _modelList = new List<RadioButton>();
            InitializeComponent();
        }

        public void FlashUI()
        {
            //AddRadioButton();


            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
        }
        public void LoadModel(List<SymbolUnit> list)
        {
            this.flowLayoutPanel1.Controls.Clear();

            foreach (var unit in list)
            {
                AddRadioButton(unit.SymbolId);
            }
            FlashUI();
        }
        private void AddRadioButton(string unitSymbolId)
        {

            RadioButton rb = new RadioButton();
            rb.Appearance = Appearance.Button;
            rb.AutoSize = true;
            rb.Location = new System.Drawing.Point(3, 3);
            rb.Name = unitSymbolId;
            rb.Size = new System.Drawing.Size(this.Width - 10, this.Height - 10);
            rb.TabIndex = 0;
            rb.TabStop = true;
            rb.Text = "Device:"+ unitSymbolId;
            rb.UseVisualStyleBackColor = true;
            rb.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);

            _modelList.Add(rb);
            this.flowLayoutPanel1.Controls.Add(rb);
        }
        /// <summary>
        /// 通知main选择了哪个tool
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {

            DeviceEventArg deviceEventArg = new DeviceEventArg();

            string modelId = ((RadioButton) (sender)).Name;//暂时用名字表示id

            deviceEventArg.DeviceId = modelId;

            if (!((RadioButton)(sender)).Checked)
                return;

            ToolSelection = "Device";

            if (ToolSelectionChanged != null)
                ToolSelectionChanged(ToolSelection, deviceEventArg);
        }
    }
}
