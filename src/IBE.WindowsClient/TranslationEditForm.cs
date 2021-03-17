using DevExpress.Xpo;
using DevExpress.XtraEditors;
using IBE.Common.Extensions;
using IBE.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IBE.WindowsClient {
    public partial class TranslationEditForm : XtraForm {
        public Translation Object { get; private set; }
        public TranslationEditForm() {
            InitializeComponent();
            this.Text = "Translation editor";
        }

        public TranslationEditForm(int id, Session session) : this() {
            if (id != 0 && session.IsNotNull()) {
                Object = new XPQuery<Translation>(session).Where(x => x.Oid == id).FirstOrDefault();
                if (Object.IsNotNull()) {
                    this.Text = $"Translation editor :: {Object.Name}";

                    foreach (var lang in Enum.GetValues(typeof(Language))) {
                        txtLanguage.Properties.Items.Add(lang);
                    }
                    foreach (var type in Enum.GetValues(typeof(TranslationType))) {
                        txtType.Properties.Items.Add(type);
                    }

                    txtName.DataBindings.Add("EditValue", Object, "Name");
                    txtDescription.DataBindings.Add("EditValue", Object, "Description");
                    txtChapterPsalmString.DataBindings.Add("EditValue", Object, "ChapterPsalmString");
                    txtChapterString.DataBindings.Add("EditValue", Object, "ChapterString");
                    txtDetailedInfo.DataBindings.Add("EditValue", Object, "DetailedInfo");
                    txtIntroduction.DataBindings.Add("EditValue", Object, "Introduction");
                    txtLanguage.DataBindings.Add("EditValue", Object, "Language");
                    txtType.DataBindings.Add("EditValue", Object, "Type");
                    cbIsCatholic.DataBindings.Add("EditValue", Object, "Catolic");
                    cbIsRecommended.DataBindings.Add("EditValue", Object, "Recommended");

                    //txtName.EditValue = Object.Name;
                    //txtDescription.EditValue = Object.Description;
                    //txtChapterPsalmString.EditValue = Object.ChapterPsalmString;
                    //txtChapterString.EditValue = Object.ChapterString;
                    //txtDetailedInfo.EditValue = Object.DetailedInfo;
                    //txtIntroduction.EditValue = Object.Introduction;
                    //txtLanguage.EditValue = Object.Language;
                    //txtType.EditValue = Object.Type;
                    //cbIsCatholic.EditValue = Object.Catolic;
                    //cbIsRecommended.EditValue = Object.Recommended;
                }
            }
        }

        public void Save() {
            Object.Save();
            Object.Session.CommitTransaction();            
        }
    }
}
