using DevExpress.XtraBars.Docking;
using System.ComponentModel;
using System.Windows.Forms;

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
