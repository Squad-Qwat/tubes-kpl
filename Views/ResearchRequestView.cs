using PaperNest_API.Models;

namespace PaperNest_API.Views
{
    public class ResearchRequestView
    {
        public void DisplayRequestDetails(ResearchRequest request)
        {
            Console.WriteLine($"Request ID: {request.Id}");
            Console.WriteLine($"Title: {request.Title}");
            Console.WriteLine($"Abstract: {request.Abstract}");
            Console.WriteLine($"Researcher: {request.ResearcherName}");
            Console.WriteLine($"Submission Date: {request.SubmissionDate}");
            Console.WriteLine($"Status: {request.State.Name}");

            if (request.Reviews.Count > 0)
            {
                Console.WriteLine("\nReviews:");
                foreach (var review in request.Reviews)
                {
                    Console.WriteLine($"- Review ID: {review.Id}");
                    Console.WriteLine($"  Reviewer: {review.ReviewerName}");
                    Console.WriteLine($"  Result: {review.Result}");
                    Console.WriteLine($"  Comment: {review.Comment}");
                    Console.WriteLine($"  Date: {review.ReviewDate}");
                }
            }
            else
            {
                Console.WriteLine("No reviews yet.");
            }
            Console.WriteLine();
        }

        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}