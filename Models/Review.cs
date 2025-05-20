using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaperNest_API.Models
{
    public class Review : BaseEntity
    {
        public Guid ResearchRequestId { get; private set; }
        
        [Required]
        public Guid UserId { get; set; }
        
        public string ReviewerName { get; private set; }
        public ReviewResult Result { get; private set; }
        public string Comment { get; private set; }
        public DateTime ReviewDate { get; private set; }

        // Konstruktor tanpa parameter untuk Entity Framework
        protected Review() { }

        public Review(Guid researchRequestId, Guid userId, string reviewerName, ReviewResult result, string comment)
        {
            ResearchRequestId = researchRequestId;
            UserId = userId;
            ReviewerName = reviewerName;
            Result = result;
            Comment = comment;
            ReviewDate = DateTime.Now;
        }

        public Review(Guid id, Guid researchRequestId, Guid userId, string reviewerName, ReviewResult result, string comment) :
            this(researchRequestId, userId, reviewerName, result, comment)
        {
            Id = id;
            ReviewDate = DateTime.Now;
        }

        [ForeignKey("ResearchRequestId")]
        public virtual ResearchRequest ResearchRequest { get; private set; } = null!;
        
        [ForeignKey("UserId")]
        public virtual User User { get; private set; } = null!;

        public virtual ICollection<DocumentBody> DocumentBodies { get; set; } = new List<DocumentBody>();
    }
}