namespace MistyMarkVisualize
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox PB_Image;
        private System.Windows.Forms.NumericUpDown NUD_Tolerance;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            PB_Image = new PictureBox();
            NUD_Tolerance = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)PB_Image).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NUD_Tolerance).BeginInit();
            SuspendLayout();
            // 
            // PB_Image
            // 
            PB_Image.BackgroundImage = Properties.Resources.kitakami2000;
            PB_Image.Location = new Point(0, 0);
            PB_Image.Name = "PB_Image";
            PB_Image.Size = new Size(2000, 2000);
            PB_Image.SizeMode = PictureBoxSizeMode.CenterImage;
            PB_Image.TabIndex = 0;
            PB_Image.TabStop = false;
            // 
            // NUD_Tolerance
            // 
            NUD_Tolerance.DecimalPlaces = 1;
            NUD_Tolerance.Location = new Point(16, 16);
            NUD_Tolerance.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            NUD_Tolerance.Minimum = new decimal(new int[] { 2, 0, 0, 65536 });
            NUD_Tolerance.Name = "NUD_Tolerance";
            NUD_Tolerance.Size = new Size(100, 33);
            NUD_Tolerance.TabIndex = 1;
            NUD_Tolerance.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(2000, 1999);
            Controls.Add(NUD_Tolerance);
            Controls.Add(PB_Image);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)PB_Image).EndInit();
            ((System.ComponentModel.ISupportInitialize)NUD_Tolerance).EndInit();
            ResumeLayout(false);
        }

        #endregion
    }
}
