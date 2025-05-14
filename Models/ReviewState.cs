using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc;
using PaperNest_API.Services;

namespace PaperNest_API.Models
{
    /*
     * 'override' keyword hanya digunakan untuk superclass (termasuk abstract class) di C#, contoh:
        public abstract class ReviewState
        {
            public abstract string Name { get; }
            public abstract void Process(ResearchRequest request, ReviewResult result, string reviewerComment);
        }
        Pada interface, kita tidak bisa menggunakan keyword 'override' karena interface tidak memiliki implementasi.
    */
    public interface ReviewState
    {
        string Name { get; }
        void Process(ResearchRequest request, ReviewResult result, string reviewerComment);
    }

    public class SubmittedState : ReviewState
    {
        public string Name => "Submitted";
        public void Process(ResearchRequest request, ReviewResult result, string reviewerComment)
        {
            if (result != ReviewResult.Pending)
            {
                Console.WriteLine($"Error: Cannot directly set to {result} from {Name}. Requires review first.");
            }
        }
    }
    public class UnderReviewState : ReviewState
    {
        public string Name => "Under Review";

        public void Process(ResearchRequest request, ReviewResult result, string reviewerComment)
        {
            ReviewService manager = new(); // Setara dengan 'new  ReviewService()'
            manager.AddReview(request, new Review(Guid.NewGuid(), request.Id, "Reviewer", result, reviewerComment));

            switch (result)
            {
                case ReviewResult.Approved:
                    manager.ChangeState(request, new ApprovedState());
                    break;
                case ReviewResult.NeedsRevision:
                    manager.ChangeState(request, new NeedsRevisionState());
                    break;
                default:
                    Console.WriteLine("Review result is still pending.");
                    break;
            }
        }
    }
    public class ApprovedState : ReviewState
    {
        public string Name => "Approved";
        public void Process(ResearchRequest request, ReviewResult result, string reviewerComment)
        {
            Console.WriteLine("Permintaan peninjauan telah disetujui.");
        }
    }

    public class RejectedState : ReviewState
    {
        public string Name => "Rejected";
        public void Process(ResearchRequest request, ReviewResult result, string reviewerComment)
        {
            Console.WriteLine("Permintaan peninjauan telah ditolak.");
        }
    }

    public class NeedsRevisionState : ReviewState
    {
        public string Name => "Needs Revision";
        public void Process(ResearchRequest request, ReviewResult result, string reviewerComment)
        {
            ReviewService manager = new(); // Setara dengan 'new  ReviewService()'
            if (result == ReviewResult.Approved)
            {
                manager.ChangeState(request, new ApprovedState());
            } 
            else
            {
                Console.WriteLine($"Research request is still under revision or received another review result: {result}");
            }
        }
    }
}