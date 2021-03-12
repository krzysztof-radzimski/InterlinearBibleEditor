/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using IBE.Data.Model;
using System;
using System.Configuration;

namespace IBE.Data {
    public static class ConnectionHelper {

        static readonly Type[] PersistentTypes = new Type[]{
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
        };

        public static void Connect(bool threadSafe = true, string connectionString = null) {
            XpoDefault.DataLayer = CreateDataLayer(threadSafe, connectionString);
        }

        static IDataLayer CreateDataLayer(bool threadSafe, string connectionString = null) {
            if (connectionString == null) {
                connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            }
            //connStr = XpoDefault.GetConnectionPoolString(connStr);  // Uncomment this line if you use a database server like SQL Server, Oracle, PostgreSql etc.
            ReflectionDictionary dictionary = new ReflectionDictionary();
            dictionary.GetDataStoreSchema(PersistentTypes);   // Pass all of your persistent object types to this method.
            AutoCreateOption autoCreateOption = AutoCreateOption.DatabaseAndSchema;  // Use AutoCreateOption.DatabaseAndSchema if the database or tables do not exist. Use AutoCreateOption.SchemaAlreadyExists if the database already exists.
            IDataStore provider = XpoDefault.GetConnectionProvider(connectionString, autoCreateOption);
            return threadSafe ? (IDataLayer)new ThreadSafeDataLayer(dictionary, provider) : new SimpleDataLayer(dictionary, provider);
        }
    }
}
