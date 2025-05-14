using PaperNest_API.Models;
using PaperNest_API.Repository;
using PaperNest_API.Views; // Karena pakai API, ini nggak kepakai
using PaperNest_API.Services; // Jika ada service yang digunakan    
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;


namespace PaperNest_API.Controllers
{
    /*
    Sebelum:
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
                {view.DisplayMessage($"Research request '{request.Title}' retrieved successfully.");}
                {view.DisplayRequestDetails(request);}
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

        public void DeleteResolvedRequest(int id)
        {
            var requestToRemove = requests.FirstOrDefault(r => r.Id == id);

            if (requestToRemove == null)
            {
                view.DisplayMessage($"Permintaan riset dengan ID: {id} tidak ditemukan.");
                return;
            }

            {Using ReviewState to confirm it's existence}
            if (requestToRemove.State is ReviewState)
            {
                requests.Remove(requestToRemove);
                view.DisplayMessage($"Permintaan riset dengan ID: {id} yang telah diselesaikan berhasil dihapus.");
            }
            else
            {
                view.DisplayMessage($"Permintaan riset dengan ID: {id} belum diselesaikan dan tidak dapat dihapus melalui metode ini. Keadaan saat ini: {requestToRemove.State?.Name ?? "Tidak diketahui"}");
            }
        }
    }
    */

    [ApiController]
    [Route("api/researchrequests")]
    public class ResearchRequestController : ControllerBase
    {
        // Now uses ResearchRequestManager to interact with requests.
        // The manager internally uses a static list for mock data.
        private readonly ReviewService _reviewManager;

        public ResearchRequestController()
        {
            _reviewManager = new ReviewService();
        }

        // POST: api/researchrequests (This will be called by DocumentController.SubmitDocumentForReview)
        [HttpPost]
        public IActionResult AddRequest([FromBody] ResearchRequestDto newRequestDto)
        {
            if (newRequestDto == null)
            {
                return BadRequest(new { message = "Permintaan tidak valid." });
            }

            // Ensure Document and DocumentBody exist for this submission
            var document = DocumentService.GetById(newRequestDto.DocumentId);
            if (document == null)
            {
                return BadRequest(new { message = $"Dokumen dengan ID {newRequestDto.DocumentId} tidak ditemukan." });
            }

            var documentBody = DocumentService.GetDocumentBodyById(newRequestDto.DocumentBodyId);
            if (documentBody == null || documentBody.DocumentId != newRequestDto.DocumentId)
            {
                return BadRequest(new { message = $"Konten dokumen (DocumentBody) dengan ID {newRequestDto.DocumentBodyId} tidak valid untuk dokumen ini." });
            }

            if(string.IsNullOrWhiteSpace(newRequestDto.Title) || string.IsNullOrWhiteSpace(newRequestDto.AbstractText) || string.IsNullOrWhiteSpace(newRequestDto.ResearcherName)
            {
                return BadRequest(new { message = "Judul, abstrak, dan nama mahasiswa tidak boleh kosong." });
            }

            var newRequest = new ResearchRequest(
                Guid.NewGuid(),
                newRequestDto.Title,
                newRequestDto.AbstractText,
                newRequestDto.ResearcherName,
                newRequestDto.UserId,
                newRequestDto.DocumentId,
                newRequestDto.DocumentBodyId
            );

            _reviewManager.AddResearchRequest(newRequest); // Use manager to add

            return CreatedAtAction(nameof(GetRequestById), new { id = newRequest.Id }, new
            {
                message = "Permintaan riset berhasil ditambahkan",
                data = newRequest
            });
        }

        // GET: api/researchrequests/{id}
        [HttpGet("{id}")]
        public IActionResult GetRequestById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new { message = "ID permintaan tidak valid." });
            }

            var request = _reviewManager.GetResearchRequestById(id); // Use manager to get

            if (request == null)
            {
                return NotFound(new { message = $"Permintaan dengan ID: {id} tidak ditemukan." });
            }

            return Ok(new { message = "Berhasil mendapatkan detail permintaan riset", data = request });
        }

        // GET: api/researchrequests
        [HttpGet]
        public IActionResult GetAllRequests()
        {
            var requests = _reviewManager.GetAllResearchRequests(); // Use manager to get all

            if (requests.Count == 0)
            {
                return Ok(new { message = "Belum ada permintaan riset.", data = new List<ResearchRequest>() });
            }
            return Ok(new { message = "Berhasil mendapatkan semua permintaan riset", data = requests });
        }

        // GET: api/researchrequests/lecturer/{lecturerId}
        [HttpGet("lecturer/{lecturerId}")]
        public IActionResult GetRequestsByLecturer(Guid lecturerId)
        {
            if (lecturerId == Guid.Empty)
            {
                return BadRequest(new { message = "ID dosen tidak valid." });
            }

            // This logic might need to be refined if 'Reviews' only applies to the current request.
            // Assuming Reviews on ResearchRequest still works for lecturer filtering.
            var lecturerRequests = _reviewManager.GetAllResearchRequests()
                                                        .Where(r => r.Reviews.Any(review => review.UserId == lecturerId)).ToList();

            return Ok(new { message = $"Berhasil mendapatkan permintaan riset untuk dosen dengan ID: {lecturerId}", data = lecturerRequests });
        }

        // PUT: api/researchrequests/{requestId}/startreview
        [HttpPut("{requestId}/startreview")]
        public IActionResult StartReview(Guid requestId)
        {
            var request = _reviewManager.GetResearchRequestById(requestId);
            if (request == null)
            {
                return NotFound(new { message = $"Permintaan riset dengan ID: {requestId} tidak ditemukan." });
            }

            if (request.State is SubmittedState)
            {
                _reviewManager.ChangeState(request, new UnderReviewState());
                return Ok(new { message = $"Permintaan riset '{request.Title}' (ID: {request.Id}) sedang ditinjau.", data = request });
            }
            else
            {
                return BadRequest(new { message = $"Permintaan riset '{request.Title}' (ID: {request.Id}) sedang tidak dalam keadaan untuk memulai peninjauan. Keadaan saat ini: {request.State.Name}" });
            }
        }

        // PUT: api/researchrequests/{requestId}/processreview
        [HttpPut("{requestId}/processreview")]
        public IActionResult ProcessReview(Guid requestId, [FromBody] ProcessReviewDto reviewDto)
        {
            if (reviewDto == null)
            {
                return BadRequest(new { message = "Data review tidak valid." });
            }

            var request = _reviewManager.GetResearchRequestById(requestId);
            if (request == null)
            {
                return NotFound(new { message = $"Permintaan riset dengan ID: {requestId} tidak ditemukan." });
            }

            if (!(request.State is UnderReviewState || request.State is NeedsRevisionState))
            {
                return BadRequest(new { message = $"Tidak bisa memproses permintaan untuk meninjau '{request.Title}' (ID: {request.Id}) dalam keadaan: {request.State.Name}" });
            }

            var reviewer = UserRepository.userRepository.FirstOrDefault(u => u.Id == reviewDto.ReviewerId);
            if (reviewer == null)
            {
                return NotFound(new { message = $"Peninjau dengan ID: {reviewDto.ReviewerId} tidak ditemukan." });
            }

            var review = new Review(request.Id, reviewDto.ReviewerId, reviewer.Name, reviewDto.Result, reviewDto.ReviewerComment ?? "");

            _reviewManager.AddReview(request, review);
            _reviewManager.ProcessReview(request, reviewDto.Result, reviewDto.ReviewerComment ?? "");

            return Ok(new { message = "Proses review berhasil.", data = request });
        }
    }
}