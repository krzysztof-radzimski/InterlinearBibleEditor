using DevExpress.XtraEditors;
using IBE.Common.Extensions;
using IBE.Data.Model;
using System;
using System.Windows.Forms;

namespace IBE.WindowsClient.Controls {
    public partial class VerseWordEditorControl : XtraUserControl {
        public VerseWord Word { get; }
        public event EventHandler<StrongCode> StrongClick;
        public event EventHandler<GrammarCode> GrammarCodeClick;
        public VerseWordEditorControl() {
            InitializeComponent();
        }
        public VerseWordEditorControl(VerseWord word) : this() {
            Word = word;

            lblNumberOfVerseWord.DataBindings.Add("Text", word, "NumberOfVerseWord");
            lblGreekWord.DataBindings.Add("Text", word, "SourceWord");
            txtTranslation.DataBindings.Add("Text", word, "Translation");
            lblTransliteration.DataBindings.Add("Text", word, "Transliteration");

            cbCitation.DataBindings.Add("Checked", word, "Citation");
            cbWordOfJesus.DataBindings.Add("Checked", word, "WordOfJesus");

            txtFootnoteText.DataBindings.Add("Text", word, "FootnoteText");

            if (word.GrammarCode.IsNotNull()) {
                lblGrammarCode.DataBindings.Add("Text", word.GrammarCode, "GrammarCodeVariant1");
            }
            if (word.StrongCode.IsNotNull()) {
                lblStrong.DataBindings.Add("Text", word.StrongCode, "Code");
            }
        }

        private void lblStrong_Click(object sender, EventArgs e) {
            if (Word.StrongCode.IsNotNull() && StrongClick.IsNotNull()) {
                StrongClick(this, Word.StrongCode);
            }
        }

        private void lblGrammarCode_Click(object sender, EventArgs e) {
            if (Word.GrammarCode.IsNotNull() && GrammarCodeClick.IsNotNull()) {
                GrammarCodeClick(this, Word.GrammarCode);
            }
        }

        private void lblTransliteration_Click(object sender, EventArgs e) {
            var result = XtraInputBox.Show("Transliteration", "Transliteration", Word.Transliteration);
            if (result != Word.Transliteration && result.IsNotNullOrEmpty()) {
                Word.Transliteration = result;
            }
        }

        private void btnInsertEmptyString_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            Word.Translation = "―";
        }

        private void txtTranslation_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.F12) {
                Word.Translation = "―";
            }
        }
    }
}
