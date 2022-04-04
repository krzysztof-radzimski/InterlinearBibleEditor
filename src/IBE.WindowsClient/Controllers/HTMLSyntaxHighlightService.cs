using DevExpress.CodeParser;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Services;
using System.Collections.Generic;
using System.Drawing;

namespace IBE.WindowsClient.Controllers {
    public class HTMLSyntaxHighlightService : ISyntaxHighlightService {
        readonly RichEditControl syntaxEditor;

        Dictionary<TokenCategory, SyntaxHighlightProperties> TokensMapping = new Dictionary<TokenCategory, SyntaxHighlightProperties>();

        SyntaxHighlightProperties textProperties;

        void AddTokensMapping(TokenCategory category, Color foreColor) {
            SyntaxHighlightProperties tokenProperties = new SyntaxHighlightProperties();
            tokenProperties.ForeColor = foreColor;
            TokensMapping.Add(category, tokenProperties);
        }

        SyntaxHighlightProperties GetTokensMapping(TokenCategory category) {
            if (TokensMapping.ContainsKey(category)) return TokensMapping[category];
            else return textProperties;
        }

        public HTMLSyntaxHighlightService(RichEditControl syntaxEditor) {
            this.syntaxEditor = syntaxEditor;
        }

        void HighlightSyntax(TokenCollection tokens) {
            if (TokensMapping.Count == 0) {
                AddTokensMapping(TokenCategory.HtmlAttributeName, Color.Red);
                AddTokensMapping(TokenCategory.HtmlAttributeValue, Color.Blue);
                AddTokensMapping(TokenCategory.HtmlComment, Color.Green);
                AddTokensMapping(TokenCategory.HtmlElementName, Color.Brown);
                AddTokensMapping(TokenCategory.HtmlEntity, Color.Green);
                AddTokensMapping(TokenCategory.HtmlOperator, Color.Blue);
                AddTokensMapping(TokenCategory.HtmlServerSideScript, Color.Gray);
                AddTokensMapping(TokenCategory.HtmlString, Color.Black);
                AddTokensMapping(TokenCategory.HtmlTagDelimiter, Color.Blue);
                AddTokensMapping(TokenCategory.CssComment, Color.Green);
                AddTokensMapping(TokenCategory.CssKeyword, Color.Red);
                AddTokensMapping(TokenCategory.CssPropertyName, Color.Red);
                AddTokensMapping(TokenCategory.CssPropertyValue, Color.Blue);
                AddTokensMapping(TokenCategory.CssSelector, Color.Brown);
                AddTokensMapping(TokenCategory.CssStringValue, Color.Blue);

                textProperties = new SyntaxHighlightProperties();
                textProperties.ForeColor = Color.Black;
            }

            if (tokens == null || tokens.Count == 0)
                return;

            Document document = syntaxEditor.Document;
            List<SyntaxHighlightToken> syntaxTokens = new List<SyntaxHighlightToken>(tokens.Count);
            foreach (Token token in tokens) {
                HighlightCategorizedToken((CategorizedToken)token, syntaxTokens);
            }
            document.ApplySyntaxHighlight(syntaxTokens);
        }

        void HighlightCategorizedToken(CategorizedToken token, List<SyntaxHighlightToken> syntaxTokens) {
            Color backColor = syntaxEditor.ActiveView.BackColor;
            TokenCategory category = token.Category;
            syntaxTokens.Add(SetTokenColor(token, GetTokensMapping(category), backColor));

        }
        SyntaxHighlightToken SetTokenColor(Token token, SyntaxHighlightProperties foreColor, Color backColor) {
            if (syntaxEditor.Document.Paragraphs.Count < token.Range.Start.Line)
                return null;
            int paragraphStart = DocumentHelper.GetParagraphStart(syntaxEditor.Document.Paragraphs[token.Range.Start.Line - 1]);
            int tokenStart = paragraphStart + token.Range.Start.Offset - 1;
            if (token.Range.End.Line != token.Range.Start.Line)
                paragraphStart = DocumentHelper.GetParagraphStart(syntaxEditor.Document.Paragraphs[token.Range.End.Line - 1]);

            int tokenEnd = paragraphStart + token.Range.End.Offset - 1;

            return new SyntaxHighlightToken(tokenStart, tokenEnd - tokenStart, foreColor);
        }

        #region #ISyntaxHighlightServiceMembers
        public void Execute() {
            string newText = syntaxEditor.Text;
            // Use DevExpress.CodeParser to parse text into tokens.
            ITokenCategoryHelper tokenHelper = TokenCategoryHelperFactory.CreateHelper(ParserLanguageID.Html);
            TokenCollection highlightTokens;
            highlightTokens = tokenHelper.GetTokens(newText);
            HighlightSyntax(highlightTokens);
        }

        public void ForceExecute() {
            Execute();
        }
        #endregion #ISyntaxHighlightServiceMembers
    }
}
