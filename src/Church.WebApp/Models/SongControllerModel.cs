using DevExpress.Xpo;
using IBE.Data.Model;

namespace Church.WebApp.Models {
    public class SongControllerModel {
        public Song Song { get; set; }
        public XPView Songs { get; set; }
    }
}
