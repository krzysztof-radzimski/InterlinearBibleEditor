namespace IBE.Ukrainian.UI {
    public partial class MainForm : Form {
        Controllers.UkrainianTransliterationController UkrainianTransliteration { get; }
        public MainForm() {
            InitializeComponent();
            
            UkrainianTransliteration = new Controllers.UkrainianTransliterationController();
        }

        private void txtUA_TextChanged(object sender, EventArgs e) {
           txtLatin.Text = UkrainianTransliteration.ToLatin(txtUA.Text);
        }
    }
}