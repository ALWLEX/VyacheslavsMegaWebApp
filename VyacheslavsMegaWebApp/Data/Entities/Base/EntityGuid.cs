using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VyacheslavsMegaWebApp.Data.Entities.Base
{
    public abstract class EntityGuid
    {
        [Key]
        [Column(Order = 0)]
        public Guid Id { get; set; }
    }
}
