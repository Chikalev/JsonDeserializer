using Castle.DynamicProxy;

namespace TestApp.Core
{
   class ProxyFactory<T> where T : class
   {
      public static T CreateProxy(string fileName)
      {
         var fileSplitter = new JsonFileSplitter(typeof(T), fileName);
         var collectionInfos = fileSplitter.SplitIntoFiles();
         var deserializer = new CollectionDeserializer(collectionInfos, fileSplitter.TempFolderName);
         var interceptor = new LazyDeserializingInterceptor(deserializer, fileSplitter);
         var generator = new ProxyGenerator();
         return generator.CreateClassProxy<T>(interceptor);
      }
   }
}
