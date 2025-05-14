using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaperNest_API.Models
{
    public class Review : BaseEntity
    {

        public int ResearchRequestId { get; private set; }
        public string ReviewerName { get; private set; }
        public ReviewResult Result { get; private set; }
        public string Comment { get; private set; }

        public Review(int researchRequestId, string reviewerName, ReviewResult result, string comment)
        {
            ResearchRequestId = researchRequestId;
            ReviewerName = reviewerName;
            Result = result;
            Comment = comment;
            ReviewDate = DateTime.Now;
        }

        public Review(int id, int researchRequestId, string reviewerName, ReviewResult result, string comment) :
            this(researchRequestId, reviewerName, result, comment)
        {
        }

        [ForeignKey("ResearchRequestId")]
        public virtual ResearchRequest ResearchRequest { get; private set; } = null!;

        public virtual ICollection<DocumentBody> DocumentBodies { get; set; } = new List<DocumentBody>();
    }
}