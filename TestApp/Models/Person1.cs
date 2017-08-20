using System;
using System.Collections.Generic;
using System.IO;

namespace TestApp.Models
{
    class Person1 : Person, IDisposable
    {
        private readonly StreamReader _streamReader;

        public Person1(string fileName)
        {
            _streamReader = File.OpenText(fileName);
        }

        public new IEnumerable<Friend> Friends => JsonReader.DeserializeSequence<Friend>(_streamReader, nameof(Friends));

        public new IEnumerable<BankCard> BankCards => JsonReader.DeserializeSequence<BankCard>(_streamReader, nameof(BankCards));

        public void Dispose()
        {
            _streamReader.Close();
        }
    }
}
