using System.Drawing;

namespace HuaTuDemo
{
    partial class WorkSpace
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkSpace));
            this.drawArea = new HuaTuDemo.DrawArea();
            this.SuspendLayout();
            // 
            // drawArea
            // 
            this.drawArea.ActiveTool = HuaTuDemo.DrawArea.DrawToolType.Pointer;
            this.drawArea.AutoScroll = true;
            this.drawArea.AutoSize = true;
            this.drawArea.Description = "Svg picture";
            this.drawArea.DrawGrid = false;
            this.drawArea.DrawNetRectangle = false;
            this.drawArea.FileName = null;
            this.drawArea.GraphicsList = null;
            this.drawArea.Location = new System.Drawing.Point(122, 125);
            this.drawArea.Name = "drawArea";
            this.drawArea.NetRectangle = ((System.Drawing.RectangleF)(resources.GetObject("drawArea.NetRectangle")));
            this.drawArea.OldScale = new System.Drawing.SizeF(1F, 1F);
            this.drawArea.OriginalSize = new System.Drawing.SizeF(500F, 400F);
            this.drawArea.Owner = null;
            this.drawArea.ScaleDraw = new System.Drawing.SizeF(1F, 1F);
            this.drawArea.Size = new System.Drawing.Size(66, 66);
            this.drawArea.SizePicture = new System.Drawing.SizeF(500F, 400F);
            this.drawArea.TabIndex = 0;
            // 
            // WorkSpace
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Lavender;
            this.Controls.Add(this.drawArea);
            this.DoubleBuffered = true;
            this.Name = "WorkSpace";
            this.Size = new System.Drawing.Size(234, 228);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DrawArea drawArea;
        public Point _scrollPersistance = new Point(0, 0);

    }
}