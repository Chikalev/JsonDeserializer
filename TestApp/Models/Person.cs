using System;
using System.Collections.Generic;

namespace TestApp.Models
{
   public class Person
   {
      public virtual int Id { get; set; }
      public virtual string Name { get; set; }
      public virtual int[] SomeProperty { get; set; }
      public virtual IEnumerable<Book> Books { get; set; }
   }

   public class Book : IEquatable<Book>
   {
      public string ISBN { get; set; }
      public string Title { get; set; }
      public string Author { get; set; }
      public int Year { get; set; }
      //public decimal Cost { get; set; }
      public string Publisher { get; set; }
      public string Category { get; set; }

      public bool Equals(Book other)
      {
         if (other == null) return false;
         return ISBN == other.ISBN;
      }

      public override int GetHashCode()
      {
         return ISBN.GetHashCode();
      }

      public override string ToString()
      {
         return $"ISBN: {ISBN}, Title: {Title}, Author: {Author}, Year: {Year}, Publisher: {Publisher}, Category: {Category}";
      }
   }
}