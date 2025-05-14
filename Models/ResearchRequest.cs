using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PaperNest_API.Models
{
    public class ResearchRequest : BaseEntity
    {

        [Required]
        public string Title { get; set; }

        [Required]
        public string Abstract { get; set; }

        [Required]
        public string ResearcherName { get; set; }
        public DateTime SubmissionDate { get; private set; }
        public ReviewState State { get; private set; }
        public virtual List<Review> Reviews { get; private set; } = new List<Review>();


        public ResearchRequest(Guid id, string title, string abstractText, string researcherName)
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