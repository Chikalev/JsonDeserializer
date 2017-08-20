using System;

namespace TestApp.Models
{
    [Serializable]
    public class DepositTariff
    {
        public string Name { get; set; }
        public decimal Tariff { get; set; }
        public int TicketStorageTypeCode { get; set; }
        public string DataChecksum { get; set; }
        public int? DeleteInVersionId { get; set; }
        public int VersionId { get; set; }
        public DateTime ChangedDateTime { get; set; }
    }
}