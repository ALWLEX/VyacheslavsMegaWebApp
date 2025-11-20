using System.ComponentModel.DataAnnotations.Schema;

namespace VyacheslavsMegaServer.Data.Entities
{
    public class FundSnapshot : Base.EntityBase
    {
        public DateTime SnapshotDate { get; private set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Balance { get; private set; }
    }
}
