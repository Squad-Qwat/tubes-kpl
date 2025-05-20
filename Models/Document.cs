using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaperNest_API.Models
{
    public class Document : BaseEntity
    {

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string? Content { get; set; }

        [Required]
        public Guid User_id { get; set; }
        [ForeignKey("User_id")]
        public virtual User User { get; set; } = null!;

        [Required]
        public Guid Workspace_id { get; set; }
        [ForeignKey("Workspace_id")]
        public virtual Workspace Workspace { get; set; } = null!;
        
        // Tambahkan properti untuk melacak pengeditan
        public Guid? LastEditedByUserId { get; set; }
        public DateTime? LastEditedAt { get; set; }
        public bool HasDraft { get; set; } = false;
        
        // Koleksi versi dokumen
        public virtual ICollection<DocumentBody> DocumentVersions { get; set; } = new List<DocumentBody>();
    }
}
