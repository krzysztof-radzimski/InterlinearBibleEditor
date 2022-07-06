using DevExpress.Xpo;
using DevExpress.XtraEditors;
using ChurchServices.Extensions;
using ChurchServices.Data.Model;
using ChurchServices.WinApp.Controllers;
using System.Text;
using System.Text.RegularExpressions;

namespace ChurchServices.WinApp {
    public partial class StrongsCodeEditForm : XtraForm {
        public StrongCode Code { get; private set; }
        public StrongsCodeEditForm() {
            InitializeComponent();
            XHtmlDocumentExporter.Register(richEditControl);
            richEditControl.SpellChecker = MainForm.Instance.SpellChecker;
        }

        public void LoadData(StrongCode strongCode) {
            Code = strongCode;
            txtCode.Text = strongCode.Code.ToString();
            txtLanguage.SelectedIndex = strongCode.Lang == Language.Greek ? 0 : 1;
            txtPronunciation.Text = strongCode.Pronunciation;
            txtSourceWord.Text = strongCode.SourceWord;
            txtShortDefinition.Text = strongCode.ShortDefinition;
            txtTransliteration.Text = strongCode.Transliteration;
            richEditControl.HtmlText = strongCode.Definition;
        }

        public void SaveData() {
            if (Code.IsNull()) { Code = new StrongCode(new UnitOfWork()); }
            if (Code.IsNotNull()) {
                Code.Transliteration = txtTransliteration.Text;
                Code.SourceWord = txtSourceWord.Text;
                Code.Code = txtCode.Text.ToInt();
                Code.Lang = txtLanguage.SelectedIndex == 0 ? Language.Greek : Language.Hebrew;
                Code.Pronunciation = txtPronunciation.Text;
                Code.ShortDefinition = txtShortDefinition.Text;

                var data = richEditControl.SaveDocument(XHtmlDocumentFormat.Id);
                var html = Encoding.UTF8.GetString(data);
                html = UpdateHtml(html);
                Code.Definition = html;
                (Code.Session as UnitOfWork).CommitChanges();
            }
        }

        private string UpdateHtml(string html) {
            var patternFontSize = @"font\-size\:\s?[0-9]+px;";
            html = Regex.Replace(html, patternFontSize, string.Empty);

            var patternEmptyStyles = @"\sstyle=""(\s+)?""";
            html = Regex.Replace(html, patternEmptyStyles, string.Empty);

            return html;
        }
    }
}
