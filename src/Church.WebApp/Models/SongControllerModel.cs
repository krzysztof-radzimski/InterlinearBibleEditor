using IBE.Data.Model;
using System.Collections.Generic;

namespace Church.WebApp.Models {
    public class SongControllerModel {
        public Song Song { get; set; }
        public IEnumerable<SongsInfo> Songs { get; set; }
        public int MaxNumber { get; set; }
    }

    public class SongsInfo {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Signature { get; set; }
        public string BPM { get; set; }
        public int Number { get; set; }
        public SongGroupType Type { get; set; }
    }
}
