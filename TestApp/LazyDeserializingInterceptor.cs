using System.Collections.Generic;
using System.Reflection;
using Castle.DynamicProxy;
using TestApp.Core;

namespace TestApp
{
   class LazyDeserializingInterceptor : IInterceptor
   {
      private const string GET_PREFIX = "get_";
      private const string DISPOSE_METHOD = "Dispose";

      private static readonly MethodInfo DeserializerGenericIteratorMethod =
            typeof(CollectionDeserializer).GetMethod("Deserialize", BindingFlags.Public | BindingFlags.Instance);

      private readonly JsonFileSplitter _fileSplitter;
      private readonly CollectionDeserializer _deserializer;

      public LazyDeserializingInterceptor(CollectionDeserializer deserializer, JsonFileSplitter fileSplitter)
      {
         _deserializer = deserializer;
         _fileSplitter = fileSplitter;
      }

      public void Intercept(IInvocation invocation)
      {
         var returnType = invocation.Method.ReturnType;

         if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof (IEnumerable<>))
            HandleGenericIteratorInvocation(invocation);
         else if (invocation.Method.Name == DISPOSE_METHOD)
            HandleDisposeInvocation();
         else invocation.Proceed();
      }

      private void HandleGenericIteratorInvocation(IInvocation invocation)
      {
         //TODO: Для чего?
         //invocation.Proceed();

         var method = DeserializerGenericIteratorMethod.MakeGenericMethod(invocation.Method.ReturnType.GetGenericArguments()[0]);
         string propertyName = invocation.Method.Name.Remove(0, GET_PREFIX.Length);
         invocation.ReturnValue = method.Invoke(_deserializer, new[] { propertyName });
      }

      private void HandleDisposeInvocation()
      {
         _fileSplitter.DeleteTempFolder();
      }

      /*private static IEnumerable<T> ProxyGenericIterator<T>(IEnumerable enumerable)
      {
         foreach (var element in enumerable)
            yield return (T)element;
      }

      private IEnumerable ProxyNonGenericIterator(IEnumerable enumerable)
      {
         try
         {
            foreach (var element in enumerable)
               yield return element;
         }
         finally
         {
            //CloseConnection(target);
         }
      }*/
   }
}
