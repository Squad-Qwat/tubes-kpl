using PaperNest_API.Models;
using PaperNest_API.Views; // Karena pakai API, ini nggak kepakai
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
    [Route("api/research-requests")]
    public class ResearchRequestController : ControllerBase
    {
        private static List<ResearchRequest> _requests = new List<ResearchRequest>(); //Yakin dibikin constant?
        private static int _nextRequestId = 1;
        private readonly static int _nextReviewId = 1; // Nggak kepakai?

        [HttpPost]
        public IActionResult AddRequest([FromBody] ResearchRequestCreateModel model)
        {
            var newRequest = new ResearchRequest(
                _nextRequestId++,
                model.Title,
                model.AbstractText,
                model.ResearcherName
            );
            _requests.Add(newRequest);

            return CreatedAtAction(nameof(GetRequestById), new { id = newRequest.Id }, new
            {
                message = $"Permintaan riset '{newRequest.Title}' berhasil ditambahkan",
                data = newRequest
            });
        }

        [HttpGet]
        public IActionResult GetAllRequests()
        {
            return Ok(new
            {
                message = "Berhasil mendapatkan semua permintaan riset",
                data = _requests
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetRequestById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "ID permintaan harus lebih dari 0." });
            }

            var request = _requests.FirstOrDefault(r => r.Id == id);

            if (request == null)
            {
                return NotFound(new { message = $"Permintaan dengan ID: {id} tidak ditemukan." });
            }

            return Ok(new
            {
                message = $"Berhasil mendapatkan data permintaan riset '{request.Title}'",
                data = request
            });
        }

        [HttpPut("{id}/start-review")]
        public IActionResult StartReview(int id)
        {
            var request = _requests.FirstOrDefault(r => r.Id == id);

            if (request == null)
            {
                return NotFound(new { message = $"Permintaan riset dengan ID: {id} tidak ditemukan." });
            }

            if (request.State is SubmittedState)
            {
                request.ChangeState(new UnderReviewState());
                return Ok(new
                {
                    message = $"Permintaan riset '{request.Title}' (ID: {request.Id}) sedang ditinjau.",
                    data = request.State.Name
                });
            }
            else
            {
                return BadRequest(new
                {
                    message = $"Permintaan riset '{request.Title}' (ID: {request.Id}) sedang tidak dalam keadaan untuk memulai peninjauan. Keadaan saat ini: {request.State.Name}",
                    data = request.State.Name
                });
            }
        }

        [HttpPut("{id}/process-review")]
        public IActionResult ProcessReview(int id, [FromBody] ReviewRequestModel model)
        {
            var request = _requests.FirstOrDefault(r => r.Id == id);

            if (request == null)
            {
                return NotFound(new { message = $"Permintaan riset dengan ID: {id} tidak ditemukan." });
            }

            if (request.State is UnderReviewState || request.State is NeedsRevisionState)
            {
                request.ProcessReview(model.Result, model.ReviewerComment);
                return Ok(new
                {
                    message = $"Permintaan riset '{request.Title}' (ID: {request.Id}) diproses. Keadaan saat ini: {request.State.Name}",
                    data = request
                });
            }
            else
            {
                return BadRequest(new
                {
                    message = $"Tidak bisa memproses permintaan untuk meninjau '{request.Title}' (ID: {request.Id}) dalam keadaan: {request.State.Name}",
                    data = request.State.Name
                });
            }
        }

        // Menghapus request setelah request ditanggapi oleh sistem/ pemilik workspace
        [HttpDelete("{id}")]
        public IActionResult DeleteRequest(int id)
        {
            var requestToRemove = _requests.FirstOrDefault(r => r.Id == id);

            if (requestToRemove == null)
            {
                return NotFound(new { message = $"Permintaan riset dengan ID: {id} tidak ditemukan." });
            }

            _requests.Remove(requestToRemove);

            return Ok(new
            {
                message = $"Permintaan riset dengan ID: {id} berhasil dihapus."
            });
        }
    }
}