
namespace SvgToIconConverter {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.svgImageBox = new DevExpress.XtraEditors.SvgImageBox();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.sizes = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.btnConvertToIcon = new DevExpress.XtraEditors.SimpleButton();
            this.btnOpenSvg = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.svgImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizes)).BeginInit();
            this.SuspendLayout();
            // 
            // svgImageBox
            // 
            this.svgImageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.svgImageBox.Location = new System.Drawing.Point(2, 23);
            this.svgImageBox.Name = "svgImageBox";
            this.svgImageBox.Size = new System.Drawing.Size(508, 487);
            this.svgImageBox.SizeMode = DevExpress.XtraEditors.SvgImageSizeMode.Zoom;
            this.svgImageBox.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.Full;
            this.svgImageBox.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.svgImageBox);
            this.groupControl1.Location = new System.Drawing.Point(12, 12);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(512, 512);
            this.groupControl1.TabIndex = 1;
            this.groupControl1.Text = "SVG Image";
            // 
            // sizes
            // 
            this.sizes.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("256", "256x256", System.Windows.Forms.CheckState.Checked),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("128", "128x128", System.Windows.Forms.CheckState.Checked),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("64", "64x64", System.Windows.Forms.CheckState.Checked),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("48", "48x48", System.Windows.Forms.CheckState.Checked),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("32", "32x32", System.Windows.Forms.CheckState.Checked),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("24", "24x24", System.Windows.Forms.CheckState.Checked),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem("16", "16x16", System.Windows.Forms.CheckState.Checked)});
            this.sizes.Location = new System.Drawing.Point(530, 12);
            this.sizes.Name = "sizes";
            this.sizes.Size = new System.Drawing.Size(258, 167);
            this.sizes.TabIndex = 2;
            // 
            // btnConvertToIcon
            // 
            this.btnConvertToIcon.Enabled = false;
            this.btnConvertToIcon.Location = new System.Drawing.Point(692, 185);
            this.btnConvertToIcon.Name = "btnConvertToIcon";
            this.btnConvertToIcon.Size = new System.Drawing.Size(96, 23);
            this.btnConvertToIcon.TabIndex = 3;
            this.btnConvertToIcon.Text = "Convert to Icon";
            this.btnConvertToIcon.Click += new System.EventHandler(this.btnConvertToIcon_Click);
            // 
            // btnOpenSvg
            // 
            this.btnOpenSvg.Location = new System.Drawing.Point(530, 185);
            this.btnOpenSvg.Name = "btnOpenSvg";
            this.btnOpenSvg.Size = new System.Drawing.Size(75, 23);
            this.btnOpenSvg.TabIndex = 4;
            this.btnOpenSvg.Text = "Open SVG file";
            this.btnOpenSvg.Click += new System.EventHandler(this.btnOpenSvg_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 548);
            this.Controls.Add(this.btnOpenSvg);
            this.Controls.Add(this.btnConvertToIcon);
            this.Controls.Add(this.sizes);
            this.Controls.Add(this.groupControl1);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SVG to Icon Converter";
            ((System.ComponentModel.ISupportInitialize)(this.svgImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sizes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SvgImageBox svgImageBox;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.CheckedListBoxControl sizes;
        private DevExpress.XtraEditors.SimpleButton btnConvertToIcon;
        private DevExpress.XtraEditors.SimpleButton btnOpenSvg;
    }
}

