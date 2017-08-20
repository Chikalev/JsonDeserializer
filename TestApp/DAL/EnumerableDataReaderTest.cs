using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TestApp.Models;

namespace TestApp.DAL
{
    /*[TestFixture]
    class EnumerableDataReaderTest
    {
        private const string LargeFileName = @"C:\Temp\OnePerson.json";
        private const int Count = 2;

        private static IEnumerable<DataObj> DataSource => new List<DataObj>
        {
            new DataObj {Name = "1", Age = 16},
            new DataObj {Name = "2", Age = 26},
            new DataObj {Name = "3", Age = 36},
            new DataObj {Name = "4", Age = 46}
        };

        private static IEnumerable<Friend> Friends => new PersonProxy(LargeFileName).Friends;

        [Test]
        public void TestIEnumerableCtor()
        {
            var r = new EnumerableDataReader(Friends, typeof(Friend));
            while (r.Read())
            {
                var values = new object[Count];
                int count = r.GetValues(values);
                Assert.AreEqual(Count, count);

                values = new object[1];
                count = r.GetValues(values);
                Assert.AreEqual(1, count);

                values = new object[3];
                count = r.GetValues(values);
                Assert.AreEqual(2, count);

                /*Assert.IsInstanceOf(typeof(string), r.GetValue(0));
                Assert.IsInstanceOf(typeof(int), r.GetValue(1));#1#

                Console.WriteLine("Id: {0}, Name: {1}", r.GetValue(0), r.GetValue(1));
            }
        }

        [Test]
        public void TestIEnumerableOfAnonymousType()
        {
            // create generic list
            Func<Type, IList> toGenericList =
                type => (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(new[] { type }));

            // create generic list of anonymous type
            IList listOfAnonymousType = toGenericList(new { Name = "1", Age = 16 }.GetType());

            listOfAnonymousType.Add(new { Name = "1", Age = 16 });
            listOfAnonymousType.Add(new { Name = "2", Age = 26 });
            listOfAnonymousType.Add(new { Name = "3", Age = 36 });
            listOfAnonymousType.Add(new { Name = "4", Age = 46 });

            var r = new EnumerableDataReader(listOfAnonymousType);
            while (r.Read())
            {
                var values = new object[2];
                int count = r.GetValues(values);
                Assert.AreEqual(2, count);

                values = new object[1];
                count = r.GetValues(values);
                Assert.AreEqual(1, count);

                values = new object[3];
                count = r.GetValues(values);
                Assert.AreEqual(2, count);

                Assert.IsInstanceOf(typeof(string), r.GetValue(0));
                Assert.IsInstanceOf(typeof(int), r.GetValue(1));

                Console.WriteLine("Name: {0}, Age: {1}", r.GetValue(0), r.GetValue(1));
            }
        }

        [Test]
        public void TestIEnumerableOfTCtor()
        {
            var r = new EnumerableDataReader(DataSource);
            while (r.Read())
            {
                var values = new object[2];
                int count = r.GetValues(values);
                Assert.AreEqual(2, count);

                values = new object[1];
                count = r.GetValues(values);
                Assert.AreEqual(1, count);

                values = new object[3];
                count = r.GetValues(values);
                Assert.AreEqual(2, count);

                Assert.IsInstanceOf(typeof(string), r.GetValue(0));
                Assert.IsInstanceOf(typeof(int), r.GetValue(1));

                Console.WriteLine("Name: {0}, Age: {1}", r.GetValue(0), r.GetValue(1));
            }
        }

        class DataObj
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }*/
}
