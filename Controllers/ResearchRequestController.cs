using PaperNest_API.Models;
using PaperNest_API.Views;

namespace PaperNest_API.Controllers
{
    public class ResearchRequestController
    {
        private List<ResearchRequest> requests = new List<ResearchRequest>();
        private ResearchRequestView view = new ResearchRequestView();
        private int nextRequestId = 1;
        private int nextReviewId = 1;

        public void AddRequest(string title, string abstractText, string researcherName)
        {
            var newRequest = new ResearchRequest(nextRequestId++, title, abstractText, researcherName);
            requests.Add(newRequest);
            view.DisplayMessage($"Permintaan riset '{newRequest.Title}' berhasil ditambahkan dengan ID: {newRequest.Id}");
        }

        public ResearchRequest? GetRequestById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentNullException(nameof(id), "IS permintaan harus lebih dari 0.");
                }
                ResearchRequest? request = requests.FirstOrDefault(r => r.Id == id);
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), $"Permintaan dengan ID: {id} tidak ada.");
                }
                // view.DisplayMessage($"Research request '{request.Title}' retrieved successfully.");
                // view.DisplayRequestDetails(request);
                return request;
            }
            catch (ArgumentNullException ex)
            {
                view.DisplayMessage($"Terjadi kesalahan saat menerima permintaan dengan ID: {id} dengan pesan: {ex.Message}");
                return null;
            }
        }

        public void StartReview(int requestId)
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

        public void ProcessReview(int requestId, ReviewResult result, string reviewerComment = "")
        {
            var request = GetRequestById(requestId);
            if (request != null && request.State is UnderReviewState || request.State is NeedsRevisionState)
            {
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

        public void DisplayRequest(int requestId)
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
    }
}