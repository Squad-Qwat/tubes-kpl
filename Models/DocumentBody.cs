using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaperNest_API.Models
{
    public class DocumentBody
    {
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Required]
        public bool IsCurrentVersion { get; set; }

        [Required] 
        public string Content { get; set; } = string.Empty;

        public string VersionDescription { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Guid ReviewId { get; set; } 
        
        public Guid DocumentId { get; set; } 

        
        [ForeignKey("ReviewId")]
        public virtual Review Review { get; set; } = null!;
        
        [ForeignKey("DocumentId")]
        public virtual Document Document { get; set; } = null!;
    }
}
