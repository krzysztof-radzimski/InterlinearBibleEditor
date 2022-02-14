namespace IBE.WindowsClient {
    partial class SongEditorForm {
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
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.comboBoxEdit1 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.spinEdit1 = new DevExpress.XtraEditors.SpinEdit();
            this.spinEdit2 = new DevExpress.XtraEditors.SpinEdit();
            this.textEdit2 = new DevExpress.XtraEditors.TextEdit();
            this.comboBoxEdit2 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 5F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 55F)});
            this.tablePanel1.Controls.Add(this.comboBoxEdit2);
            this.tablePanel1.Controls.Add(this.textEdit2);
            this.tablePanel1.Controls.Add(this.spinEdit2);
            this.tablePanel1.Controls.Add(this.spinEdit1);
            this.tablePanel1.Controls.Add(this.comboBoxEdit1);
            this.tablePanel1.Controls.Add(this.textEdit1);
            this.tablePanel1.Controls.Add(this.labelControl6);
            this.tablePanel1.Controls.Add(this.labelControl5);
            this.tablePanel1.Controls.Add(this.labelControl4);
            this.tablePanel1.Controls.Add(this.labelControl3);
            this.tablePanel1.Controls.Add(this.labelControl2);
            this.tablePanel1.Controls.Add(this.labelControl1);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tablePanel1.Location = new System.Drawing.Point(0, 0);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(800, 176);
            this.tablePanel1.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.tablePanel1.SetColumn(this.labelControl1, 0);
            this.labelControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelControl1.Location = new System.Drawing.Point(42, 3);
            this.labelControl1.Name = "labelControl1";
            this.tablePanel1.SetRow(this.labelControl1, 0);
            this.labelControl1.Size = new System.Drawing.Size(32, 20);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Name:";
            // 
            // labelControl2
            // 
            this.tablePanel1.SetColumn(this.labelControl2, 0);
            this.labelControl2.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelControl2.Location = new System.Drawing.Point(21, 29);
            this.labelControl2.Name = "labelControl2";
            this.tablePanel1.SetRow(this.labelControl2, 1);
            this.labelControl2.Size = new System.Drawing.Size(53, 20);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Signature:";
            // 
            // labelControl3
            // 
            this.tablePanel1.SetColumn(this.labelControl3, 0);
            this.labelControl3.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelControl3.Location = new System.Drawing.Point(49, 55);
            this.labelControl3.Name = "labelControl3";
            this.tablePanel1.SetRow(this.labelControl3, 2);
            this.labelControl3.Size = new System.Drawing.Size(25, 20);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "BPM:";
            // 
            // labelControl4
            // 
            this.tablePanel1.SetColumn(this.labelControl4, 0);
            this.labelControl4.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelControl4.Location = new System.Drawing.Point(30, 81);
            this.labelControl4.Name = "labelControl4";
            this.tablePanel1.SetRow(this.labelControl4, 3);
            this.labelControl4.Size = new System.Drawing.Size(44, 20);
            this.labelControl4.TabIndex = 3;
            this.labelControl4.Text = "Number:";
            // 
            // labelControl5
            // 
            this.tablePanel1.SetColumn(this.labelControl5, 0);
            this.labelControl5.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelControl5.Location = new System.Drawing.Point(3, 107);
            this.labelControl5.Name = "labelControl5";
            this.tablePanel1.SetRow(this.labelControl5, 4);
            this.labelControl5.Size = new System.Drawing.Size(71, 20);
            this.labelControl5.TabIndex = 4;
            this.labelControl5.Text = "YouTube URL:";
            // 
            // labelControl6
            // 
            this.tablePanel1.SetColumn(this.labelControl6, 0);
            this.labelControl6.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelControl6.Location = new System.Drawing.Point(47, 133);
            this.labelControl6.Name = "labelControl6";
            this.tablePanel1.SetRow(this.labelControl6, 5);
            this.labelControl6.Size = new System.Drawing.Size(27, 20);
            this.labelControl6.TabIndex = 5;
            this.labelControl6.Text = "Type:";
            // 
            // textEdit1
            // 
            this.tablePanel1.SetColumn(this.textEdit1, 1);
            this.textEdit1.Location = new System.Drawing.Point(80, 3);
            this.textEdit1.Name = "textEdit1";
            this.tablePanel1.SetRow(this.textEdit1, 0);
            this.textEdit1.Size = new System.Drawing.Size(717, 20);
            this.textEdit1.TabIndex = 6;
            // 
            // comboBoxEdit1
            // 
            this.tablePanel1.SetColumn(this.comboBoxEdit1, 1);
            this.comboBoxEdit1.Location = new System.Drawing.Point(80, 29);
            this.comboBoxEdit1.Name = "comboBoxEdit1";
            this.comboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit1.Properties.Items.AddRange(new object[] {
            "4/4",
            "3/4",
            "6/8",
            "12/8"});
            this.comboBoxEdit1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.tablePanel1.SetRow(this.comboBoxEdit1, 1);
            this.comboBoxEdit1.Size = new System.Drawing.Size(717, 20);
            this.comboBoxEdit1.TabIndex = 7;
            // 
            // spinEdit1
            // 
            this.tablePanel1.SetColumn(this.spinEdit1, 1);
            this.spinEdit1.EditValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.spinEdit1.Location = new System.Drawing.Point(80, 55);
            this.spinEdit1.Name = "spinEdit1";
            this.spinEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit1.Properties.MaxValue = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.spinEdit1.Properties.MinValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.spinEdit1.Properties.SpinStyle = DevExpress.XtraEditors.Controls.SpinStyles.Horizontal;
            this.tablePanel1.SetRow(this.spinEdit1, 2);
            this.spinEdit1.Size = new System.Drawing.Size(717, 20);
            this.spinEdit1.TabIndex = 8;
            // 
            // spinEdit2
            // 
            this.tablePanel1.SetColumn(this.spinEdit2, 1);
            this.spinEdit2.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinEdit2.Location = new System.Drawing.Point(80, 81);
            this.spinEdit2.Name = "spinEdit2";
            this.spinEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit2.Properties.MaxValue = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.spinEdit2.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinEdit2.Properties.SpinStyle = DevExpress.XtraEditors.Controls.SpinStyles.Horizontal;
            this.tablePanel1.SetRow(this.spinEdit2, 3);
            this.spinEdit2.Size = new System.Drawing.Size(717, 20);
            this.spinEdit2.TabIndex = 9;
            // 
            // textEdit2
            // 
            this.tablePanel1.SetColumn(this.textEdit2, 1);
            this.textEdit2.Location = new System.Drawing.Point(80, 107);
            this.textEdit2.Name = "textEdit2";
            this.tablePanel1.SetRow(this.textEdit2, 4);
            this.textEdit2.Size = new System.Drawing.Size(717, 20);
            this.textEdit2.TabIndex = 10;
            // 
            // comboBoxEdit2
            // 
            this.tablePanel1.SetColumn(this.comboBoxEdit2, 1);
            this.comboBoxEdit2.Location = new System.Drawing.Point(80, 133);
            this.comboBoxEdit2.Name = "comboBoxEdit2";
            this.comboBoxEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit2.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.tablePanel1.SetRow(this.comboBoxEdit2, 5);
            this.comboBoxEdit2.Size = new System.Drawing.Size(717, 20);
            this.comboBoxEdit2.TabIndex = 11;
            // 
            // grid
            // 
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(0, 176);
            this.grid.MainView = this.gridView;
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(800, 470);
            this.grid.TabIndex = 1;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.GridControl = this.grid;
            this.gridView.Name = "gridView";
            this.gridView.OptionsView.ColumnAutoWidth = false;
            this.gridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            this.gridView.OptionsView.ShowGroupPanel = false;
            // 
            // SongEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 646);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.tablePanel1);
            this.Name = "SongEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Song Editor";
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            this.tablePanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.SpinEdit spinEdit1;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit1;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit2;
        private DevExpress.XtraEditors.TextEdit textEdit2;
        private DevExpress.XtraEditors.SpinEdit spinEdit2;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
    }
}