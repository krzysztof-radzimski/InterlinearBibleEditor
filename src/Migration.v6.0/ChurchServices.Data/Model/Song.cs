/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.Data.Model {
    public class Song : XPObject {
        private string name;
        private string signature;
        private int bpm;
        private int number;
        private string youtube;
        private SongGroupType type;

        [Size(250)]
        public string Name {
            get { return name; }
            set { SetPropertyValue(nameof(Name), ref name, value); }
        }

        [Size(20)]
        public string Signature {
            get { return signature; }
            set { SetPropertyValue(nameof(Signature), ref signature, value); }
        }

        public int BPM {
            get { return bpm; }
            set { SetPropertyValue(nameof(BPM), ref bpm, value); }
        }

        public int Number {
            get { return number; }
            set { SetPropertyValue(nameof(Number), ref number, value); }
        }

        [Size(200)]
        public string YouTube {
            get { return youtube; }
            set { SetPropertyValue(nameof(YouTube), ref youtube, value); }
        }

        public SongGroupType Type {
            get { return type; }
            set { SetPropertyValue(nameof(Type), ref type, value); }
        }

        //   [NonPersistent] public bool YoutubeLinkAdded => YouTube.IsNotNullOrEmpty();

        [Association("SongVerses")]
        public XPCollection<SongVerse> SongVerses {
            get { return GetCollection<SongVerse>(nameof(SongVerses)); }
        }

        public Song(Session session) : base(session) { }

        public SongPart[] GetParts() {
            var list = new List<SongPart>();
            SongPart current = null;

            foreach (var item in SongVerses.OrderBy(x => x.Index)) {
                var addBr = true;

                var next = SongVerses.Where(x => x.Index == item.Index + 1).FirstOrDefault();
                if (next != null) {
                    if (next.Type != item.Type || next.Number != item.Number) {
                        addBr = false;
                    }
                }
                else {
                    addBr = false;
                }

                if (current.IsNull()) {
                    current = new SongPart() { Type = item.Type, Number = item.Number, Text = "", Chords = "" };
                    list.Add(current);
                }
                else if (current.IsNotNull() && (current.Type != item.Type || current.Number != item.Number)) {
                    current = new SongPart() { Type = item.Type, Number = item.Number, Text = "", Chords = "" };
                    list.Add(current);
                }
                current.Chords += item.Chords + (addBr ? "<br />" : "");
                current.Text += item.Text + (addBr ? "<br />" : "");
            }

            return list.ToArray();
        }
    }

    public class SongPart {
        public string Text { get; set; }
        public string Chords { get; set; }
        public SongVerseType Type { get; set; }
        public int Number { get; set; }
    }

    public class SongVerse : XPObject {
        private Song parent;
        private string text;
        private string chords;
        private SongVerseType type;
        private int number;
        private int index;

        [Size(250)]
        public string Text {
            get { return text; }
            set { SetPropertyValue(nameof(Text), ref text, value); }
        }

        [Size(50)]
        public string Chords {
            get { return chords; }
            set { SetPropertyValue(nameof(Chords), ref chords, value); }
        }

        public SongVerseType Type {
            get { return type; }
            set { SetPropertyValue(nameof(Type), ref type, value); }
        }

        [Description("Part Number")]
        public int Number {
            get { return number; }
            set { SetPropertyValue(nameof(Number), ref number, value); }
        }

        public int Index {
            get { return index; }
            set { SetPropertyValue(nameof(Index), ref index, value); }
        }

        [Browsable(false)]        
        [Association("SongVerses")]
        public Song Parent {
            get { return parent; }
            set { SetPropertyValue(nameof(Parent), ref parent, value); }
        }        

        public SongVerse(Session session) : base(session) { }
    }

    public enum SongVerseType : int {
        [Description("Zwrotka")]
        Default = 0,
        [Description("Refren")]
        Chorus,
        [Description("Most")]
        Bridge
    }
    public enum SongGroupType : int {
        [Description("Pieśń")]
        Default = 0,
        [Description("Kolęda")]
        Carols,
        [Description("Wieczerza")]
        Eucharist,
        [Description("Dla dzieci")]
        SongsForChildren
    }
}
