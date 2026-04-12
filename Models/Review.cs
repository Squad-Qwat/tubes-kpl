// PaperNest_API.Models.Review.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaperNest_API.Models
{
    // Review (feedback) on a ResearchRequest
    public class Review : BaseEntity
    {
        public Guid ResearchRequestId { get; private set; } // Links to the specific ResearchRequest being reviewed

        [Required]
        public Guid UserId { get; set; } // The reviewer's ID

        public string ReviewerName { get; private set; } // Name of the reviewer
        public ReviewResult Result { get; private set; } // Outcome of the review
        public string Comment { get; private set; } // Reviewer's comments
        public DateTime ReviewDate { get; private set; }

        public ReviewState State { get; private set; } // Current state of the review process

        // Constructor for EF
        protected Review() { }

        public Review(Guid researchRequestId, Guid userId, string reviewerName, ReviewResult result, string comment)
        {
            Id = Guid.NewGuid(); // BaseEntity already handles Id, but explicitly set for clarity.
            ResearchRequestId = researchRequestId;
            UserId = userId;
            ReviewerName = reviewerName;
            Result = result;
            Comment = comment;
            ReviewDate = DateTime.Now;
            Created_at = DateTime.Now; // Set by BaseEntity, but good to be explicit for new obj
            Updated_at = DateTime.Now;
        }

        [ForeignKey("ResearchRequestId")]
        public virtual ResearchRequest ResearchRequest { get; private set; } = null!;

        [ForeignKey("UserId")]
        public virtual User User { get; private set; } = null!;
    }

    // Review related DTOs (if needed, but mostly ProcessReviewDto is used by ResearchRequest)
    public class ReviewRequestModel // Used for input when submitting a review, e.g., in a CLI
    {
        public ReviewResult Result { get; set; }
        public string? ReviewerComment { get; set; }
    }
}