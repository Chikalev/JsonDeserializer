using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using NLog;

namespace TestApp.Core
{
    public class JsonFileSplitter
    {
        private readonly string _fileName;
        private readonly string _directoryName;
        private readonly string _tempDirectoryName;
        private readonly Dictionary<string, CollectionInfo> _propertyName2CollectionMap = new Dictionary<string, CollectionInfo>(StringComparer.InvariantCultureIgnoreCase);
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
       private DirectoryInfo _dirInfo;

       public JsonFileSplitter(Type rootType, string fileName, string tempDirectoryName = null)
        {
            if (rootType == null)
                throw new ArgumentNullException(nameof(rootType));
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));
            if (tempDirectoryName != null && !Directory.Exists(tempDirectoryName))
               throw new DirectoryNotFoundException(tempDirectoryName);

            _fileName = fileName;
            _directoryName = Path.GetDirectoryName(_fileName);
            _tempDirectoryName = tempDirectoryName ?? _directoryName;
            RegisterTypes(rootType);
         //Create temp folder
         var file = Path.GetFileNameWithoutExtension(fileName);
         _dirInfo = Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(fileName), file + "temp"));
      }

       public string FileName => _fileName;
       public string TempFolderName => _dirInfo.FullName;

       //internal Dictionary<string, CollectionInfo> PropertyCollection => _propertyName2CollectionMap; 

        private void RegisterTypes(Type type)
        {
           const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

           var properties = type.GetProperties(bindingFlags);
            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;
                if (propertyType.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(propertyType.GetGenericTypeDefinition()))
                {
                    var elementType = propertyType.GetGenericArguments()[0];
                    var nestedPropCount = elementType.GetProperties(bindingFlags).Length;

                    var propertyName = GetPropertyName(property);
                    _propertyName2CollectionMap[propertyName] = new CollectionInfo(propertyName, elementType, nestedPropCount);
                }
            }

            foreach (var property in _propertyName2CollectionMap)
               _logger.Debug("PropertyName: {0} Type: {1}", property.Key, property.Value);
        }

        public Dictionary<string, CollectionInfo> SplitIntoFiles()
        {
            //var type2FileNameMap = new Dictionary<Type, string>();
            using (var streamReader = File.OpenText(_fileName))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                while (jsonReader.Read())
                {
                   if (jsonReader.TokenType != JsonToken.PropertyName) continue;

                   CollectionInfo collectionInfo;
                   var propertyName = (string) jsonReader.Value;
                   if (!_propertyName2CollectionMap.TryGetValue(propertyName, out collectionInfo))
                   {
                      _logger.Error("Unknown property name: {0}", propertyName);
                      do
                      {
                         jsonReader.Read();
                      } while (jsonReader.TokenType != JsonToken.EndArray);
                      continue;
                   }
                   WriteCollectionToFile(collectionInfo, jsonReader);
                   //type2FileNameMap[collectionInfo.Type] = propertyName;
                }
            }
            return _propertyName2CollectionMap;
        }

        private void WriteCollectionToFile(CollectionInfo collectionInfo, JsonReader jsonReader)
        {
            var fullFileName = Path.Combine(_dirInfo.FullName, collectionInfo.Name);
            using (var streamWriter = new StreamWriter(fullFileName))
            using (var writer = new JsonTextWriter(streamWriter))
            {
               writer.Formatting = Formatting.Indented;

               int propCount = GetActualPropetyCount(jsonReader, writer);
               if (propCount == 0) return;

               while (jsonReader.Read())
               {
                  if (jsonReader.TokenType == JsonToken.EndArray)
                     break;

                  writer.WriteStartObject();
                  for (int i = 0; i < propCount; i++)
                  {
                     jsonReader.Read();
                     writer.WritePropertyName((string) jsonReader.Value);
                     jsonReader.Read();
                     writer.WriteValue(jsonReader.Value);
                  }
                  jsonReader.Read();
                  writer.WriteEndObject();
               }

               writer.WriteEndArray();
            }
        }

      private int GetActualPropetyCount(JsonReader reader, JsonTextWriter writer)
      {
         int count = 0;
         reader.Read();
         writer.WriteStartArray();
         reader.Read();
         if (reader.TokenType == JsonToken.EndArray)
         {
            writer.WriteEndArray();
            return count;
         }

         writer.WriteStartObject();
         while (reader.Read())
         {
            if (reader.TokenType == JsonToken.EndObject)
            {
               writer.WriteEndObject();
               return count;
            }

            writer.WritePropertyName((string)reader.Value);
            reader.Read();
            writer.WriteValue(reader.Value);
            count++;
         }
         return count;
      }

      private string GetPropertyName(PropertyInfo propertyInfo)
        {
            var jsonPropertyAttributes = (JsonPropertyAttribute[])Attribute.GetCustomAttributes(propertyInfo, typeof(JsonPropertyAttribute));
            if (jsonPropertyAttributes.Length == 0)
                return propertyInfo.Name;

            return jsonPropertyAttributes[0].PropertyName;
        }

       public void DeleteTempFolder()
       {
          try
          {
            if (_dirInfo.Exists)
               _dirInfo.Delete(true);
          }
          catch (Exception e)
          {
             _logger.Error(e.ToString);
          }
       }

       public class CollectionInfo
       {
          internal string Name { get; }
          internal Type Type { get; }
          internal int PropertyCount { get; }

          public CollectionInfo(string name, Type type, int propertyCount)
          {
             Name = name;
             Type = type;
             PropertyCount = propertyCount;
          }

          public override string ToString()
          {
             return $"Name: {Name}, Type: {Type.FullName}, PropertyCount: {PropertyCount}";
          }
       }
    }
}