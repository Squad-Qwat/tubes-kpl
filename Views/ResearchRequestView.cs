using PaperNest_API.Models;

namespace PaperNest_API.Views
{
    public class ResearchRequestView
    {
        public void DisplayRequestDetails(ResearchRequest request)
        {
            Console.WriteLine($"ID Permintaan: {request.Id}");
            Console.WriteLine($"Judul: {request.Title}");
            Console.WriteLine($"Abstrak: {request.Abstract}");
            Console.WriteLine($"Periset: {request.ResearcherName}");
            Console.WriteLine($"Tanggal pengumpulan: {request.SubmissionDate}");
            Console.WriteLine($"Status: {request.State.Name}");

            if (request.Reviews.Count > 0)
            {
                Console.WriteLine("\nTinjauan:");
                foreach (var review in request.Reviews)
                {
                    Console.WriteLine($"- ID tinjauan: {review.Id}");
                    Console.WriteLine($"- Peninjau: {review.ReviewerName}");
                    Console.WriteLine($"- Hasil: {review.Result}");
                    Console.WriteLine($"- Komentar: {review.Comment}");
                    Console.WriteLine($"- Tanggal: {review.Created_at}");
                }
            }
            else
            {
                Console.WriteLine("Belum ada tinjauan.");
            }
            Console.WriteLine();
        }

        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}