using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog;
using TestApp.JsonUtils;
using TestApp.Models;

namespace TestApp
{
    class PersonSplittedVersionProxy : IPerson, IDisposable
    {
        private readonly string _tempDirectoryName;
        private CollectionDeserializer _collectionDeserializer;
        private readonly JsonFileSplitter _fileSplitter;
        private List<string> _fileNames;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public PersonSplittedVersionProxy(string fileName, string tempDirectoryName = null)
        {
            _tempDirectoryName = tempDirectoryName ?? Path.GetDirectoryName(fileName);
            _fileSplitter = new JsonFileSplitter(typeof(Person), fileName, _tempDirectoryName);
        }

        public void Initialize()
        {
            var typeInfos = _fileSplitter.SplitIntoFiles();
            _fileNames = typeInfos.Select(x => Path.Combine(_tempDirectoryName, x.Value)).ToList();
            _collectionDeserializer = new CollectionDeserializer(typeInfos, _tempDirectoryName);
        }

        public IEnumerable<Friend> Friends => _collectionDeserializer.Deserialize<Friend>();
        public IEnumerable<BankCard> BankCards => _collectionDeserializer.Deserialize<BankCard>();
        public IEnumerable<Book> Books => _collectionDeserializer.Deserialize<Book>();

        public void Dispose()
        {
            foreach (var file in _fileNames)
                try
                {
                    File.Delete(file);
                }
                catch (Exception exception)
                {
                    _logger.Error(exception);
                }
        }
    }
}
