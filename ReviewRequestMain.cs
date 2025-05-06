using PaperNest_API.Controllers;
using PaperNest_API.Models;

namespace PaperNest_API
{
    public class ReviewRequestMain
    {
        public static void Main(string[] args)
        {
            var controller = new ResearchRequestController();

            // Simulate user interactions
            controller.AddRequest("Novel Algorithm for Image Recognition", "This paper proposes a new algorithm...", "Dr. Alice Smith");
            controller.AddRequest("Impact of Climate Change on Coastal Ecosystems", "An investigation into...", "Prof. Bob Johnson");

            controller.DisplayAllRequests();

            controller.StartReview(1);
            controller.ProcessReview(1, ReviewResult.Approved, "Excellent work!");

            controller.StartReview(2);
            controller.ProcessReview(2, ReviewResult.NeedsRevision, "Please elaborate on the methodology.");
            controller.ProcessReview(2, ReviewResult.Approved, "Revisions addressed the concerns.");

            controller.DisplayAllRequests();
        }
    }
}
