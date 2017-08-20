using System;

namespace TestApp.Models
{
    [Serializable]
    class Version
    {
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime StartingDateTime { get; set; }
        public bool IsCriticalChange { get; set; }
        public int VersionId { get; set; }
        public int? Status { get; set; }
        public bool IsSeed { get; set; }
        public string DataChecksum { get; set; }
    }
}
