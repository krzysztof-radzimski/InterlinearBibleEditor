using DevExpress.XtraEditors;
using IBE.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IBE.WindowsClient {
    public partial class RecognizeChapterContentForm : XtraForm {
        public RecognizeChapterContentForm() {
            InitializeComponent();
        }

        /// <summary>
        /// 1. I zostały ukończone niebo i ziemia i wszystkie ich możliwości. 
        /// 2. Ukończył Bóg na siódmy dzień Swoje czynności, które wykonywał. 
        /// I zaprzestał w siódmym dniu wszelkich Swoich czynności, które wykonywał. 
        /// 3. Pobłogosławił Bóg siódmy dzień i uświęcił go. Bo w nim zaprzestał wszelkich Swoich czynności, 
        /// które Bóg stworzył do wykonania. 
        /// </summary>
        /// <returns></returns>
        public List<RecognizedVerse> GetRecognizedVerses() {
            var verses = new List<RecognizedVerse>();
            var pattern = @"(?<nr>[0-9]+)\.(\s+)(?<content>[^\d]+)";
            var regex = new Regex(pattern);
            var matches = regex.Matches(richEditControl.Text).Cast<Match>().ToList();
            foreach (Match match in matches) {
                var number = match.Groups["nr"].Value.ToInt();
                var content = match.Groups["content"].Value;
                if (number > 0 && content.IsNotNullOrEmpty()) {
                    verses.Add(new RecognizedVerse() {
                        Number = number,
                        Content = content
                    });
                }
            }

            return verses;
        }
    }

    public class RecognizedVerse {
        public int Number { get; set; }
        public string Content { get; set; }
    }
}
