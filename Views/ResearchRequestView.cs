using PaperNest_API.Models;

namespace PaperNest_API.Views
{
    public class ResearchRequestView
    {
        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void DisplayRequestDetails(ResearchRequest request)
        {
            if (request == null)
            {
                Console.WriteLine("Detail permintaan tidak tersedia.");
                return;
            }

            Console.WriteLine($"\n--- Permintaan Riset: {request.Title} (ID: {request.Id}) ---");
            Console.WriteLine($"Abstrak: {request.Abstract}");
            Console.WriteLine($"Peneliti: {request.ResearcherName}");
            Console.WriteLine($"Tanggal Pengajuan: {request.SubmissionDate.ToShortDateString()}");
            Console.WriteLine($"Keadaan: {request.State.Name}");
            Console.WriteLine($"User ID: {request.UserId}");
            Console.WriteLine($"DocumentBody ID: {request.DocumentBodyId}");

            if (request.Reviews.Any())
            {
                Console.WriteLine("--- Riwayat Review ---");
                foreach (var review in request.Reviews)
                {
                    Console.WriteLine($"- Peninjau: {review.ReviewerName} ({review.UserId}), Hasil: {review.Result}, Komentar: {review.Comment}");
                }
            }
            else
            {
                Console.WriteLine("Belum ada review untuk permintaan ini.");
            }
            Console.WriteLine("-------------------------------------");
        }

        public void DisplayAllRequests(List<ResearchRequest> requests)
        {
            if (requests == null || !requests.Any())
            {
                DisplayMessage("Belum ada permintaan riset yang ditambahkan/ diantrikan.");
                return;
            }

            Console.WriteLine("\n--- Semua permintaan riset ---");
            foreach (var request in requests)
            {
                DisplayRequestDetails(request);
            }
            Console.WriteLine("--- Akhir Daftar Permintaan Riset ---");
        }
    }
}