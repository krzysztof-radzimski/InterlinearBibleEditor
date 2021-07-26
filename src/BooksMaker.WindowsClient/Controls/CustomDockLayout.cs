using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraBars.Docking.Helpers;

namespace BooksMaker.WindowsClient.Controls {
    public class CustomDockLayout : DockLayout {
        public CustomDockLayout(DockingStyle dock, DockPanel panel) : base(dock, panel) { }
        public CustomDockLayout(DockingStyle dock, Size size, DockPanel panel) : base(dock, size, panel) { }
        protected override bool HasCaption {
            get {
                bool hasCaption = base.HasCaption;
                CustomDockPanel dockPanel = Panel as CustomDockPanel;
                return dockPanel.ShowCaption && hasCaption;
                // return base.HasCaption;
            }
        }

        public new CustomDockManager DockManager {
            get { return base.DockManager as CustomDockManager; }
        }

        internal new void LayoutChanged() {
            base.LayoutChanged();
        }
    }
}
