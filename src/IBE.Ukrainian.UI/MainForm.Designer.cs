namespace IBE.Ukrainian.UI {
    partial class MainForm {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtUA = new System.Windows.Forms.TextBox();
            this.txtLatin = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtUA);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtLatin);
            this.splitContainer1.Size = new System.Drawing.Size(1156, 818);
            this.splitContainer1.SplitterDistance = 385;
            this.splitContainer1.TabIndex = 0;
            // 
            // txtUA
            // 
            this.txtUA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUA.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtUA.Location = new System.Drawing.Point(0, 0);
            this.txtUA.Multiline = true;
            this.txtUA.Name = "txtUA";
            this.txtUA.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtUA.Size = new System.Drawing.Size(385, 818);
            this.txtUA.TabIndex = 0;
            this.txtUA.TextChanged += new System.EventHandler(this.txtUA_TextChanged);
            // 
            // txtLatin
            // 
            this.txtLatin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLatin.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtLatin.Location = new System.Drawing.Point(0, 0);
            this.txtLatin.Multiline = true;
            this.txtLatin.Name = "txtLatin";
            this.txtLatin.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLatin.Size = new System.Drawing.Size(767, 818);
            this.txtLatin.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1156, 818);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Transliterate Ukrainian Text To Latin";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SplitContainer splitContainer1;
        private TextBox txtUA;
        private TextBox txtLatin;
    }
}