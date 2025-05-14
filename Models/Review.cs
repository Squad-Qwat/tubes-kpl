using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaperNest_API.Models
{
    public class Review : BaseEntity
    {

        public Guid ResearchRequestId { get; private set; }
        public string ReviewerName { get; private set; }
        public ReviewResult Result { get; private set; }
        public string Comment { get; private set; }
        public DateTime ReviewDate { get; private set; }

        public Review(Guid researchRequestId, string reviewerName, ReviewResult result, string comment)
        {
            ResearchRequestId = researchRequestId;
            ReviewerName = reviewerName;
            Result = result;
            Comment = comment;
            ReviewDate = DateTime.Now;
        }

        public Review(Guid id, Guid researchRequestId, string reviewerName, ReviewResult result, string comment) :
            this(researchRequestId, reviewerName, result, comment)
        {
            ReviewDate = DateTime.Now;
        }

        [ForeignKey("ResearchRequestId")]
        public virtual ResearchRequest ResearchRequest { get; private set; } = null!;

        public virtual ICollection<DocumentBody> DocumentBodies { get; set; } = new List<DocumentBody>();
    }
}