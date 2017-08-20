using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using NLog;

namespace TestApp
{
    class JsonCollectionReader
   {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly StreamReader _streamReader;
        private readonly JsonReader _reader;
        private readonly Dictionary<Type, string> _type2CollectionNameMap = new Dictionary<Type, string>();
        private readonly Queue<JsonCollectionInfo> _currentCollectionInfos = new Queue<JsonCollectionInfo>();
        private readonly JsonSerializer _serializer = new JsonSerializer { DateTimeZoneHandling = DateTimeZoneHandling.Unspecified };

        public JsonCollectionReader(StreamReader streamReader)
        {
            _streamReader = streamReader;
            _reader = new JsonTextReader(_streamReader);
            //_reader.
        }

        public void RegisterTypes<T>()
        {
            /*_currentCollectionInfos.Enqueue(new JsonCollectionInfo { Name = "tariffs", Type = typeof(Tariff) });
            _currentCollectionInfos.Enqueue(new JsonCollectionInfo { Name = "ProhibitedForManualEntryExemptions", Type = typeof(ProhibitedForManualEntryExemption) });
            _currentCollectionInfos.Enqueue(new JsonCollectionInfo {Name = "versions", Type = typeof(Models.Version) });
            _currentCollectionInfos.Enqueue(new JsonCollectionInfo { Name = "deposittariffs", Type = typeof(DepositTariff) });
            return;*/
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;
                if (propertyType.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(propertyType.GetGenericTypeDefinition()))
                {
                    Type elementType = propertyType.GetGenericArguments()[0];

                    _type2CollectionNameMap[elementType] = GetPropertyName(property);
                }
            }

            foreach (var property in _type2CollectionNameMap)
                _logger.Debug("Type: {0} CollectionName: {1}", property.Key, property.Value);

            //PopulateQueue();
        }

        private void PopulateQueue()
        {
            while (_reader.Read())
            {
                string propertyName = null;
                if (_reader.TokenType != JsonToken.PropertyName ||
                    !string.Equals((string)_reader.Value, propertyName, StringComparison.InvariantCultureIgnoreCase))
                    continue;

                while (_reader.Read())
                {
                    if (_reader.TokenType == JsonToken.StartArray)
                        continue;

                    if (_reader.TokenType == JsonToken.EndArray)
                        break;

                    var item = _serializer.Deserialize<PersonProxy>(_reader);
                    //yield return item;
                }
            }
            _streamReader.BaseStream.Position = 0;
            _streamReader.DiscardBufferedData();
        }

        public IEnumerable<T> DeserializeSequence<T>()
        {
            var info = _currentCollectionInfos.Peek();
            var currentType = typeof(T);
            if (info.Type != currentType)
            {
                _logger.Debug("Type mismatch: current type is {0}, expected {1}", currentType.Name, info.Type.Name);
                yield break;
            }
            //throw new Exception("Probably incorrect order of call or type has not been registered");
            /*streamReader.BaseStream.Position = 0;
              streamReader.DiscardBufferedData();*/

            //using (var reader = new JsonTextReader(streamReader))
            //var reader = new JsonTextReader(streamReader);


            /*string propertyName;
            if (!_type2PropertyNameMap.TryGetValue(currentType, out propertyName))
            {
               throw new Exception("Type has not been registered");
            }
            _typesProcessed.Add(currentType);*/

            while (_reader.Read())
            {
                if (_reader.TokenType == JsonToken.PropertyName && string.Equals((string)_reader.Value, info.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    while (_reader.Read())
                    {
                        if (_reader.TokenType == JsonToken.StartArray)
                            continue;
                        
                        if (_reader.TokenType == JsonToken.EndArray)
                        {
                            _currentCollectionInfos.Dequeue();
                            break;
                        }

                        var item = _serializer.Deserialize<T>(_reader);
                        yield return item;
                    }
                    break;
                }
            }
        }

        public IEnumerable<T> DeserializeSequenceRevisited<T>()
        {
            string propertyName;
            if (!_type2CollectionNameMap.TryGetValue(typeof(T), out propertyName))
            {
                _logger.Error("Type {0} has not been registered", typeof(T));
                yield break;
            }

            _streamReader.BaseStream.Position = 0;

            using (var reader = new JsonTextReader(_streamReader))
            {
                reader.CloseInput = false;
                var serializer = new JsonSerializer { DateTimeZoneHandling = DateTimeZoneHandling.Unspecified };

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        if (!string.Equals((string) reader.Value, propertyName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            reader.Read();
                            reader.Skip();
                            continue;
                        }

                        while (reader.Read())
                        {
                            if (reader.TokenType == JsonToken.StartArray)
                                continue;

                            if (reader.TokenType == JsonToken.EndArray)
                                break;

                            var item = serializer.Deserialize<T>(reader);
                            yield return item;
                        }
                    }
                }
            }
        }

        private string GetPropertyName(PropertyInfo propertyInfo)
        {
            var jsonPropertyAttributes = (JsonPropertyAttribute[])Attribute.GetCustomAttributes(propertyInfo, typeof(JsonPropertyAttribute));
            if (jsonPropertyAttributes.Length == 0)
                return propertyInfo.Name;

            return jsonPropertyAttributes[0].PropertyName;
        }

        class JsonCollectionInfo
        {
            public Type Type { get; set; }
            public string Name { get; set; }
        }
    }
}
