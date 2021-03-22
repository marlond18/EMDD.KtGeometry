namespace TestForm
{
    partial class PolygonTest
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
            this.pbTest = new System.Windows.Forms.PictureBox();
            this.btnAddPolygon1 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnExecuteCombination = new System.Windows.Forms.Button();
            this.cbTypOfCombination = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbTest)).BeginInit();
            this.SuspendLayout();
            // 
            // pbTest
            // 
            this.pbTest.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pbTest.Location = new System.Drawing.Point(41, 34);
            this.pbTest.Margin = new System.Windows.Forms.Padding(4);
            this.pbTest.Name = "pbTest";
            this.pbTest.Size = new System.Drawing.Size(804, 513);
            this.pbTest.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbTest.TabIndex = 1;
            this.pbTest.TabStop = false;
            this.pbTest.Paint += new System.Windows.Forms.PaintEventHandler(this.PbTest_Paint);
            // 
            // btnAddPolygon1
            // 
            this.btnAddPolygon1.Location = new System.Drawing.Point(853, 34);
            this.btnAddPolygon1.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddPolygon1.Name = "btnAddPolygon1";
            this.btnAddPolygon1.Size = new System.Drawing.Size(171, 28);
            this.btnAddPolygon1.TabIndex = 3;
            this.btnAddPolygon1.Text = "Add region to polygon 1";
            this.btnAddPolygon1.UseVisualStyleBackColor = true;
            this.btnAddPolygon1.Click += new System.EventHandler(this.BtnAddPolygon1_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(853, 106);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(170, 28);
            this.button1.TabIndex = 3;
            this.button1.Text = "Extract Text";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(853, 70);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(171, 28);
            this.button2.TabIndex = 3;
            this.button2.Text = "Add region to polygon 2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.BtnAddPolygon2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(852, 141);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(171, 294);
            this.textBox1.TabIndex = 4;
            // 
            // btnExecuteCombination
            // 
            this.btnExecuteCombination.Location = new System.Drawing.Point(852, 471);
            this.btnExecuteCombination.Margin = new System.Windows.Forms.Padding(4);
            this.btnExecuteCombination.Name = "btnExecuteCombination";
            this.btnExecuteCombination.Size = new System.Drawing.Size(172, 28);
            this.btnExecuteCombination.TabIndex = 3;
            this.btnExecuteCombination.Text = "Combine";
            this.btnExecuteCombination.UseVisualStyleBackColor = true;
            this.btnExecuteCombination.Click += new System.EventHandler(this.ExecuteCombination_Click);
            // 
            // cbTypOfCombination
            // 
            this.cbTypOfCombination.FormattingEnabled = true;
            this.cbTypOfCombination.Items.AddRange(new object[] {
            "Union",
            "Difference",
            "Intersect",
            "Xor"});
            this.cbTypOfCombination.Location = new System.Drawing.Point(853, 441);
            this.cbTypOfCombination.Name = "cbTypOfCombination";
            this.cbTypOfCombination.Size = new System.Drawing.Size(171, 24);
            this.cbTypOfCombination.TabIndex = 5;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(853, 507);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(170, 27);
            this.button3.TabIndex = 6;
            this.button3.Text = "Extract Combination Op";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // PolygonTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1073, 566);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.cbTypOfCombination);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnExecuteCombination);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnAddPolygon1);
            this.Controls.Add(this.pbTest);
            this.Name = "PolygonTest";
            this.Text = "PolygonTest";
            ((System.ComponentModel.ISupportInitialize)(this.pbTest)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbTest;
        private System.Windows.Forms.Button btnAddPolygon1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnExecuteCombination;
        private System.Windows.Forms.ComboBox cbTypOfCombination;
        private System.Windows.Forms.Button button3;
    }
}