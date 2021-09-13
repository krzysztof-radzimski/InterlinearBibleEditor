using IBE.Data.Model;
using System.Collections.Generic;

namespace IBE.Data.Export {
    public class CompareVerseModel {
        public VerseIndex Index { get; set; }
        public List<Verse> Verses { get; set; }
    }
}
