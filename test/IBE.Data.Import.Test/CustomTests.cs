/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using DevExpress.Xpo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IBE.Data.Import.Test {
    [TestClass]
    public class CustomTests {
        [TestMethod]
        public void ImportBaseBooks() {
            ConnectionHelper.Connect();
            var uow = new UnitOfWork();
            uow.BeginTransaction();
            new BaseBooksImporter().Import(@"..\..\..\..\db\import\UBG'18.zip", uow);
            uow.CommitChanges();
        }
    }
}
