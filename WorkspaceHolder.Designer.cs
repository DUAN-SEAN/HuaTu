namespace HuaTuDemo
{
    partial class WorkspaceHolder
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label_mouseX = new System.Windows.Forms.Label();
            this.rulerControl_left = new RulerControl();
            this.rulerControl_top = new RulerControl();
            this.svgDrawForm = new WorkSpace();
            this.label_mouseY = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_mouseX
            // 
            this.label_mouseX.AutoSize = true;
            this.label_mouseX.Location = new System.Drawing.Point(-2, 11);
            this.label_mouseX.Name = "label_mouseX";
            this.label_mouseX.Size = new System.Drawing.Size(21, 21);
            this.label_mouseX.TabIndex = 3;
            this.label_mouseX.Text = "0";
            // 
            // rulerControl_left
            // 
            this.rulerControl_left.ActualSize = true;
            this.rulerControl_left.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.rulerControl_left.DivisionMarkFactor = 10;
            this.rulerControl_left.Divisions = 5;
            this.rulerControl_left.ForeColor = System.Drawing.Color.Black;
            this.rulerControl_left.Location = new System.Drawing.Point(0, 27);
            this.rulerControl_left.MajorInterval = 100;
            this.rulerControl_left.MiddleMarkFactor = 8;
            this.rulerControl_left.MouseTrackingOn = true;
            this.rulerControl_left.Name = "rulerControl_left";
            this.rulerControl_left.Orientation = enumOrientation.orVertical;
            this.rulerControl_left.RulerAlignment = enumRulerAlignment.raMiddle;
            this.rulerControl_left.ScaleMode = enumScaleMode.smPixels;
            this.rulerControl_left.Size = new System.Drawing.Size(34, 264);
            this.rulerControl_left.StartValue = 0D;
            this.rulerControl_left.TabIndex = 2;
            this.rulerControl_left.Text = "rulerControl1";
            this.rulerControl_left.VerticalNumbers = false;
            this.rulerControl_left.ZoomFactor = 1D;
            // 
            // rulerControl_top
            // 
            this.rulerControl_top.ActualSize = true;
            this.rulerControl_top.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rulerControl_top.DivisionMarkFactor = 10;
            this.rulerControl_top.Divisions = 5;
            this.rulerControl_top.ForeColor = System.Drawing.Color.Black;
            this.rulerControl_top.Location = new System.Drawing.Point(32, 0);
            this.rulerControl_top.MajorInterval = 100;
            this.rulerControl_top.MiddleMarkFactor = 8;
            this.rulerControl_top.MouseTrackingOn = true;
            this.rulerControl_top.Name = "rulerControl_top";
            this.rulerControl_top.Orientation = enumOrientation.orHorizontal;
            this.rulerControl_top.RulerAlignment = enumRulerAlignment.raMiddle;
            this.rulerControl_top.ScaleMode = enumScaleMode.smPixels;
            this.rulerControl_top.Size = new System.Drawing.Size(269, 29);
            this.rulerControl_top.StartValue = 0D;
            this.rulerControl_top.TabIndex = 1;
            this.rulerControl_top.Text = "rulerControl1";
            this.rulerControl_top.VerticalNumbers = false;
            this.rulerControl_top.ZoomFactor = 1D;
            // 
            // svgDrawForm
            // 
            this.svgDrawForm.AutoScroll = true;
            this.svgDrawForm.BackColor = System.Drawing.Color.Lavender;
            this.svgDrawForm.Location = new System.Drawing.Point(0, 0);
            this.svgDrawForm.Name = "svgDrawForm";
            this.svgDrawForm.Size = new System.Drawing.Size(301, 291);
            this.svgDrawForm.TabIndex = 0;
            this.svgDrawForm.Load += new System.EventHandler(this.SvgDrawForm_Load);
            // 
            // label_mouseY
            // 
            this.label_mouseY.AutoSize = true;
            this.label_mouseY.Location = new System.Drawing.Point(-2, -2);
            this.label_mouseY.Name = "label_mouseY";
            this.label_mouseY.Size = new System.Drawing.Size(21, 21);
            this.label_mouseY.TabIndex = 3;
            this.label_mouseY.Text = "0";
            // 
            // WorkspaceHolder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_mouseY);
            this.Controls.Add(this.label_mouseX);
            this.Controls.Add(this.rulerControl_left);
            this.Controls.Add(this.rulerControl_top);
            this.Controls.Add(this.svgDrawForm);
            this.Name = "WorkspaceHolder";
            this.Resize += new System.EventHandler(this.SvgFormHolderResize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public WorkSpace svgDrawForm;
        private RulerControl rulerControl_top;
        private RulerControl rulerControl_left;
        private System.Windows.Forms.Label label_mouseX;
        private System.Windows.Forms.Label label_mouseY;

    }
}
