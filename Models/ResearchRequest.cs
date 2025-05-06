using System;
using System.Collections.Generic;
namespace PaperNest_API.Models
{
    public class ResearchRequest
    {
        public int Id { get; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string ResearcherName { get; set; }
        public DateTime SubmissionDate { get; }
        public ReviewState State { get; private set; }
        public List<Review> Reviews { get; private set; } = new List<Review>();

        public ResearchRequest(int id, string title, string abstractText, string researcherName)
        {
            Id = id;
            Title = title;
            Abstract = abstractText;
            ResearcherName = researcherName;
            SubmissionDate = DateTime.Now;
            State = new SubmittedState();
        }

        public void ChangeState(ReviewState newState)
        {
            State = newState;
        }

        public void AddReview(Review review)
        {
            Reviews.Add(review);
        }

        public void ProcessReview(ReviewResult result, string reviewerComment = "")
        {
            State.Process(this, result, reviewerComment);
        }
    }
}