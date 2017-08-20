using System;
using System.Collections.Generic;

namespace TestApp.Tests
{
   internal class Container : IDisposable
   {
      public virtual IEnumerable<EntityOne> SequenceOne { get; set; }

      public virtual IEnumerable<Book> Books { get; set; }

      public virtual void Dispose()
      {
      }
   }
}