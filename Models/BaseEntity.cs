using System.ComponentModel.DataAnnotations;

namespace PaperNest_API.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Updated_at { get; set; }
    }
}