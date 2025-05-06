using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaperNest_API.Models
{
    public class Workspace
    {
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public Guid User_id { get; set; }

        public bool Is_public { get; set; }

        public DateTime Created_at { get; private set; } = DateTime.Now;

        public DateTime Updated_at { get; set; }

        public ICollection<Document> Documents { get; set; } = new List<Document>();
    }
}
