using DevExpress.Xpo;
using System;

namespace IBE.Data.Import {
    public interface IImporter : IDisposable {
        void Import(string filePath, UnitOfWork uow);
    }
}
