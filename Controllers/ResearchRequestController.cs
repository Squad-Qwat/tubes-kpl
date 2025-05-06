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
            view.DisplayMessage($"Research request '{newRequest.Title}' submitted successfully with ID: {newRequest.Id}");
        }

        public ResearchRequest GetRequestById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentNullException(nameof(id), "Request ID must be greater than zero.");
                }
                var request = requests.FirstOrDefault(r => r.Id == id);
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), $"Request with ID: {id} not found.");
                }
                // view.DisplayMessage($"Research request '{request.Title}' retrieved successfully.");
                // view.DisplayRequestDetails(request);
                return request;
            }
            catch (ArgumentNullException ex)
            {
                view.DisplayMessage($"Error retrieving request with ID: {id}. Exception: {ex.Message}");
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
                    view.DisplayMessage($"Research request '{request.Title}' (ID: {request.Id}) is now under review.");
                }
                else
                {
                    view.DisplayMessage($"Research request '{request.Title}' (ID: {request.Id}) is not in a state to start review. Current state: {request.State.Name}");
                }
            }
            else
            {
                view.DisplayMessage($"Research request with ID: {requestId} not found.");
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
                view.DisplayMessage($"Cannot process review for request '{request.Title}' (ID: {request.Id}) in state: {request.State.Name}");
            }
            else
            {
                view.DisplayMessage($"Research request with ID: {requestId} not found.");
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
                view.DisplayMessage($"Research request with ID: {requestId} not found.");
            }
        }

        public void DisplayAllRequests()
        {
            if (requests.Count > 0)
            {
                Console.WriteLine("--- All Research Requests ---");
                foreach (var request in requests)
                {
                    view.DisplayRequestDetails(request);
                }
            }
            else
            {
                view.DisplayMessage("No research requests submitted yet.");
            }
        }
    }
}