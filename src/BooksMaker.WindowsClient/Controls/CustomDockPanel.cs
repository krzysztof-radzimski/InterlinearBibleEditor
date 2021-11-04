using DevExpress.XtraBars.Docking;

namespace BooksMaker.WindowsClient.Controls {
    public class CustomDockPanel : DockPanel {
        public CustomDockPanel()
            : this(false, DockingStyle.Float, null) {
        }
        public CustomDockPanel(bool createControlContainer, DockingStyle dock, DockManager dockManager)
            : base(createControlContainer, dock, dockManager) {
            DockLayout = new CustomDockLayout(dock, this);
        }

        //// Fields...

        private bool _ShowCaption = true;

        public bool ShowCaption {
            get { return _ShowCaption; }
            set {
                if (_ShowCaption != value) {
                    _ShowCaption = value;
                    DockLayout.LayoutChanged();
                }
            }
        }

        internal new CustomDockLayout DockLayout {
            get { return base.DockLayout as CustomDockLayout; }
            set { base.DockLayout = value; }
        }
    }
}
