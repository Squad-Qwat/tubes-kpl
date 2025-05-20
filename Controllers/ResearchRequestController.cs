using PaperNest_API.Models;
using PaperNest_API.Views;
using PaperNest_API.Repository;

namespace PaperNest_API.Controllers
{
    public class ResearchRequestController
    {
        private List<ResearchRequest> requests = new List<ResearchRequest>();
        private ResearchRequestView view = new ResearchRequestView();

        public void AddRequest(string title, string abstractText, string researcherName, Guid userId, Guid documentBodyId)
        {
            var newRequest = new ResearchRequest(Guid.NewGuid(), title, abstractText, researcherName, userId, documentBodyId);
            requests.Add(newRequest);
            view.DisplayMessage($"Permintaan riset '{newRequest.Title}' berhasil ditambahkan dengan ID: {newRequest.Id}");
        }

        public ResearchRequest? GetRequestById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    throw new ArgumentNullException(nameof(id), "ID permintaan tidak valid.");
                }
                
                ResearchRequest? request = requests.FirstOrDefault(r => r.Id == id);
                
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), $"Permintaan dengan ID: {id} tidak ada.");
                }
                
                return request;
            }
            catch (ArgumentNullException ex)
            {
                view.DisplayMessage($"Terjadi kesalahan saat menerima permintaan dengan ID: {id} dengan pesan: {ex.Message}");
                return null;
            }
        }

        public void StartReview(Guid requestId)
        {
            var request = GetRequestById(requestId);
            if (request != null)
            {
                if (request.State is SubmittedState)
                {
                    request.ChangeState(new UnderReviewState());
                    view.DisplayMessage($"Permintaan riset '{request.Title}' (ID: {request.Id}) sedang ditinjau.");
                }
                else
                {
                    view.DisplayMessage($"Permintaan riset '{request.Title}' (ID: {request.Id}) sedang tidak dalam keadaan untuk memulai peninjauan. Keadaan saat ini: {request.State.Name}");
                }
            }
            else
            {
                view.DisplayMessage($"Permintaan riset dengan ID: {requestId} tidak ada.");
            }
        }

        public void ProcessReview(Guid requestId, ReviewResult result, Guid reviewerId, string reviewerComment = "")
        {
            var request = GetRequestById(requestId);
            if (request != null && (request.State is UnderReviewState || request.State is NeedsRevisionState))
            {
                // Dapatkan reviewer dari repository
                var reviewer = UserRepository.userRepository.FirstOrDefault(u => u.Id == reviewerId);
                
                if (reviewer == null)
                {
                    view.DisplayMessage($"Peninjau dengan ID: {reviewerId} tidak ditemukan.");
                    return;
                }
                
                // Buat objek review
                var review = new Review(request.Id, reviewerId, reviewer.Name, result, reviewerComment);
                
                // Tambahkan review ke request
                request.AddReview(review);
                
                // Proses review
                request.ProcessReview(result, reviewerComment);
                
                view.DisplayRequestDetails(request);
            }
            else if (request != null)
            {
                view.DisplayMessage($"Tidak bisa memproses permintaan untuk meninjau '{request.Title}' (ID: {request.Id}) dalam keadaan: {request.State.Name}");
            }
            else
            {
                view.DisplayMessage($"Permintaan riset dengan ID: {requestId} tidak ada.");
            }
        }

        public void DisplayRequest(Guid requestId)
        {
            var request = GetRequestById(requestId);
            if (request != null)
            {
                view.DisplayRequestDetails(request);
            }
            else
            {
                view.DisplayMessage($"Permintaan riset dengan ID: {requestId} tidak ada.");
            }
        }

        public void DisplayAllRequests()
        {
            if (requests.Count > 0)
            {
                Console.WriteLine("--- Semua permintaan riset ---");
                foreach (var request in requests)
                {
                    view.DisplayRequestDetails(request);
                }
            }
            else
            {
                view.DisplayMessage("Belum ada permintaan riset yang ditambahkan/ diantrikan.");
            }
        }
        
        public List<ResearchRequest> GetRequestsByLecturer(Guid lecturerId)
        {
            // Dapatkan semua requests yang memiliki reviews dengan lecturer sebagai reviewer
            return requests.Where(r => r.Reviews.Any(review => review.UserId == lecturerId)).ToList();
        }

        public List<ResearchRequest> GetAllRequests()
        {
            return requests;
        }
    }
}