namespace TestApp.Tests
{
   internal class EntityTwo
   {
      public int A { get; set; }

      public double B { get; set; }

      public override string ToString()
      {
         return $"A: {A}, B: {B}";
      }
   }
}