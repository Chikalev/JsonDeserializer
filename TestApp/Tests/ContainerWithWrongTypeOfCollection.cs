using System;
using System.Collections.Generic;

namespace TestApp.Tests
{
   internal class ContainerWithWrongTypeOfCollection : IDisposable
   {
      public virtual IEnumerable<EntityTwo> SequenceOne { get; set; }

      public virtual IEnumerable<Book> Books { get; set; }

      public virtual void Dispose()
      {
      }
   }
}