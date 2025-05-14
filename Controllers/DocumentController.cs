// PaperNest_API.Controllers.DocumentController.cs
using Microsoft.AspNetCore.Mvc;
using PaperNest_API.Models;
using PaperNest_API.Services;
using System;
using System.Linq;
using System.Collections.Generic; // Added for List<T>

namespace PaperNest_API.Controllers
{
    [ApiController, Route("api/documents")]
    public class DocumentController : ControllerBase // Changed from Controller to ControllerBase for API
    {
        // DocumentService is now a static facade for mock data, or would be injected in a real app.
        // private readonly DocumentService _documentService; // In a real app, inject IService

        public DocumentController()
        {
            // _documentService = new DocumentService(); // If it were an instance service
        }

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
        public IActionResult CreateDocument([FromBody] DocumentCreateDto documentDto)
        {
            // The document's initial content is considered a draft initially,
            // then it creates its first DocumentBody upon creation.
            var document = new Document(
                Guid.NewGuid(), // ID will be set by service/repository
                documentDto.Title,
                documentDto.UserId,
                documentDto.WorkspaceId,
                documentDto.Description,
                documentDto.InitialContent // This content becomes the first DocumentBody
            );

            DocumentService.Create(document); // This service call handles initial DocumentBody creation

            return CreatedAtAction(nameof(GetDocumentById), new { id = document.Id }, new
            {
                message = "Berhasil membuat dokumen baru",
                data = document
            });
        }

        // PUT: api/documents/{id}/metadata (for title, description)
        [HttpPut("{id}/metadata")]
        public IActionResult UpdateDocumentMetadata(Guid id, [FromBody] DocumentUpdateMetadataDto documentDto)
        {
            var existingDocument = DocumentService.GetById(id);

            if (existingDocument == null)
            {
                return NotFound(new { message = "Dokumen tidak ditemukan" });
            }

            if (documentDto.Title != null)
            {
                existingDocument.Title = documentDto.Title;
            }
            if (documentDto.Description != null)
            {
                existingDocument.Description = documentDto.Description;
            }

            DocumentService.Update(id, existingDocument); // Update the existing document in the service

            return Ok(new
            {
                message = "Metadata dokumen berhasil diperbarui",
                data = existingDocument // Return the updated object
            });
        }

        // PUT: api/documents/{id}/content (for local content/draft)
        [HttpPut("{id}/content")]
        public IActionResult UpdateDocumentContent(Guid id, [FromBody] DocumentUpdateContentDto contentDto)
        {
            var existingDocument = DocumentService.GetById(id);

            if (existingDocument == null)
            {
                return NotFound(new { message = "Dokumen tidak ditemukan" });
            }

            existingDocument.LocalContentDraft = contentDto.Content;
            existingDocument.HasDraft = true;
            existingDocument.LastEditedByUserId = contentDto.EditorId;
            existingDocument.LastEditedAt = DateTime.Now;

            DocumentService.Update(id, existingDocument); // Update the existing document in the service

            return Ok(new
            {
                message = "Konten dokumen (draft) berhasil diperbarui",
                data = existingDocument // Return the updated object
            });
        }

        // POST: api/documents/{id}/create-version (like a Git commit)
        [HttpPost("{id}/create-version")]
        public IActionResult CreateDocumentVersion(Guid id, [FromBody] string versionDescription = "New version")
        {
            var document = DocumentService.GetById(id);
            if (document == null)
            {
                return NotFound(new { message = "Dokumen tidak ditemukan." });
            }

            string contentToVersion = document.LocalContentDraft ?? document.CurrentDocumentBody?.Content ?? string.Empty;

            if (string.IsNullOrEmpty(contentToVersion))
            {
                return BadRequest(new { message = "Tidak dapat membuat versi dari konten kosong." });
            }

            // Check if content is identical to the latest version, if any
            var latestVersion = DocumentService.GetVersions(id).FirstOrDefault();
            if (latestVersion != null && latestVersion.Content == contentToVersion)
            {
                return BadRequest(new { message = "Konten sama dengan versi terakhir. Tidak ada perubahan untuk di-version." });
            }

            var newVersion = DocumentService.CreateVersion(id, contentToVersion, versionDescription);

            return Ok(new { message = "Versi dokumen baru berhasil dibuat.", data = newVersion });
        }


        // GET: api/documents/{id}/versions
        [HttpGet("{id}/versions")]
        public IActionResult GetDocumentVersions(Guid id)
        {
            var versions = DocumentService.GetVersions(id);
            if (!versions.Any())
            {
                return NotFound(new { message = "Tidak ada versi dokumen ditemukan." });
            }
            return Ok(new { message = "Berhasil mendapatkan versi dokumen.", data = versions });
        }

        // POST: api/documents/{id}/rollback/{versionId}
        [HttpPost("{id}/rollback/{versionId}")]
        public IActionResult RollbackDocument(Guid id, Guid versionId)
        {
            try
            {
                var rolledBackVersion = DocumentService.RollbackToVersion(id, versionId);
                var updatedDocument = DocumentService.GetById(id); // Get the document after rollback
                return Ok(new { message = "Dokumen berhasil di-rollback.", data = updatedDocument });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Gagal melakukan rollback: " + ex.Message });
            }
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

        // NEW ENDPOINT: Submit a Document's content for review (the 'push' to PaperNest remote)
        // This will create a ResearchRequest.
        [HttpPost("{documentId}/submit-for-review")]
        public IActionResult SubmitDocumentForReview(Guid documentId, [FromBody] ResearchRequestSubmissionDto submissionDto)
        {
            var document = DocumentService.GetById(documentId);
            if (document == null)
            {
                return NotFound(new { message = $"Dokumen dengan ID: {documentId} tidak ditemukan." });
            }

            // Ensure the user exists (mock or real)
            var submitter = PaperNest_API.Repository.UserRepository.userRepository.FirstOrDefault(u => u.Id == submissionDto.UserId);
            if (submitter == null)
            {
                return BadRequest(new { message = "Pengguna yang mengajukan tidak ditemukan." });
            }

            try
            {
                // DocumentService.SubmitForReview now handles creating the DocumentBody and ResearchRequest
                var newResearchRequest = DocumentService.SubmitForReview(
                    documentId,
                    submissionDto.UserId,
                    submitter.Name, // Or get from submitter object
                    submissionDto.Title ?? document.Title, // Use submission title or document title
                    submissionDto.AbstractText // Use submission abstract
                );
                return CreatedAtAction("GetRequestById", "ResearchRequest", new { id = newResearchRequest.Id }, newResearchRequest);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Gagal mengajukan dokumen untuk review: " + ex.Message });
            }
        }

        // PUT: api/documents/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateDocument(Guid id, [FromBody] dynamic documentUpdateDto)
        {
            var existingDocument = DocumentService.GetById(id);

            if (existingDocument == null)
            {
                return NotFound(new { message = "Dokumen tidak ditemukan" });
            }

            // Check if the incoming DTO is for metadata update
            if (documentUpdateDto.Title != null || documentUpdateDto.Description != null)
            {
                // Assuming DocumentUpdateMetadataDto would have these properties
                // We can directly map if types are compatible or use a dedicated DTO and try to bind
                if (documentUpdateDto.Title != null)
                {
                    existingDocument.Title = documentUpdateDto.Title;
                }
                if (documentUpdateDto.Description != null)
                {
                    existingDocument.Description = documentUpdateDto.Description;
                }

                DocumentService.Update(id, existingDocument);

                return Ok(new
                {
                    message = "Metadata dokumen berhasil diperbarui",
                    data = existingDocument
                });
            }
            // Check if the incoming DTO is for content update
            else if (documentUpdateDto.Content != null && documentUpdateDto.EditorId != null)
            {
                // Assuming DocumentUpdateContentDto would have these properties
                existingDocument.LocalContentDraft = documentUpdateDto.Content;
                existingDocument.HasDraft = true;
                existingDocument.LastEditedByUserId = (Guid)documentUpdateDto.EditorId;
                existingDocument.LastEditedAt = DateTime.Now;

                DocumentService.Update(id, existingDocument);

                return Ok(new
                {
                    message = "Konten dokumen (draft) berhasil diperbarui",
                    data = existingDocument
                });
            }
            else
            {
                return BadRequest(new { message = "Permintaan pembaruan tidak valid. Mohon berikan data metadata atau konten yang sesuai." });
            }
        }
}