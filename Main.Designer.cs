namespace MistyMarkVisualize
{
    partial class Main
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
            B_ExportIntersections = new Button();
            FLP_Alternate = new FlowLayoutPanel();
            CB_Coordinates1 = new ComboBox();
            CB_Coordinates2 = new ComboBox();
            B_ExportCreated = new Button();
            B_ClearCreated = new Button();
            B_BB = new Button();
            B_OpenCoords = new Button();
            L_Coordinate = new Label();
            ((System.ComponentModel.ISupportInitialize)PB_Image).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NUD_Tolerance).BeginInit();
            FLP_Alternate.SuspendLayout();
            SuspendLayout();
            // 
            // PB_Image
            // 
            PB_Image.BackgroundImage = Properties.Resources.kitakami2000;
            PB_Image.BackgroundImageLayout = ImageLayout.Zoom;
            PB_Image.Dock = DockStyle.Fill;
            PB_Image.Location = new Point(0, 0);
            PB_Image.Margin = new Padding(2);
            PB_Image.Name = "PB_Image";
            PB_Image.Size = new Size(1273, 1359);
            PB_Image.SizeMode = PictureBoxSizeMode.Zoom;
            PB_Image.TabIndex = 0;
            PB_Image.TabStop = false;
            PB_Image.Click += PB_Image_Click;
            PB_Image.MouseMove += TrackCoordinate;
            // 
            // NUD_Tolerance
            // 
            NUD_Tolerance.DecimalPlaces = 1;
            NUD_Tolerance.Location = new Point(10, 11);
            NUD_Tolerance.Margin = new Padding(2);
            NUD_Tolerance.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            NUD_Tolerance.Minimum = new decimal(new int[] { 2, 0, 0, 65536 });
            NUD_Tolerance.Name = "NUD_Tolerance";
            NUD_Tolerance.Size = new Size(64, 25);
            NUD_Tolerance.TabIndex = 1;
            NUD_Tolerance.Value = new decimal(new int[] { 17, 0, 0, 0 });
            // 
            // B_ExportIntersections
            // 
            B_ExportIntersections.Location = new Point(395, 3);
            B_ExportIntersections.Name = "B_ExportIntersections";
            B_ExportIntersections.Size = new Size(152, 32);
            B_ExportIntersections.TabIndex = 3;
            B_ExportIntersections.Text = "Export Intersections";
            B_ExportIntersections.UseVisualStyleBackColor = true;
            B_ExportIntersections.Click += B_ExportIntersections_Click;
            // 
            // FLP_Alternate
            // 
            FLP_Alternate.BackColor = Color.Transparent;
            FLP_Alternate.Controls.Add(CB_Coordinates1);
            FLP_Alternate.Controls.Add(CB_Coordinates2);
            FLP_Alternate.Controls.Add(B_ExportIntersections);
            FLP_Alternate.Controls.Add(B_ExportCreated);
            FLP_Alternate.Controls.Add(B_ClearCreated);
            FLP_Alternate.Controls.Add(B_BB);
            FLP_Alternate.Controls.Add(B_OpenCoords);
            FLP_Alternate.Location = new Point(76, 0);
            FLP_Alternate.Margin = new Padding(0);
            FLP_Alternate.Name = "FLP_Alternate";
            FLP_Alternate.Size = new Size(1188, 36);
            FLP_Alternate.TabIndex = 5;
            // 
            // CB_Coordinates1
            // 
            CB_Coordinates1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            CB_Coordinates1.AutoCompleteSource = AutoCompleteSource.ListItems;
            CB_Coordinates1.DropDownStyle = ComboBoxStyle.DropDownList;
            CB_Coordinates1.FormattingEnabled = true;
            CB_Coordinates1.Location = new Point(8, 8);
            CB_Coordinates1.Margin = new Padding(8);
            CB_Coordinates1.Name = "CB_Coordinates1";
            CB_Coordinates1.Size = new Size(180, 25);
            CB_Coordinates1.TabIndex = 9;
            // 
            // CB_Coordinates2
            // 
            CB_Coordinates2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            CB_Coordinates2.AutoCompleteSource = AutoCompleteSource.ListItems;
            CB_Coordinates2.DropDownStyle = ComboBoxStyle.DropDownList;
            CB_Coordinates2.FormattingEnabled = true;
            CB_Coordinates2.Location = new Point(204, 8);
            CB_Coordinates2.Margin = new Padding(8);
            CB_Coordinates2.Name = "CB_Coordinates2";
            CB_Coordinates2.Size = new Size(180, 25);
            CB_Coordinates2.TabIndex = 4;
            // 
            // B_ExportCreated
            // 
            B_ExportCreated.Location = new Point(553, 3);
            B_ExportCreated.Name = "B_ExportCreated";
            B_ExportCreated.Size = new Size(152, 32);
            B_ExportCreated.TabIndex = 5;
            B_ExportCreated.Text = "Export Created";
            B_ExportCreated.UseVisualStyleBackColor = true;
            B_ExportCreated.Click += B_ExportCreated_Click;
            // 
            // B_ClearCreated
            // 
            B_ClearCreated.Location = new Point(711, 3);
            B_ClearCreated.Name = "B_ClearCreated";
            B_ClearCreated.Size = new Size(152, 32);
            B_ClearCreated.TabIndex = 6;
            B_ClearCreated.Text = "Clear Created";
            B_ClearCreated.UseVisualStyleBackColor = true;
            B_ClearCreated.Click += B_ClearCreated_Click;
            // 
            // B_BB
            // 
            B_BB.Location = new Point(869, 3);
            B_BB.Name = "B_BB";
            B_BB.Size = new Size(40, 32);
            B_BB.TabIndex = 7;
            B_BB.Text = "BB";
            B_BB.UseVisualStyleBackColor = true;
            B_BB.Click += B_BB_Click;
            // 
            // B_OpenCoords
            // 
            B_OpenCoords.Location = new Point(915, 3);
            B_OpenCoords.Name = "B_OpenCoords";
            B_OpenCoords.Size = new Size(87, 32);
            B_OpenCoords.TabIndex = 8;
            B_OpenCoords.Text = "OpenCoord";
            B_OpenCoords.UseVisualStyleBackColor = true;
            B_OpenCoords.Click += B_OpenCoords_Click;
            // 
            // L_Coordinate
            // 
            L_Coordinate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            L_Coordinate.AutoSize = true;
            L_Coordinate.BackColor = Color.Transparent;
            L_Coordinate.Location = new Point(12, 1333);
            L_Coordinate.Name = "L_Coordinate";
            L_Coordinate.Size = new Size(0, 17);
            L_Coordinate.TabIndex = 6;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(1273, 1359);
            Controls.Add(L_Coordinate);
            Controls.Add(FLP_Alternate);
            Controls.Add(NUD_Tolerance);
            Controls.Add(PB_Image);
            Margin = new Padding(2);
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Misty Mark Visualizer";
            ((System.ComponentModel.ISupportInitialize)PB_Image).EndInit();
            ((System.ComponentModel.ISupportInitialize)NUD_Tolerance).EndInit();
            FLP_Alternate.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button B_ExportIntersections;
        private FlowLayoutPanel FLP_Alternate;
        private ComboBox CB_Coordinates2;
        private Label L_Coordinate;
        private Button B_ExportCreated;
        private Button B_ClearCreated;
        private Button B_BB;
        private Button B_OpenCoords;
        private ComboBox CB_Coordinates1;
    }
}
