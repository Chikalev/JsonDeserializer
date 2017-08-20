using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TestApp.Tests
{
   internal class ContainerWithRenamedProperty : IDisposable
   {
      [JsonProperty(PropertyName = "RenamedProperty")]
      public virtual IEnumerable<EntityOne> SequenceOne { get; set; }

      public virtual IEnumerable<EntityTwo> SequenceTwo { get; set; }

      public virtual void Dispose()
      {
      }
   }
}