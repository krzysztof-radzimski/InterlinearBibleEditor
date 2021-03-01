using DevExpress.Xpo;
using DevExpress.XtraBars.Ribbon;
using IBE.Data;
using IBE.Data.Model;
using Microsoft.Web.WebView2.Core;
using System;

namespace IBE.WindowsClient {
    public partial class ViewerForm : RibbonForm {
        public bool BrowserIsReady { get; private set; }
        public event EventHandler BrowserInitializationCompleted;
        public ViewerForm() {
            InitializeComponent();
            this.Text = "Bible Viewer";
            this.WebBrowser.CoreWebView2InitializationCompleted += WebBrowser_CoreWebView2InitializationCompleted;
            this.BrowserInitializationCompleted += OnBrowserInitializationCompleted;
            InitializeAsync();

            //ConnectionHelper.Connect();
            //using (var uow = new UnitOfWork()) {
            //    var books = new XPCollection<Book>(uow);
            //    foreach (var book in books) {
            //        cbBooksList.Items.Add(new BookInfo(book));
            //    }
            //    beiBooksList.Enabled = true;
            //}
        }

        async void InitializeAsync() {
            await WebBrowser.EnsureCoreWebView2Async(null);
        }

        private void OnBrowserInitializationCompleted(object sender, EventArgs e) {

        }

        private void WebBrowser_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e) {
            BrowserIsReady = e.IsSuccess;
            if (BrowserIsReady) {
                if (BrowserInitializationCompleted != null) { BrowserInitializationCompleted(sender, e); }
            }
        }
        private void cbBooksList_SelectedIndexChanged(object sender, EventArgs e) {
            //var book = beiBooksList.EditValue as BookInfo;
            //if (book != null) {
            //    beiChapter.EditValue = 1;
            //    txtChapter.MinValue = 1;
            //    txtChapter.MaxValue = book.NumberOfChapters;



                
            //}
        }
    }
}
