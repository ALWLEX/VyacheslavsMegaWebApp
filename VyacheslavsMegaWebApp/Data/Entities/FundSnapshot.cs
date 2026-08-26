using System.ComponentModel.DataAnnotations.Schema;

namespace VyacheslavsMegaWebApp.Data.Entities
{
    public class FundSnapshot : Base.EntityBase
    {
        public DateTime SnapshotDate { get; private set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Balance { get; set; }
    }
}
