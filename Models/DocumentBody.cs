using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaperNest_API.Models
{
    public class DocumentBody : BaseEntity
    {
        [Required]
        public string Content { get; set; }

        public string VersionDescription { get; set; } = "Initial version"; // Like a commit message
        public bool IsCurrentVersion { get; set; } // Indicates if this DocumentBody is the active content for a Document

        public bool IsReviewed { get; set; } // Indicates if this DocumentBody has been reviewed

        [Required]
        public ReviewResult? ReviewResult { get; set; } // The result of the review, if applicable

        [Required]
        public Guid ReviewId { get; set; } // Foreign key to the Review this body is associated with
        [ForeignKey("ReviewId")]
        public virtual Review? Review { get; set; } // Nullable, as not all DocumentBodies may be linked to a review

        // Foreign key to the Document this body belongs to
        [Required]
        public Guid DocumentId { get; set; }

        [ForeignKey("DocumentId")]
        public virtual Document Document { get; set; } = null!;

        // Constructor for EF
        protected DocumentBody() { }

        public DocumentBody(string content, Guid documentId, string? versionDescription = null)
        {
            Id = Guid.NewGuid();
            Content = content;
            DocumentId = documentId;
            VersionDescription = versionDescription ?? "New version";
            IsCurrentVersion = true; // By default, newly created is current
        }
    }
}