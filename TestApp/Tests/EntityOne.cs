namespace TestApp.Tests
{
   internal class EntityOne
   {
      public int A { get; set; }

      public string B { get; set; }

      public override string ToString()
      {
         return $"A: {A}, B: {B}";
      }
   }
}