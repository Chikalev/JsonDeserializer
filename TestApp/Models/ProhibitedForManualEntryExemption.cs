using System;

namespace TestApp.Models
{
    [Serializable]
    class ProhibitedForManualEntryExemption
    {
        public int? RegionCode { get; set; }
        public int Code { get; set; }
        public int ExemptionCode { get; set; }
        public int? RegionOkatoCode { get; set; }
        public string DataChecksum { get; set; }
        public int? DeleteInVersionId { get; set; }
        public int VersionId { get; set; }
        public DateTime ChangedDateTime { get; set; }
    }
}
