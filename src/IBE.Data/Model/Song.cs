using DevExpress.Xpo;
using System.ComponentModel;

namespace IBE.Data.Model {
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

        [Association("SongVerses")]
        public XPCollection<SongVerse> SongVerses {
            get { return GetCollection<SongVerse>(nameof(SongVerses)); }
        }

        public Song(Session session) : base(session) { }
    }

    public class SongVerse : XPObject {
        private Song parent;
        private string text;
        private string chords;
        private SongVerseType type;

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

        [Browsable(false)]
        [Association("SongVerses")]
        public Song Parent {
            get { return parent; }
            set { SetPropertyValue(nameof(Parent), ref parent, value); }
        }

        public SongVerse(Session session) : base(session) { }
    }

    public enum SongVerseType : int {
        Default = 0,
        Chorus,
        Bridge
    }
    public enum SongGroupType : int {        
        Default = 0,
        Carols,
        Eucharist,
        SongsForChildren
    }
}
