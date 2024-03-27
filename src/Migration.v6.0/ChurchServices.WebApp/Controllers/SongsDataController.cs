/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using System.Text;

namespace ChurchServices.WebApp.Controllers {
    [ApiController]
    [Route("/api/[controller]")]
    public class SongsDataController : JsonControllerBase {
        public IActionResult Get() {
            var songs = GetSongData();
            return FileJson(songs, "Pieśni.json");
        }
        [Route("/api/[controller]/{number}")]
        public IActionResult Get(int number) {
            var songs = GetSongData(number);
            return FileJson(songs, "Pieśni.json");
        }
        private List<SongDataItem> GetSongData(int number = 0) {
            List<Song> songs = null;
            if (number > 0) {
                songs = new XPQuery<Song>(new UnitOfWork()).Where(x => x.Number == number).ToList();
            }
            else {
                songs = new XPQuery<Song>(new UnitOfWork()).OrderBy(x => x.Number).ToList();
            }

            if (songs.Count > 0) {
                var result = new List<SongDataItem>();
                foreach (var song in songs) {
                    var item = new SongDataItem() {
                        BPM = song.BPM,
                        Title = song.Name,
                        Number = song.Number,
                        Signature = song.Signature,
                        Type = song.Type.GetDescription()
                    };
                    var sb = new StringBuilder();
                    var currentType = SongVerseType.Default;
                    var verses = song.SongVerses.OrderBy(x => x.Index).ToList();
                    foreach (var verse in verses) {
                        if (verse.Type != currentType) {
                            if (verse != verses.First()) { sb.Append(" "); }
                            if (verse.Type == SongVerseType.Default) {
                                sb.Append($"[Verse {verse.Number}] ");
                            }
                            else { sb.Append($"[{verse.Type.ToString()}] "); }
                            currentType = verse.Type;
                        }
                        sb.Append($"{verse.Text} ");
                    }
                    item.Text = sb.ToString();
                    result.Add(item);
                }
                return result;
            }

            return default;
        }
    }

    public class SongDataItem {
        public string Title { get; set; }
        public string Signature { get; set; }
        public int BPM { get; set; }
        public int Number { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
    }
}
