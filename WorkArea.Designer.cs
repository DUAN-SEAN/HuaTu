using HuaTu.Controls.Public.UnitPagePackage.Drawer;
using HuaTu.Controls.Public.UnitPagePackage.UnitPage;

namespace HuaTuDemo
{
    partial class WorkArea
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TopUnitButtonDrawer topTabButtonRenderer1 = new TopUnitButtonDrawer();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkArea));
            this.tabbedView = new UnitContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.SuspendLayout();
            // 
            // tabbedView
            // 
            this.tabbedView.BackGradient1 = System.Drawing.SystemColors.Control;
            this.tabbedView.BackGradient2 = System.Drawing.SystemColors.ControlLightLight;
            this.tabbedView.BackGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            topTabButtonRenderer1.BackGradient1 = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            topTabButtonRenderer1.BackGradient2 = System.Drawing.Color.White;
            topTabButtonRenderer1.BackGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            topTabButtonRenderer1.Border1 = System.Drawing.Color.White;
            topTabButtonRenderer1.Border2 = System.Drawing.Color.DarkGray;
            topTabButtonRenderer1.SelectedBackGradient1 = System.Drawing.Color.White;
            topTabButtonRenderer1.SelectedBackGradient2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(223)))), ((int)(((byte)(227)))));
            topTabButtonRenderer1.SelectedBorder1 = System.Drawing.Color.White;
            topTabButtonRenderer1.SelectedBorder2 = System.Drawing.Color.DarkGray;
            topTabButtonRenderer1.TextColor = System.Drawing.Color.Black;
            this.tabbedView.ButtonsRenderer = topTabButtonRenderer1;
            this.tabbedView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabbedView.Location = new System.Drawing.Point(0, 0);
            this.tabbedView.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.tabbedView.Name = "tabbedView";
            this.tabbedView.SelectedIndex = -1;
            this.tabbedView.ShowOneTabButton = true;
            this.tabbedView.Size = new System.Drawing.Size(535, 408);
            this.tabbedView.TabIndex = 1;
            this.tabbedView.PageSelectionMade += new UnitContainer.OnPageSelectionMade(this.TabbedViewPageSelectionMade);
            this.tabbedView.Load += new System.EventHandler(this.TabbedView_Load);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.statusStrip1.Location = new System.Drawing.Point(0, 408);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 26, 0);
            this.statusStrip1.Size = new System.Drawing.Size(535, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // WorkArea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 430);
            this.Controls.Add(this.tabbedView);
            this.Controls.Add(this.statusStrip1);
            //this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.Name = "WorkArea";
            this.Text = "WorkArea";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UnitContainer tabbedView;
        private System.Windows.Forms.StatusStrip statusStrip1;

    }
}