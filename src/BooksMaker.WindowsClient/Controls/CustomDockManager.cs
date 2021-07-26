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
    public class CustomDockManager : DockManager {
        public CustomDockManager() : base() { }
        public CustomDockManager(ContainerControl form) : base(form) { }
        public CustomDockManager(IContainer container) : base(container) { }
        protected override DockPanel CreateDockPanel(DockingStyle dock, bool createControlContainer) {
            return new CustomDockPanel(createControlContainer, dock, this);
        }
    }
}
