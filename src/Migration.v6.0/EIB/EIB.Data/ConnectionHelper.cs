using DevExpress.Xpo.DB;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EIB.Data.Model;

namespace EIB.Data {
    public class ConnectionHelper {

        readonly Type[] PersistentTypes = new Type[]{
            typeof(Translation),
            typeof(Book),
            typeof(Chapter),
            typeof(Subtitle),
            typeof(Verse),
            typeof(BookBase),
         };

        public void Connect(bool threadSafe = true, string connectionString = null, AutoCreateOption autoCreateOption = AutoCreateOption.SchemaAlreadyExists) {
            XpoDefault.DataLayer = CreateDataLayer(threadSafe, connectionString, autoCreateOption);
        }

        private IDataLayer CreateDataLayer(bool threadSafe, string connectionString = null, AutoCreateOption autoCreateOption = AutoCreateOption.SchemaAlreadyExists) {
            if (connectionString == null) {
                connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            }
            var dictionary = new DevExpress.Xpo.Metadata.ReflectionDictionary();
            dictionary.GetDataStoreSchema(PersistentTypes);   // Pass all of your persistent object types to this method.
            IDataStore provider = XpoDefault.GetConnectionProvider(connectionString, autoCreateOption);
            return threadSafe ? (IDataLayer)new ThreadSafeDataLayer(dictionary, provider) : new SimpleDataLayer(dictionary, provider);
        }
    }
}
