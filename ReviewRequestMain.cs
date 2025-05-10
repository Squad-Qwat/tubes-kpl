using PaperNest_API.Controllers;
using PaperNest_API.Models;

namespace PaperNest_API
{
    public class ReviewRequestMain
    {
        public static void Main(string[] args)
        {
            ResearchRequestController controller = new ResearchRequestController();

            // Simulate user interactions
            controller.AddRequest("Algoritma Novel untuk Image Recognition", "Makalah ini mengusulkan algoritma baru...", "Dr. Alice Smith");
            controller.AddRequest("Dampak perubahan iklim pada ekosistem pesisir", "Sebuah investigasi mengenai...", "Prof. Bob Johnson");

            controller.DisplayAllRequests();

            controller.StartReview(1);
            controller.ProcessReview(1, ReviewResult.Approved, "Kerja bagus!");

            controller.StartReview(2);
            controller.ProcessReview(2, ReviewResult.NeedsRevision, "Tolong jelaskan metodologinya.");
            controller.ProcessReview(2, ReviewResult.Approved, "Revisi telah menyelesaikan permasalahannya.");

            controller.DisplayAllRequests();
        }
    }
}
