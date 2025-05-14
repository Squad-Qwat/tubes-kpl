using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaperNest_API.Models
{
    public class Workspace : BaseEntity
    {

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public Guid User_id { get; set; }
        
        [ForeignKey("User_id")]
        public virtual User User { get; set; } = null!;

        public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
        
        // Koleksi untuk UserWorkspace
        public virtual ICollection<UserWorkspace> UserWorkspaces { get; set; } = new List<UserWorkspace>();
    }
}
