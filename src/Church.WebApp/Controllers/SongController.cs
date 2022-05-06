/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using Church.WebApp.Models;
using DevExpress.Xpo;
using IBE.Common.Extensions;
using IBE.Data.Model;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Church.WebApp.Controllers {
    public class SongController : Controller {
        public IActionResult Index() {
            var qs = Request.QueryString;
            if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty() && qs.Value.Length > 3) {
                var value = qs.Value;
                if (value.Contains("&")) {
                    value = value.Substring(0, value.IndexOf("&"));
                }
                var id = value.ToLower().Replace("?id=", "").Trim().ToInt();
                var song = new XPQuery<Song>(new UnitOfWork()).Where(x => x.Number == id).FirstOrDefault();
                if (song.IsNotNull()) {

                    var view = new XPView(song.Session, typeof(Song));
                    view.CriteriaString = $"([Number] < {song.Number + 10} ) AND ([Number] > {song.Number - 10})";
                    view.Properties.Add(new ViewProperty("Id", SortDirection.None, "[Oid]", false, true));
                    view.Properties.Add(new ViewProperty("Name", SortDirection.None, "[Name]", false, true));
                    view.Properties.Add(new ViewProperty("Number", SortDirection.Ascending, "[Number]", false, true));

                    var maxNumber = new XPQuery<Song>(new UnitOfWork()).Select(x => x.Number).Max();

                    var result = new SongControllerModel() { Song = song, Songs = view, MaxNumber = maxNumber };

                    return View(result);
                }
            }
            return View();
        }
    }

    public class SongsController : Controller {
        public IActionResult Index() {
            var view = new XPView(new UnitOfWork(), typeof(Song));
            view.Properties.Add(new ViewProperty("Id", SortDirection.None, "[Oid]", false, true));
            view.Properties.Add(new ViewProperty("Name", SortDirection.None, "[Name]", false, true));
            view.Properties.Add(new ViewProperty("Signature", SortDirection.None, "[Signature]", false, true));
            view.Properties.Add(new ViewProperty("BPM", SortDirection.None, "[BPM]", false, true));
            view.Properties.Add(new ViewProperty("Number", SortDirection.Ascending, "[Number]", false, true));
            view.Properties.Add(new ViewProperty("Type", SortDirection.None, "[Type]", false, true));
            return View(view);
        }
    }
}
