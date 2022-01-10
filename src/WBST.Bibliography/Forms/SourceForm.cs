using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using WBST.Bibliography.Model;

namespace WBST.Bibliography.Forms {
    public partial class SourceForm : XtraForm {
        public BibliographySource Source { get; private set; }
        public SourceForm() {
            InitializeComponent();
            var guid = Guid.NewGuid().ToString();
            Source = new BibliographySource() {
                Author = new BibliographyAuthor() {
                    Author = new Author() {
                        Objects = new List<object> {
                             new BibliographyNameList() {
                                  People = new List<BibliographyPerson> {
                                      new BibliographyPerson() {
                                        First = "Andrzej",
                                         Last = "Witczak"
                                      }
                                  }
                             }
                         }
                    }
                },
                City = "Warszawa",
                CountryRegion = "Polska",
                DayAccessed = DateTime.Now.Day.ToString().PadLeft(2, '0'),
                MonthAccessed = DateTime.Now.Month.ToString().PadLeft(2, '0'),
                YearAccessed = DateTime.Now.Year.ToString(),
                Guid = guid,
                Publisher = "Wydawnictwo ",
                SourceType = SourceTypeEnum.Book,
                Tag = "W" + guid.Substring(0, 5).ToLower(),
                Title = "Tytuł",
                Year = DateTime.Now.Year.ToString()
            };
        }
    }
}
