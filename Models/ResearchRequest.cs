using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaperNest_API.Models
{
    public class ResearchRequest : BaseEntity
    {
        [Required]
        public string Title { get; set; } // Title of the submission

        [Required]
        public string Abstract { get; set; } // Abstract of the submission

        [Required]
        public string ResearcherName { get; set; } // Submitter's name

        public DateTime SubmissionDate { get; private set; } // When it was submitted
        public ReviewState State { get; set; } // Current state of the review process

        [Required]
        public Guid UserId { get; set; } // User who submitted this request

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        // Link to the 'local' Document this request is about
        [Required]
        public Guid DocumentId { get; set; }

        [ForeignKey("DocumentId")]
        public virtual Document Document { get; set; } = null!;

        // Link to the specific DocumentBody being proposed for review (the content of this submission)
        [Required]
        public Guid DocumentBodyId { get; set; } // This is the ID of the DocumentBody that is being submitted

        [ForeignKey("DocumentBodyId")]
        public virtual DocumentBody DocumentBody { get; set; } = null!; // The proposed content version

        public virtual List<Review> Reviews { get; private set; } = new List<Review>(); // Collection of reviews for this request

        // Constructor for Entity Framework
        protected ResearchRequest() { }

        public ResearchRequest(Guid id, string title, string abstractText, string researcherName, Guid userId, Guid documentId, Guid documentBodyId)
        {
            Id = id;
            Title = title;
            Abstract = abstractText;
            ResearcherName = researcherName;
            UserId = userId;
            DocumentId = documentId;
            DocumentBodyId = documentBodyId;
            SubmissionDate = DateTime.Now;
            State = new SubmittedState(); // Initial state
            Created_at = DateTime.Now;
            Updated_at = DateTime.Now;
        }
    }

    // DTO for adding a research request (used when a document is 'pushed' for review)
    public class ResearchRequestDto
    {
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? AbstractText { get; set; }
        [Required]
        public string? ResearcherName { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid DocumentId { get; set; } // The document being submitted
        [Required]
        public Guid DocumentBodyId { get; set; } // The specific content version being submitted
    }

    // DTO for processing a review
    public class ProcessReviewDto
    {
        [Required]
        public ReviewResult Result { get; set; }
        [Required]
        public Guid ReviewerId { get; set; }
        public string? ReviewerComment { get; set; }
    }
}