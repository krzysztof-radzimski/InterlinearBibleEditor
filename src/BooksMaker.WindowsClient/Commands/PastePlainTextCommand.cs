using DevExpress.Utils.Commands;
using DevExpress.XtraBars;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Commands;
using System;
using System.Windows.Forms;

namespace BooksMaker.WindowsClient.Commands {
    public class PastePlainTextCommand : PasteSelectionCommand {
        public PastePlainTextCommand(IRichEditControl control) : base(control) { }
        public override void ForceExecute(ICommandUIState state) {
            //base.ForceExecute(state);
            var text = Clipboard.GetText();
            if (text != null) {
                Control.Document.InsertText(Control.Document.CaretPosition, text);
            }
        }
    }

    public class PastePlainTextBarButtonItem : BarButtonItem {
        RichEditControl control;

        public void Initialize(RichEditControl initControl) {
            UnsubscribeControlEvents();
            control = initControl;
            SubscribeControlEvents();
        }

        public RichEditControl RichEditControl {
            get { return control; }
            set {
                if (control == value)
                    return;
                UnsubscribeControlEvents();
                this.control = value;
                SubscribeControlEvents();
                OnUpdateUI(this, EventArgs.Empty);
            }
        }

        void SubscribeControlEvents() {
            if (control == null)
                return;
            control.UpdateUI += OnUpdateUI;
        }
        void UnsubscribeControlEvents() {
            if (control == null)
                return;
            control.UpdateUI -= OnUpdateUI;
        }
        void OnUpdateUI(object sender, EventArgs e) {
            Command command = CreateCommand();
            if (command != null) {
                CommandButtonUIState state = new CommandButtonUIState(this);
                command.UpdateUIState(state);
            }
        }

        protected virtual Command CreateCommand() {
            return new PastePlainTextCommand(control);
        }
    }

    public class CommandButtonUIState : ICommandUIState {
        readonly PastePlainTextBarButtonItem button;
        public CommandButtonUIState(PastePlainTextBarButtonItem button) {
            this.button = button;
        }
       
        public bool Checked { get { return button.Down; } set { button.Down = value; } }
        public bool Enabled { get { return button.Enabled; } set { button.Enabled = value; } }
        public bool Visible { get { return button.Visibility == BarItemVisibility.Always; } set { button.Visibility = value ? BarItemVisibility.Always : BarItemVisibility.Never; } }
        public virtual object EditValue { get { return null; } set { } }
    }
}
