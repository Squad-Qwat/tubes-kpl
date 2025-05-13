using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaperNest_API.Models
{
    public class Workspace
    {
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public Guid User_id { get; set; }
        
        [ForeignKey("User_id")]
        public virtual User User { get; set; } = null!;

        public DateTime Created_at { get; private set; } = DateTime.Now;

        public DateTime Updated_at { get; set; }

        public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
        
        // Koleksi untuk UserWorkspace
        public virtual ICollection<UserWorkspace> UserWorkspaces { get; set; } = new List<UserWorkspace>();
    }
}
