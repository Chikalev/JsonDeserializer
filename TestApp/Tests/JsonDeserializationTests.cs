using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using TestApp.Core;

namespace TestApp.Tests
{
   [TestFixture(Category = "JsonDeserialization", 
                Description = "Функциональные тесты на десериализатор, использующий механизм " +
                              "разбивки исходного json-файла на файлы-коллекции")]
   public class JsonDeserializationTests
   {
      private string _tempFileName;

      [SetUp]
      public void Init()
      {
         _tempFileName = Path.GetTempFileName();
      }

      [Test]
      public void TestDeserializationIsCorrect()
      {
         File.AppendAllText(_tempFileName, Resource.JsonFile1);
         using (var proxy = ProxyFactory<Container>.CreateProxy(_tempFileName))
         {
            var sequenceOne = proxy.SequenceOne.ToArray();
            Assert.AreEqual(2, sequenceOne.Length);
            Assert.AreEqual(123, sequenceOne[0].A);
            Assert.AreEqual("Some Value", sequenceOne[0].B);
            Assert.AreEqual(586, sequenceOne[1].A);
            Assert.AreEqual("Another Value", sequenceOne[1].B);
            Assert.IsEmpty(proxy.Books);
         }
      }

      [Test]
      public void TestSwappedEmptyCollectionIsTreatedCorrectly()
      {
         File.AppendAllText(_tempFileName, Resource.JsonFille2);
         using (var proxy = ProxyFactory<Container>.CreateProxy(_tempFileName))
         {
            Assert.AreEqual(2, proxy.SequenceOne.Count());
            Assert.IsEmpty(proxy.Books);
         }
      }

      [Test]
      public void TestMissingPropertyInCollectionIsIgnored()
      {
         File.AppendAllText(_tempFileName, Resource.JsonFile3);
         using (var proxy = ProxyFactory<Container>.CreateProxy(_tempFileName))
         {
            Assert.AreEqual(2, proxy.Books.Count());
            Assert.That(proxy.Books.All(x => x.Category == null));
         }
      }

      [Test]
      public void TestAdditionalPropertyInJsonCollectionIsIgnored()
      {
         File.AppendAllText(_tempFileName, Resource.JsonFile4);
         using (var proxy = ProxyFactory<Container>.CreateProxy(_tempFileName))
         {
            Assert.AreEqual(2, proxy.SequenceOne.Count());
         }
      }

      [Test]
      public void TestRedundantCollectionInJsonIsIgnored()
      {
         File.AppendAllText(_tempFileName, Resource.JsonFile5);
         using (var proxy = ProxyFactory<Container>.CreateProxy(_tempFileName))
         {
            Assert.AreEqual(2, proxy.SequenceOne.Count());
            Assert.AreEqual(2, proxy.Books.Count());
         }
      }

      [Test]
      public void TestExceptionIsThrown()
      {
         File.AppendAllText(_tempFileName, Resource.JsonFile5);
         using (var proxy = ProxyFactory<ContainerWithWrongTypeOfCollection>.CreateProxy(_tempFileName))
         {
            ActualValueDelegate<int> testDelegate = () => proxy.SequenceOne.Count();
            Assert.That(testDelegate, Throws.TypeOf<JsonReaderException>());
         }
      }

      [Test]
      public void TestRenamedPropertyPicksUpCorrectly()
      {
         File.AppendAllText(_tempFileName, Resource.JsonFile6);
         using (var proxy = ProxyFactory<ContainerWithRenamedProperty>.CreateProxy(_tempFileName))
         {
            Assert.AreEqual(2, proxy.SequenceOne.Count());
            Assert.AreEqual(2, proxy.SequenceTwo.Count());
         }
      }

      [TearDown]
      public void DeleteTempFile()
      {
         File.Delete(_tempFileName);
      }
   }
}