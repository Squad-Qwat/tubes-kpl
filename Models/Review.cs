using Microsoft.AspNetCore.Mvc;

namespace PaperNest_API.Models
{
    public class Review
    {
        public int Id { get; }
        public int ResearchRequestId { get; }
        public string ReviewerName { get; }
        public ReviewResult Result { get; }
        public string Comment { get; }
        public DateTime ReviewDate { get; }

        public Review(int id, int researchRequestId, string reviewerName, ReviewResult result, string comment)
        {
            Id = id;
            ResearchRequestId = researchRequestId;
            ReviewerName = reviewerName;
            Result = result;
            Comment = comment;
            ReviewDate = DateTime.Now;
        }
    }
}