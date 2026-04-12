using Microsoft.AspNetCore.Mvc;
using PaperNest_API.Controllers;
using PaperNest_API.Models;
using PaperNest_API.Repository;
using PaperNest_API.View;
using System.Linq;

namespace PaperNest_API
{
    public class ReviewRequestMain
    {
        public static void Main(string[] args)
        {
            // Instantiate the ResearchRequestManager and pass it to the controller
            // In a real ASP.NET Core app, this would be handled by Dependency Injection (DI)
            var researchRequestManager = new PaperNest_API.Services.ReviewService();
            var controller = new PaperNest_API.Controllers.ResearchRequestController(); // Controller now has a default constructor

            var consoleView = new PaperNest_API.Views.ResearchRequestView();

            // Getting existing user IDs (assuming UserRepository exists with static data)
            Guid userId1 = PaperNest_API.Repository.UserRepository.userRepository.FirstOrDefault(u => u.Role == "Mahasiswa")?.Id ?? Guid.NewGuid();
            Guid userId2 = PaperNest_API.Repository.UserRepository.userRepository.LastOrDefault(u => u.Role == "Mahasiswa")?.Id ?? Guid.NewGuid(); // Get a different student ID
            if (userId1 == userId2) // Ensure they are different if only one student exists
            {
                userId2 = Guid.NewGuid();
            }

            // Getting lecturer for reviewer
            Guid lecturerId = PaperNest_API.Repository.UserRepository.userRepository.FirstOrDefault(u => u.Role == "Dosen")?.Id ?? Guid.NewGuid();

            // Getting DocumentBody (mocking IDs for this console test)
            Guid docBody1 = Guid.NewGuid();
            Guid docBody2 = Guid.NewGuid();

            Console.WriteLine("--- Menambahkan Permintaan Riset ---");
            // Making research requests using DTOs
            controller.AddRequest(new ResearchRequestDto { Title = "Algoritma Novel untuk Image Recognition", AbstractText = "Makalah ini mengusulkan algoritma baru...", ResearcherName = "Dr. Alice Smith", UserId = userId1, DocumentBodyId = docBody1 });
            controller.AddRequest(new ResearchRequestDto { Title = "Dampak perubahan iklim pada ekosistem pesisir", AbstractText = "Sebuah investigasi mengenai...", ResearcherName = "Prof. Bob Johnson", UserId = userId2, DocumentBodyId = docBody2 });

            Console.WriteLine("\n--- Menampilkan Semua Permintaan Riset Awal ---");
            // Extracting data from IActionResult for console display
            var allRequestsResult = controller.GetAllRequests();
            if (allRequestsResult is OkObjectResult okResult && okResult.Value is { } value && value.GetType().GetProperty("data")?.GetValue(value) is List<ResearchRequest> initialRequests)
            {
                consoleView.DisplayAllRequests(initialRequests);
            }
            else
            {
                consoleView.DisplayMessage("Gagal mendapatkan semua permintaan riset awal.");
            }

            // Using IDs from the added requests
            var currentRequestsResult = controller.GetAllRequests();
            if (currentRequestsResult is OkObjectResult okCurrentResult && okCurrentResult.Value is { } currentValue && currentValue.GetType().GetProperty("data")?.GetValue(currentValue) is List<ResearchRequest> currentRequests && currentRequests.Count >= 2)
            {
                Guid requestId1 = currentRequests[0].Id;
                Guid requestId2 = currentRequests[1].Id;

                Console.WriteLine($"\n--- Memulai dan Memproses Review untuk Request ID: {requestId1} ---");
                controller.StartReview(requestId1);
                controller.ProcessReview(requestId1, new ProcessReviewDto { Result = ReviewResult.Approved, ReviewerId = lecturerId, ReviewerComment = "Kerja bagus!" });

                Console.WriteLine($"\n--- Memulai dan Memproses Review untuk Request ID: {requestId2} ---");
                controller.StartReview(requestId2);
                controller.ProcessReview(requestId2, new ProcessReviewDto { Result = ReviewResult.NeedsRevision, ReviewerId = lecturerId, ReviewerComment = "Tolong jelaskan metodologinya." });
                controller.ProcessReview(requestId2, new ProcessReviewDto { Result = ReviewResult.Approved, ReviewerId = lecturerId, ReviewerComment = "Revisi telah menyelesaikan permasalahannya." });
            }
            else
            {
                consoleView.DisplayMessage("Tidak cukup permintaan riset untuk melakukan pengujian review.");
            }

            Console.WriteLine("\n--- Menampilkan Semua Permintaan Riset Setelah Review ---");
            var finalRequestsResult = controller.GetAllRequests();
            if (finalRequestsResult is OkObjectResult okFinalResult && okFinalResult.Value is { } finalValue && finalValue.GetType().GetProperty("data")?.GetValue(finalValue) is List<ResearchRequest> finalRequests)
            {
                consoleView.DisplayAllRequests(finalRequests);
            }
            else
            {
                consoleView.DisplayMessage("Gagal mendapatkan semua permintaan riset akhir.");
            }
            Console.WriteLine("\n--- Pengujian Selesai ---");
        }
    }
}
