namespace HuaTu.Controls.Public.UnitPagePackage.UnitPage
{
    partial class UnitContainer
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
            this._pagesPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // _pagesPanel
            // 
            this._pagesPanel.Location = new System.Drawing.Point(0, 27);
            this._pagesPanel.Name = "_pagesPanel";
            this._pagesPanel.Size = new System.Drawing.Size(40, 40);
            this._pagesPanel.TabIndex = 0;
            // 
            // UnitContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this._pagesPanel);
            this.Size = new System.Drawing.Size(474, 428);
            this.Name = "UnitContainer";
            this.Text = "UnitContainer";
            this.Load += new System.EventHandler(this.UnitContainer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _pagesPanel;

    }
}