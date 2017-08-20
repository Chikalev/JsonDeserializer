using System;
using System.Collections.Generic;

namespace TestApp.Models
{
    [Serializable]
    class Tariff
    {
        public int TariffPlanCode { get; set; }
        public int? RouteCode { get; set; }
        public int DepartureStationCode { get; set; }
        public int DestinationStationCode { get; set; }
        public int TicketTypeCode { get; set; }
        public decimal Price { get; set; }
        public int? DepartureTariffZoneCode { get; set; }
        public int? DestinationTariffZoneCode { get; set; }
        public int Code { get; set; }
        public string DataChecksum { get; set; }
        public int? DeleteInVersionId { get; set; }
        public int VersionId { get; set; }
        public DateTime ChangedDateTime { get; set; }
    }

    internal interface IBaseReferenceDataVersionTables
    {
        IEnumerable<Tariff> Tariffs { get; }
        IEnumerable<ProhibitedForManualEntryExemption> ProhibitedForManualEntryExemptions { get; }
        IEnumerable<Version> Versions { get; }
        IEnumerable<DepositTariff> DepositTariffs { get; }
    }

    class BaseReferenceDataVersionTablesProxy : IBaseReferenceDataVersionTables, IDisposable
    {
        private readonly CollectionPrоvider<IBaseReferenceDataVersionTables> _collectionPrоvider;

        public BaseReferenceDataVersionTablesProxy(string fileName)
        {
            _collectionPrоvider = new CollectionPrоvider<IBaseReferenceDataVersionTables>(fileName);
        }

        public IEnumerable<Tariff> Tariffs => _collectionPrоvider.GetCollection<Tariff>();

        public IEnumerable<ProhibitedForManualEntryExemption> ProhibitedForManualEntryExemptions =>
            _collectionPrоvider.GetCollection<ProhibitedForManualEntryExemption>();

        public IEnumerable<Version> Versions => _collectionPrоvider.GetCollection<Version>();

        public IEnumerable<DepositTariff> DepositTariffs => _collectionPrоvider.GetCollection<DepositTariff>();

        public void Dispose()
        {
            _collectionPrоvider.Dispose();
        }
    }

    [Serializable]
    class BaseReferenceDataVersionTables : IBaseReferenceDataVersionTables
    {
        public IEnumerable<Tariff> Tariffs { get; set; }

        public IEnumerable<ProhibitedForManualEntryExemption> ProhibitedForManualEntryExemptions { get; set; }

        public IEnumerable<Version> Versions { get; set; }

        public IEnumerable<DepositTariff> DepositTariffs { get; set; }
    }
}
