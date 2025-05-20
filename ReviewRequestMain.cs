using PaperNest_API.Controllers;
using PaperNest_API.Models;
using PaperNest_API.Repository;
using System.Linq;

namespace PaperNest_API
{
    public class ReviewRequestMain
    {
        public static void Main(string[] args)
        {
            ResearchRequestController controller = new(); // Setara dengan 'new ResearchRequestController()'
            
            // Mendapatkan ID pengguna yang ada
            Guid userId1 = UserRepository.userRepository.FirstOrDefault(u => u.Role == "Mahasiswa")?.Id ?? Guid.NewGuid();
            Guid userId2 = UserRepository.userRepository.FirstOrDefault(u => u.Role == "Mahasiswa" && u.Id != userId1)?.Id ?? Guid.NewGuid();
            
            // Mendapatkan dosen untuk reviewer
            Guid lecturerId = UserRepository.userRepository.FirstOrDefault(u => u.Role == "Dosen")?.Id ?? Guid.NewGuid();
            
            // Mendapatkan DocumentBody
            Guid docBody1 = Guid.NewGuid();
            Guid docBody2 = Guid.NewGuid();
            
            // Membuat permintaan penelitian
            controller.AddRequest("Algoritma Novel untuk Image Recognition", "Makalah ini mengusulkan algoritma baru...", "Dr. Alice Smith", userId1, docBody1);
            controller.AddRequest("Dampak perubahan iklim pada ekosistem pesisir", "Sebuah investigasi mengenai...", "Prof. Bob Johnson", userId2, docBody2);

            controller.DisplayAllRequests();

            // Menggunakan ID dari requests
            var allRequests = controller.GetAllRequests();
            if (allRequests.Count >= 2)
            {
                Guid requestId1 = allRequests[0].Id;
                Guid requestId2 = allRequests[1].Id;
                
                controller.StartReview(requestId1);
                controller.ProcessReview(requestId1, ReviewResult.Approved, lecturerId, "Kerja bagus!");

                controller.StartReview(requestId2);
                controller.ProcessReview(requestId2, ReviewResult.NeedsRevision, lecturerId, "Tolong jelaskan metodologinya.");
                controller.ProcessReview(requestId2, ReviewResult.Approved, lecturerId, "Revisi telah menyelesaikan permasalahannya.");
            }

            controller.DisplayAllRequests();
        }
    }
}
