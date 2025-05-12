using Microsoft.AspNetCore.Mvc;
using PaperNest_API.Models;
using PaperNest_API.Services;

namespace PaperNest_API.Controllers
{
    [ApiController, Route("api/documents")]
    public class DocumentController : Controller
    {
        [HttpGet]
        public IActionResult GetAllDocuments()
        {
            var documents = DocumentService.GetAll();

            return Ok(new
            {
                message = "Berhasil mendapatkan semua dokumen",
                data = documents
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetDocumentById(Guid id)
        {
            var document = DocumentService.GetById(id);

            if (document == null)
            {
                return NotFound(new
                {
                    message = "Dokumen tidak ditemukan"
                });
            }

            return Ok(new
            {
                message = "Berhasil mendapatkan data dokumen",
                data = document
            });
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetDocumentsByUserId(Guid userId)
        {
            var documents = DocumentService.GetByUserId(userId);

            return Ok(new
            {
                message = "Berhasil mendapatkan dokumen pengguna",
                data = documents
            });
        }

        [HttpGet("workspace/{workspaceId}")]
        public IActionResult GetDocumentsByWorkspaceId(Guid workspaceId)
        {
            var documents = DocumentService.GetByWorkspaceId(workspaceId);

            return Ok(new
            {
                message = "Berhasil mendapatkan dokumen dalam workspace",
                data = documents
            });
        }

        [HttpPost]
        public IActionResult CreateDocument([FromBody] Document document)
        {
            document.Updated_at = DateTime.Now;
            DocumentService.Create(document);

            return CreatedAtAction(nameof(GetDocumentById), new { id = document.Id }, new
            {
                message = "Berhasil membuat dokumen baru",
                data = document
            });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDocument(Guid id, [FromBody] Document document)
        {
            var existingDocument = DocumentService.GetById(id);

            if (existingDocument == null)
            {
                return NotFound(new
                {
                    message = "Dokumen tidak ditemukan"
                });
            }

            DocumentService.Update(id, document);

            return Ok(new
            {
                message = "Dokumen berhasil diperbarui",
                data = DocumentService.GetById(id)
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDocument(Guid id)
        {
            var existingDocument = DocumentService.GetById(id);

            if (existingDocument == null)
            {
                return NotFound(new
                {
                    message = "Dokumen tidak ditemukan"
                });
            }

            DocumentService.Delete(id);

            return Ok(new
            {
                message = "Dokumen berhasil dihapus"
            });
        }
    }
}