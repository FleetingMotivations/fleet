namespace LatticeViewer
{
    partial class PictureViewer
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
            this.imageView = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.imageView)).BeginInit();
            this.SuspendLayout();
            // 
            // imageView
            // 
            this.imageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageView.Location = new System.Drawing.Point(0, 0);
            this.imageView.Name = "imageView";
            this.imageView.Size = new System.Drawing.Size(284, 261);
            this.imageView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.imageView.TabIndex = 0;
            this.imageView.TabStop = false;
            // 
            // PictureViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.imageView);
            this.Name = "PictureViewer";
            this.Text = "PictureViewer";
            ((System.ComponentModel.ISupportInitialize)(this.imageView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox imageView;
    }
}