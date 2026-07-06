namespace COM_TestForm
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.testPage_btn = new System.Windows.Forms.Button();
            this.jpegSettings_btn = new System.Windows.Forms.Button();
            this.mergedFiles_btn = new System.Windows.Forms.Button();
            this.coverPage_btn = new System.Windows.Forms.Button();
            this.backgroundPage_btn = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // testPage_btn
            // 
            this.testPage_btn.Location = new System.Drawing.Point(90, 11);
            this.testPage_btn.Name = "testPage_btn";
            this.testPage_btn.Size = new System.Drawing.Size(150, 48);
            this.testPage_btn.TabIndex = 0;
            this.testPage_btn.Text = "TestPage2Pdf";
            this.testPage_btn.UseVisualStyleBackColor = true;
            this.testPage_btn.Click += new System.EventHandler(this.testPage_btn_Click);
            // 
            // jpegSettings_btn
            // 
            this.jpegSettings_btn.Location = new System.Drawing.Point(246, 11);
            this.jpegSettings_btn.Name = "jpegSettings_btn";
            this.jpegSettings_btn.Size = new System.Drawing.Size(154, 49);
            this.jpegSettings_btn.TabIndex = 1;
            this.jpegSettings_btn.Text = "TestPage2Jpeg";
            this.jpegSettings_btn.UseVisualStyleBackColor = true;
            this.jpegSettings_btn.Click += new System.EventHandler(this.jpegSettings_btn_Click);
            // 
            // mergedFiles_btn
            // 
            this.mergedFiles_btn.Location = new System.Drawing.Point(12, 65);
            this.mergedFiles_btn.Name = "mergedFiles_btn";
            this.mergedFiles_btn.Size = new System.Drawing.Size(150, 48);
            this.mergedFiles_btn.TabIndex = 2;
            this.mergedFiles_btn.Text = "MergedMultipleFiles2Tif";
            this.mergedFiles_btn.UseVisualStyleBackColor = true;
            this.mergedFiles_btn.Click += new System.EventHandler(this.mergedFiles_btn_Click);
            // 
            // coverPage_btn
            // 
            this.coverPage_btn.Location = new System.Drawing.Point(168, 65);
            this.coverPage_btn.Name = "coverPage_btn";
            this.coverPage_btn.Size = new System.Drawing.Size(150, 48);
            this.coverPage_btn.TabIndex = 3;
            this.coverPage_btn.Text = "CoverPage";
            this.coverPage_btn.UseVisualStyleBackColor = true;
            this.coverPage_btn.Click += new System.EventHandler(this.coverPage_btn_Click);
            // 
            // backgroundPage_btn
            // 
            this.backgroundPage_btn.Location = new System.Drawing.Point(324, 65);
            this.backgroundPage_btn.Name = "backgroundPage_btn";
            this.backgroundPage_btn.Size = new System.Drawing.Size(151, 48);
            this.backgroundPage_btn.TabIndex = 4;
            this.backgroundPage_btn.Text = "BackgroundPage";
            this.backgroundPage_btn.UseVisualStyleBackColor = true;
            this.backgroundPage_btn.Click += new System.EventHandler(this.backgroundPage_btn_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 118);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(492, 20);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.Resize += new System.EventHandler(this.statusStrip1_Resize);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 15);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 138);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.backgroundPage_btn);
            this.Controls.Add(this.coverPage_btn);
            this.Controls.Add(this.mergedFiles_btn);
            this.Controls.Add(this.jpegSettings_btn);
            this.Controls.Add(this.testPage_btn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button testPage_btn;
        private System.Windows.Forms.Button jpegSettings_btn;
        private System.Windows.Forms.Button mergedFiles_btn;
        private System.Windows.Forms.Button coverPage_btn;
        private System.Windows.Forms.Button backgroundPage_btn;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}

