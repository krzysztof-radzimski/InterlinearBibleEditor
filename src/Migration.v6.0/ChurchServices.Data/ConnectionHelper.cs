/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using ChurchServices.Data.Model;
using DevExpress.Xpo.DB;

namespace ChurchServices.Data {
    public class ConnectionHelper {
        readonly Type[] PersistentTypes = new Type[]{
            typeof(Book),
            typeof(BookStatus),
            typeof(Chapter),
            typeof(Subtitle),
            typeof(Translation),
            typeof(Verse),
            typeof(VerseWord),
            typeof(Commentary),
            typeof(CommentaryItem),
            typeof(StrongCode),
            typeof(GrammarCode),
            typeof(AncientDictionary),
            typeof(AncientDictionaryItem),
            typeof(VerseInfo),
            typeof(BookBase),
            typeof(Article),
            typeof(UrlShort),
            typeof(Song),
            typeof(SongVerse)
        };

        public void Connect(bool threadSafe = true, string connectionString = null) {
            XpoDefault.DataLayer = CreateDataLayer(threadSafe, connectionString);
        }

        private IDataLayer CreateDataLayer(bool threadSafe, string connectionString = null) {
            if (connectionString == null) {
                connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            }
            var dictionary = new DevExpress.Xpo.Metadata.ReflectionDictionary();
            dictionary.GetDataStoreSchema(PersistentTypes);   // Pass all of your persistent object types to this method.
            var autoCreateOption = AutoCreateOption.DatabaseAndSchema;  // Use AutoCreateOption.DatabaseAndSchema if the database or tables do not exist. Use AutoCreateOption.SchemaAlreadyExists if the database already exists.
            IDataStore provider = XpoDefault.GetConnectionProvider(connectionString, autoCreateOption);
            return threadSafe ? (IDataLayer)new ThreadSafeDataLayer(dictionary, provider) : new SimpleDataLayer(dictionary, provider);
        }
    }
}
