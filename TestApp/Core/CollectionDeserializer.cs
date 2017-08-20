using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NLog;

namespace TestApp.Core
{
    public class CollectionDeserializer
   {
      //private readonly Dictionary<Type, string> _type2PropertyNameMap;
       private readonly Dictionary<string, JsonFileSplitter.CollectionInfo> _propertyCollection; 
      private readonly string _tempDirectoryName;
      private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

      public CollectionDeserializer(Dictionary<string, JsonFileSplitter.CollectionInfo> propCollection, string tempDirectoryName)
      {
         _propertyCollection = propCollection;
         _tempDirectoryName = tempDirectoryName;
      }

       public int GetSomeValue()
       {
           return 125;
       }

       /*public IEnumerable<Book> GetBooks(string propName)
       {
          yield return
             new Book
             {
                Author = "Richter",
                Cost = 10.23m,
                ISBN = "693-1589-85425-032",
                Title = "CLR via .NET",
                Year = 2008
             };
         yield return
             new Book
             {
                Author = "Troelsen",
                Cost = 12.23m,
                ISBN = "132-3244-353223-632",
                Title = ".NET Framework",
                Year = 2010
             };
      }*/

      public IEnumerable<T> Deserialize<T>(string propertyNameInClass)
      {
         JsonFileSplitter.CollectionInfo collectionInfo;
         if (!_propertyCollection.TryGetValue(propertyNameInClass, out collectionInfo) || typeof(T) != collectionInfo.Type)
         {
            _logger.Error("Property {0} of type {1} not found", propertyNameInClass, typeof(T).FullName);
            yield break;
         }
         var fileName = Path.Combine(_tempDirectoryName, collectionInfo.Name);
         if (!File.Exists(fileName))
            throw new FileNotFoundException("File not found", fileName);

         using (var streamReader = File.OpenText(fileName))
         using (var reader = new JsonTextReader(streamReader))
         {
                //TODO: use SerializationHelper.StandardJsonSerializer
            var serializer = new JsonSerializer { DateTimeZoneHandling = DateTimeZoneHandling.Unspecified };

            reader.Read();//Start array
            while (reader.Read())
            {
               if (reader.TokenType == JsonToken.EndArray)
                  break;

               var item = serializer.Deserialize<T>(reader);
               yield return item;
            }
         }
      }
   }
}
