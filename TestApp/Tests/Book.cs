namespace TestApp.Tests
{
   internal class Book
   {
      public string ISBN { get; set; }
      public string Title { get; set; }
      public string Author { get; set; }
      public int Year { get; set; }
      public decimal Cost { get; set; }
      public string Publisher { get; set; }
      public string Category { get; set; }

      public override string ToString()
      {
         return $"ISBN: {ISBN}, Title: {Title}, Author: {Author}, Publisher: {Publisher}, Category: {Category}, Year: {Year}, Cost: {Cost}";
      }
   }
}