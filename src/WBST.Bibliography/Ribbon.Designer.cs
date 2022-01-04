namespace WBST.Bibliography {
    partial class Ribbon : Microsoft.Office.Tools.Ribbon.RibbonBase {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Ribbon()
            : base(Globals.Factory.GetRibbonFactory()) {
            InitializeComponent();
        }

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.tab1 = this.Factory.CreateRibbonTab();
            this.WbstBibliographyGroup = this.Factory.CreateRibbonGroup();
            this.btnShowPane = this.Factory.CreateRibbonToggleButton();
            this.separator1 = this.Factory.CreateRibbonSeparator();
            this.btnApplyFormatting = this.Factory.CreateRibbonButton();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.tab1.SuspendLayout();
            this.WbstBibliographyGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.WbstBibliographyGroup);
            this.tab1.Label = "TabAddIns";
            this.tab1.Name = "tab1";
            // 
            // WbstBibliographyGroup
            // 
            this.WbstBibliographyGroup.Items.Add(this.btnShowPane);
            this.WbstBibliographyGroup.Items.Add(this.separator1);
            this.WbstBibliographyGroup.Items.Add(this.btnApplyFormatting);
            this.WbstBibliographyGroup.Label = "WBST";
            this.WbstBibliographyGroup.Name = "WbstBibliographyGroup";
            // 
            // btnShowPane
            // 
            this.btnShowPane.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnShowPane.Label = "Pokaż listę";
            this.btnShowPane.Name = "btnShowPane";
            this.btnShowPane.OfficeImageId = "BibliographyStyle";
            this.btnShowPane.ShowImage = true;
            this.btnShowPane.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnShowPane_Click);
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            // 
            // btnApplyFormatting
            // 
            this.btnApplyFormatting.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnApplyFormatting.Label = "Zastosuj formatowanie";
            this.btnApplyFormatting.Name = "btnApplyFormatting";
            this.btnApplyFormatting.OfficeImageId = "ApplyStylesPane";
            this.btnApplyFormatting.ShowImage = true;
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Office 2019 Black";
            // 
            // Ribbon
            // 
            this.Name = "Ribbon";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.WbstBibliographyGroup.ResumeLayout(false);
            this.WbstBibliographyGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup WbstBibliographyGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton btnShowPane;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnApplyFormatting;
    }

    partial class ThisRibbonCollection {
        internal Ribbon Ribbon {
            get { return this.GetRibbon<Ribbon>(); }
        }
    }
}
