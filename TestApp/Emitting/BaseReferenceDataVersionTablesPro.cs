using System;
using System.Collections.Generic;
using ARMD.DataContracts.Api.ReferenceData.ReferenceDataVersionTypesContainers;
using ARMD.DataContracts.ToStations.ReferenceData.RatesRoutes;
using ARMD.DataContracts.ToStations.ReferenceData.SupportingTables;
using TestApp.Core;

namespace TestApp.Emitting
{
    public class BaseReferenceDataVersionTablesPro : BaseReferenceDataVersionTables
    {
        private CollectionDeserializer _collectionDeserializer;

        public BaseReferenceDataVersionTablesPro(CollectionDeserializer collectionDeserializer)
        {
            _collectionDeserializer = collectionDeserializer;
        }

        public int GetTariffs()
        {
            return _collectionDeserializer.GetSomeValue();
        }

        public new IEnumerable<GypType> GypTypes { get; }
        public IEnumerable<Tariff> Tariffs => new []
        {
            new Tariff
            {
                Code = 1,
                DepartureStationCode = 12,
                ChangedDateTime = DateTime.Today,
                DestinationStationCode = 343
            }
        };

        public dynamic Values
        {
            get
            {
                var items = new[] { "One", "Two", "Three" };
                return items;
            }
        }
    }
}