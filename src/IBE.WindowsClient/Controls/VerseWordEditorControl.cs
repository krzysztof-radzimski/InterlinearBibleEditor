using DevExpress.Xpo;
using DevExpress.XtraEditors;
using IBE.Common.Extensions;
using IBE.Data.Import.Greek;
using IBE.Data.Model;
using System;
using System.Linq;
using System.Windows.Forms;

namespace IBE.WindowsClient.Controls {
    public partial class VerseWordEditorControl : XtraUserControl {
        private bool changed = false;

        public bool Modified { get { return changed; } }
        public VerseWord Word { get; }
        public event EventHandler<StrongCode> StrongClick;
        public event EventHandler<GrammarCode> GrammarCodeClick;
        public event EventHandler<VerseWord> DeleteClick;
        public VerseWordEditorControl() {
            InitializeComponent();
        }
        public VerseWordEditorControl(VerseWord word) : this() {
            Word = word;
            Word.Changed += Word_Changed;

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

        private void Word_Changed(object sender, DevExpress.Xpo.ObjectChangeEventArgs e) {
            if (e.NewValue != e.OldValue) { changed = true; }
        }

        private void lblStrong_Click(object sender, EventArgs e) {
            if (Word.StrongCode.IsNotNull() && StrongClick.IsNotNull()) {
                StrongClick(this, Word.StrongCode);
            }
            else {
                var strongCode = XtraInputBox.Show("Insert Strong's code:", "Strong Codes", "");
                if (strongCode.IsNotNullOrEmpty()) {
                    var sc = new XPQuery<StrongCode>(Word.Session).Where(x => x.Code == strongCode.ToInt() && x.Lang == Language.Greek).FirstOrDefault();
                    if (sc.IsNotNull()) {
                        Word.StrongCode = sc;
                        (Word.Session as UnitOfWork).CommitChanges();

                        lblStrong.DataBindings.Add("Text", Word.StrongCode, "Code");
                    }
                }
            }
        }

        private void lblGrammarCode_Click(object sender, EventArgs e) {
            if (Word.GrammarCode.IsNotNull() && GrammarCodeClick.IsNotNull()) {
                GrammarCodeClick(this, Word.GrammarCode);
            }
            else {
                var grammarCode = XtraInputBox.Show("Insert grammar code:", "Grammar Code", "");
                if (grammarCode.IsNotNullOrEmpty()) {
                    var gc = new XPQuery<GrammarCode>(Word.Session).Where(x => x.GrammarCodeVariant1 == grammarCode).FirstOrDefault();
                    if (gc.IsNotNull()) {
                        Word.GrammarCode = gc;
                        (Word.Session as UnitOfWork).CommitChanges();

                        lblGrammarCode.DataBindings.Add("Text", Word.GrammarCode, "GrammarCodeVariant1");
                    }
                }
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

        private void btnDelete_Click(object sender, EventArgs e) {
            if (XtraMessageBox.Show("Do you want to delete this word?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                if (DeleteClick.IsNotNull()) {
                    DeleteClick(this, Word);
                }
            }
        }

        private void lblGreekWord_DoubleClick(object sender, EventArgs e) {
            var sourceWord = XtraInputBox.Show("Insert source word:", "Source Word", Word.SourceWord);
            if (sourceWord.IsNotNullOrEmpty()) {
                Word.SourceWord = sourceWord;
                Word.Transliteration = sourceWord.TransliterateAncientGreek();
            }
        }

        private void lblNumberOfVerseWord_DoubleClick(object sender, EventArgs e) {
            var index = XtraInputBox.Show("Set word index:", "Word Index", Word.NumberOfVerseWord);
            if (index != 0) {
                Word.NumberOfVerseWord = index;
            }
        }

        private void lblStrong_DoubleClick(object sender, EventArgs e) {
            if (Word.StrongCode.IsNotNull()) {
                var strongCode = XtraInputBox.Show("Insert Strong's code:", "Strong Codes", lblStrong.Text);
                if (strongCode.IsNotNullOrEmpty()) {
                    var sc = new XPQuery<StrongCode>(Word.Session).Where(x => x.Code == strongCode.ToInt() && x.Lang == Language.Greek).FirstOrDefault();
                    if (sc.IsNotNull()) {
                        Word.StrongCode = sc;
                        (Word.Session as UnitOfWork).CommitChanges();
                    }
                }
            }
        }

        private void lblGrammarCode_DoubleClick(object sender, EventArgs e) {
            if (Word.GrammarCode.IsNotNull()) {
                var grammarCode = XtraInputBox.Show("Insert grammar's code:", "Grammar Codes", lblGrammarCode.Text);
                if (grammarCode.IsNotNullOrEmpty()) {
                    var gc = new XPQuery<GrammarCode>(Word.Session).Where(x => x.GrammarCodeVariant1 == grammarCode).FirstOrDefault();
                    if (gc.IsNotNull()) {
                        Word.GrammarCode = gc;
                        (Word.Session as UnitOfWork).CommitChanges();
                    }
                }
            }
        }
    }
}
