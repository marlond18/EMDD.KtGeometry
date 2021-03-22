namespace TestForm
{
    partial class Form1
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
            this.btnAddPolygon2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbTest)).BeginInit();
            this.SuspendLayout();
            // 
            // pbTest
            // 
            this.pbTest.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pbTest.Location = new System.Drawing.Point(28, 15);
            this.pbTest.Margin = new System.Windows.Forms.Padding(4);
            this.pbTest.Name = "pbTest";
            this.pbTest.Size = new System.Drawing.Size(485, 305);
            this.pbTest.TabIndex = 0;
            this.pbTest.TabStop = false;
            this.pbTest.Click += new System.EventHandler(this.PbTest_Click);
            this.pbTest.Paint += new System.Windows.Forms.PaintEventHandler(this.PbTest_Paint);
            // 
            // btnAddPolygon1
            // 
            this.btnAddPolygon1.Location = new System.Drawing.Point(555, 33);
            this.btnAddPolygon1.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddPolygon1.Name = "btnAddPolygon1";
            this.btnAddPolygon1.Size = new System.Drawing.Size(137, 28);
            this.btnAddPolygon1.TabIndex = 2;
            this.btnAddPolygon1.Text = "Add Polygon 1";
            this.btnAddPolygon1.UseVisualStyleBackColor = true;
            this.btnAddPolygon1.Click += new System.EventHandler(this.BtnAddPolygon1_Click);
            // 
            // btnAddPolygon2
            // 
            this.btnAddPolygon2.Location = new System.Drawing.Point(555, 69);
            this.btnAddPolygon2.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddPolygon2.Name = "btnAddPolygon2";
            this.btnAddPolygon2.Size = new System.Drawing.Size(137, 28);
            this.btnAddPolygon2.TabIndex = 2;
            this.btnAddPolygon2.Text = "Add Polygon 2";
            this.btnAddPolygon2.UseVisualStyleBackColor = true;
            this.btnAddPolygon2.Click += new System.EventHandler(this.BtnAddPolygon2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(555, 105);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(137, 28);
            this.button1.TabIndex = 2;
            this.button1.Text = "CreateInterSection";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(555, 190);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(148, 147);
            this.textBox1.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(555, 141);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(137, 28);
            this.button2.TabIndex = 4;
            this.button2.Text = "Clear";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(454, 384);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(137, 28);
            this.button3.TabIndex = 2;
            this.button3.Text = "CreateInterSection";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 458);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnAddPolygon2);
            this.Controls.Add(this.btnAddPolygon1);
            this.Controls.Add(this.pbTest);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbTest)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbTest;
        private System.Windows.Forms.Button btnAddPolygon1;
        private System.Windows.Forms.Button btnAddPolygon2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}

