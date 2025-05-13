using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaperNest_API.Models
{
    public class DocumentBody : BaseEntity
    {

        [Required]
        public bool IsCurrentVersion { get; set; }

        [Required] 
        public string Content { get; set; } = string.Empty;

        public string VersionDescription { get; set; } = string.Empty;

        public Guid ReviewId { get; set; } = Guid.Empty;
        
        public Guid DocumentId { get; set; } 
        
        public bool IsReviewed { get; set; } = false;
        
        public ReviewResult? ReviewResult { get; set; }

        [ForeignKey("ReviewId")]
        public virtual Review Review { get; set; } = null!;
        
        [ForeignKey("DocumentId")]
        public virtual Document Document { get; set; } = null!;
    }
}
