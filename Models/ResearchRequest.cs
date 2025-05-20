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
        public string Title { get; set; }

        [Required]
        public string Abstract { get; set; }

        [Required]
        public string ResearcherName { get; set; }
        
        public DateTime SubmissionDate { get; private set; }
        public ReviewState State { get; private set; }
        
        [Required]
        public Guid UserId { get; set; }
        
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
        
        [Required]
        public Guid DocumentBodyId { get; set; }
        
        [ForeignKey("DocumentBodyId")]
        public virtual DocumentBody DocumentBody { get; set; } = null!;
        
        public virtual List<Review> Reviews { get; private set; } = new List<Review>();

        // Konstruktor tanpa parameter untuk Entity Framework
        protected ResearchRequest() { }

        public ResearchRequest(Guid id, string title, string abstractText, string researcherName, Guid userId, Guid documentBodyId)
        {
            Id = id;
            Title = title;
            Abstract = abstractText;
            ResearcherName = researcherName;
            UserId = userId;
            DocumentBodyId = documentBodyId;
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