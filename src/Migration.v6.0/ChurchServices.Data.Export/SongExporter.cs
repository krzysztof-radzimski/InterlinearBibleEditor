/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using Aspose.Words.Tables;

namespace ChurchServices.Data.Export {
    public class SongExporter : BaseExporter {
        public SongExporter() : base() { }
        public SongExporter(byte[] asposeLicense, string host) : base(asposeLicense, host) { }
        public byte[] Export(Song song, ExportSaveFormat saveFormat) {
            if (song != null) {
                var builder = GetDocumentBuilder();

                var normalStyle = builder.Document.Styles[StyleIdentifier.Normal];
                if (normalStyle.IsNotNull()) {
                    normalStyle.Font.Size = 14;
                }
                AddSong(song, builder);
                builder.Font.Size = 10;
                builder.Writeln($"{DateTime.Now.Year} - Śpiewnik zborowy Kościoła Chrześcijan Baptystów w Nowym Dworze Mazowieckim");
                builder.Writeln("ul. Sukienna 52, 05-100 Nowy Dwór Mazowiecki");
                builder.Writeln("https://kosciol-jezusa.pl http://ndm.baptysci.pl");

                return SaveBuilder(saveFormat, builder);
            }
            return default;
        }

        private void AddSong(Song song, DocumentBuilder builder) {
            builder.ParagraphFormat.Style = builder.Document.Styles["Nagłówek 1"];
            builder.ParagraphFormat.KeepWithNext = true;
            builder.Writeln($"{song.Number}. {song.Name}");
            builder.ParagraphFormat.ClearFormatting();
            builder.ParagraphFormat.KeepWithNext = true;
            builder.Writeln($"Syg.: {song.Signature} BPM: {song.BPM}");
            builder.ParagraphFormat.KeepWithNext = true;
            builder.InsertParagraph();
            
            builder.StartTable();

            foreach (var item in song.GetParts()) {
                var cellText = builder.InsertCell();
                cellText.CellFormat.PreferredWidth = PreferredWidth.FromPercent(70);
                var textTable = item.Text.Split("<br />");
                if (item.Type == SongVerseType.Chorus) {
                    builder.Bold = true;
                    foreach (var text in textTable) {
                        builder.ParagraphFormat.KeepWithNext = false;
                        builder.Write(text);
                        builder.InsertBreak(BreakType.LineBreak);
                    }
                    builder.Bold = false;
                }
                else if (item.Type == SongVerseType.Default) {
                    builder.Bold = false;
                    builder.Italic = false;
                    foreach (var text in textTable) {
                        builder.Write(text);
                        builder.InsertBreak(BreakType.LineBreak);
                    }
                }
                else if (item.Type == SongVerseType.Bridge) {
                    builder.Italic = true;
                    foreach (var text in textTable) {
                        builder.Write(text);
                        builder.InsertBreak(BreakType.LineBreak);
                    }
                    builder.Italic = false;
                }

                var chordsTable = item.Chords.Split("<br />");
                var cellChords = builder.InsertCell();
                cellChords.CellFormat.PreferredWidth = PreferredWidth.FromPercent(25);
                foreach (var text in chordsTable) {
                    builder.Write(text);
                    builder.InsertBreak(BreakType.LineBreak);
                }
                var row = builder.EndRow();
                row.RowFormat.AllowBreakAcrossPages = false;
            }


            var table = builder.EndTable();
            table.ClearBorders();
            builder.ParagraphFormat.KeepWithNext = false;
            builder.InsertParagraph();
            builder.ParagraphFormat.KeepWithNext = false;
        }

        public byte[] ExportAll(List<Song> songs, ExportSaveFormat saveFormat) {
            if (songs != null) {
                var builder = GetDocumentBuilder();

                var normalStyle = builder.Document.Styles[StyleIdentifier.Normal];
                if (normalStyle.IsNotNull()) {
                    normalStyle.Font.Size = 14;
                }

                foreach (var song in songs) {
                    AddSong(song, builder);
                }

                builder.Font.Size = 10;
                builder.Writeln($"{DateTime.Now.Year} - Śpiewnik zborowy Kościoła Chrześcijan Baptystów w Nowym Dworze Mazowieckim");
                builder.Writeln("ul. Sukienna 52, 05-100 Nowy Dwór Mazowiecki");
                builder.Writeln("https://kosciol-jezusa.pl http://ndm.baptysci.pl");

                return SaveBuilder(saveFormat, builder);
            }
            return default;
        }
    }
}
