using PaperNest_API.Models;

namespace PaperNest_API.Services
{
    public class ReviewService
    {
        // This static list will act as a mock "repository" for ResearchRequests for the console test
        // In a real application, this would be an injected IResearchRequestRepository.
        private static readonly List<ResearchRequest> _researchRequests = []; // Setara dengan 'new List<ResearchRequest>()'

        public void AddResearchRequest(ResearchRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Research request cannot be null.");
            }
            _researchRequests.Add(request);
        }

        public ResearchRequest? GetResearchRequestById(Guid id)
        {
            return _researchRequests.FirstOrDefault(r => r.Id == id);
        }

        public List<ResearchRequest> GetAllResearchRequests()
        {
            return _researchRequests;
        }

        public void ChangeState(ResearchRequest request, ReviewState newState)
        {
            request.State = newState;
        }

        public void AddReview(ResearchRequest request, Review review)
        {
            request.Reviews.Add(review);
        }

        public void ProcessReview(ResearchRequest request, ReviewResult result, string reviewerComment = "")
        {
            request.State.Process(request, result, reviewerComment);

            // After processing a review, if approved, update the Document's current DocumentBody
            if (request.State is ApprovedState)
            {
                // This is the "merge" or "accept pull request" part of the Git analogy
                // The DocumentService should update the Document's CurrentDocumentBodyId
                // to the DocumentBody that was submitted in this ResearchRequest.
                var document = DocumentService.GetById(request.DocumentId);
                if (document != null)
                {
                    // Invalidate previous current version of the Document
                    var previousCurrent = DocumentService.GetVersions(document.Id)
                                                         .FirstOrDefault(db => db.IsCurrentVersion);
                    if (previousCurrent != null)
                    {
                        previousCurrent.IsCurrentVersion = false;
                    }

                    // Set the submitted DocumentBody as the new CurrentDocumentBody for the Document
                    var submittedBody = DocumentService.GetDocumentBodyById(request.DocumentBodyId);
                    if (submittedBody != null)
                    {
                        submittedBody.IsCurrentVersion = true;
                        document.CurrentDocumentBodyId = submittedBody.Id;
                        document.LocalContentDraft = null; // Clear draft after merge
                        document.HasDraft = false;
                        document.Updated_at = DateTime.Now;
                        // No need to call DocumentService.Update here as DocumentService operates on its own static list.
                    }
                }
            }
            else if (request.State is RejectedState)
            {
                Console.WriteLine($"Research request {request.Id} has been rejected.");
            }
            else if (request.State is NeedsRevisionState)
            {
                Console.WriteLine($"Research request {request.Id} needs revision.");
            }
        }
    }
}
