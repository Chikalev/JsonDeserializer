using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using ARMD.DataContracts.Api.ReferenceData.ReferenceDataVersionTypesContainers;
using ARMD.DataContracts.ToStations.ReferenceData.RatesRoutes;
using ARMD.DataContracts.ToStations.ReferenceData.SupportingTables;
using Castle.DynamicProxy;
using Castle.DynamicProxy.Generators;
using Newtonsoft.Json;
using NLog;
using TestApp.Core;
using TestApp.DAL;
using TestApp.Emitting;
using TestApp.Models;

namespace TestApp
{
    class Program
    {
        private const string DefaultFileName = @"C:\CPPKSoftware\Caches\RDS\Base_135\data";
        private const string FileName1 = @"C:\CPPKSoftware\Caches\RDS\Base_141\data";

       private const string PersonFileName =
          @"C:\Users\Igor\Downloads\TestAppReflected\TestApp\TestApp\OnePersonLarge.json";

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            try
            {
                TestInterceptor(PersonFileName);
                //SingleMethodDynamicTest(FileName1);
                //TestBulkInsert();
                //TestFile();
                Console.WriteLine("SUCCEEDED");
                _logger.Debug("**********************************************************************************************");
            }
            catch (Exception exception)
            {
                _logger.Error(exception.ToString);
                Console.WriteLine(exception.Message);
            }
            Console.ReadLine();
        }

       private static void TestInterceptor(string fileName)
       {
          JsonFileSplitter fileSplitter = null;
          try
         {
            var generator = new ProxyGenerator();
            fileSplitter = new JsonFileSplitter(typeof(Person), fileName);
            var collectionInfos = fileSplitter.SplitIntoFiles();

            var deserializer = new CollectionDeserializer(collectionInfos, fileSplitter.TempFolderName);
            var interceptor = new LazyDeserializingInterceptor(deserializer, fileSplitter);
            var proxy = generator.CreateClassProxy<Person>(interceptor);
            //var books = proxy.Books;
            foreach (var item in proxy.Books.Distinct())
               _logger.Debug(item);
         }
         finally
         {
            fileSplitter?.DeleteTempFolder();
         }
       }

       /*private static void SingleMethodDynamicTest(string fileName)
        {
            var emitter = new Emitter();
            var type = emitter.CreateType();
            //var proxy = new BaseReferenceDataVersionTablesPro();
            //proxy.

            var propertyInfo = type.GetProperty("Tariffs", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            //_logger.Debug("type: {0}", type.FullName);
            var deserializer = new CollectionDeserializer(SplitIntoFiles(fileName), Path.GetDirectoryName(fileName));
            var constructors = type.GetConstructors();
            var obj = constructors[0].Invoke(new object[] { deserializer });
            //var value = propertyInfo.GetValue(obj, null);
            //_logger.Debug("value = {0}", value);
            foreach (Tariff tariff in (IEnumerable<Tariff>)propertyInfo.GetValue(obj, null))
            {
                _logger.Debug("DepartureStationCode {0}; DepartureTariffZoneCode {1} DestinationStationCode {2} Price {3} Code {4}",
                    tariff.DepartureStationCode, tariff.DepartureTariffZoneCode, tariff.DestinationStationCode, tariff.Price, tariff.Code);
            }
        }*/

        /*private static BaseReferenceDataVersionTablesPro GetProxy()
        {
            return new BaseReferenceDataVersionTablesPro();
        }*/

        private static Dictionary<Type, string> SplitIntoFiles(string fileName)
        {
            var types = RegisterTypes(typeof(BaseReferenceDataVersionTables));

            var type2FileNameMap = new Dictionary<Type, string>();
            using (var streamReader = File.OpenText(fileName))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                while (jsonReader.Read())
                {
                    if (jsonReader.TokenType != JsonToken.PropertyName) continue;

                    JsonFileSplitter.CollectionInfo collectionInfo;
                    var propertyName = (string)jsonReader.Value;
                    if (!types.TryGetValue(propertyName, out collectionInfo))
                    {
                        //_logger.Error("Unknown property name: {0}", propertyName);
                        continue;
                    }
                    /*WriteCollectionToFile(collectionInfo, jsonReader);*/
                    type2FileNameMap[collectionInfo.Type] = propertyName;
                }
            }
            return type2FileNameMap;
        }

        private static Dictionary<string, JsonFileSplitter.CollectionInfo> RegisterTypes(Type type)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

            var propertyName2CollectionMap = new Dictionary<string, JsonFileSplitter.CollectionInfo>(StringComparer.InvariantCultureIgnoreCase);

            var properties = type.GetProperties(bindingFlags);
            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;
                if (propertyType.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(propertyType.GetGenericTypeDefinition()))
                {
                    var elementType = propertyType.GetGenericArguments()[0];
                    var nestedPropCount = elementType.GetProperties(bindingFlags).Length;

                    var propertyName = GetPropertyName(property);
                    propertyName2CollectionMap[propertyName] = new JsonFileSplitter.CollectionInfo(propertyName, elementType, nestedPropCount);
                }
            }
            return propertyName2CollectionMap;
        }

        private static string GetPropertyName(PropertyInfo propertyInfo)
        {
            var jsonPropertyAttributes = (JsonPropertyAttribute[])Attribute.GetCustomAttributes(propertyInfo, typeof(JsonPropertyAttribute));
            if (jsonPropertyAttributes.Length == 0)
                return propertyInfo.Name;

            return jsonPropertyAttributes[0].PropertyName;
        }

        private static void TestBulkInsert()
        {
            var class1 = new Class1();
            class1.TestBulkInsertWithBLToolkit();
            //class1.TestBulkInsert();
        }

        /*private static void TestFile()
        {
            var watch = Stopwatch.StartNew();
            List<long> milliseconds = new List<long>();

            var fileName = FileName;
            Console.WriteLine("FileName: {0}", fileName);
            _logger.Debug("FileName: {0}", fileName);

            using (var proxy = new BaseReferenceDataVersionTablesProxy1(fileName))
            {
                proxy.Initialize();
                watch.Stop();
                milliseconds.Add(watch.ElapsedMilliseconds);
                _logger.Debug("Initialize taken {0} ms", watch.ElapsedMilliseconds);
                watch.Restart();
                LogMemoryConsumption();

                LogCollectionCount(proxy);

                watch.Stop();
                milliseconds.Add(watch.ElapsedMilliseconds);
                _logger.Debug("multiple properties, time = {0}", milliseconds[1]);

                LogMemoryConsumption();
            }
            _logger.Debug("Totally elapsed {0} ms", milliseconds.Sum());
        }*/

        private static void LogCollectionCount(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

            var countMethodInfo = typeof(Enumerable).GetMethods().Single(
                method => method.Name == "Count" && method.IsStatic && method.GetParameters().Length == 1);

            var properties = obj.GetType().GetProperties(bindingFlags);
            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;
                if (propertyType.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(propertyType.GetGenericTypeDefinition()))
                {
                    var elementType = propertyType.GetGenericArguments()[0];
                    MethodInfo countMethod = countMethodInfo.MakeGenericMethod(elementType);

                    var count = countMethod.Invoke(null, new[] { property.GetValue(obj, null) });
                    _logger.Debug("{0}: Count = {1}", property.Name, count);
                }
            }
        }

        private static string FileName
        {
            get
            {
                var fileName = ConfigurationManager.AppSettings["FileName"];
                return fileName ?? DefaultFileName;
            }
        }

        private static void LogMemoryConsumption()
        {
            Process currentProcess = Process.GetCurrentProcess();
            long totalBytesOfMemoryUsed = currentProcess.WorkingSet64;
            _logger.Debug("Totally used {0} bytes", totalBytesOfMemoryUsed);
        }
    }
}
