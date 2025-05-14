// PaperNest_API.Services.DocumentService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using PaperNest_API.Models;
using PaperNest_API.Repository; // Assuming UserRepository for mock user data
using PaperNest_API.Services; // For ResearchRequestManager if needed for internal calls

namespace PaperNest_API.Services
{
    // This class acts as a mock repository/service for Documents and DocumentBodies
    // In a real application, this would interact with a database.
    public static class DocumentService
    {
        private static readonly List<Document> _documents = new List<Document>();
        private static readonly List<DocumentBody> _documentBodies = new List<DocumentBody>();

        public static List<Document> GetAll() => _documents;
        public static Document? GetById(Guid id) => _documents.FirstOrDefault(d => d.Id == id);
        public static List<Document> GetByUserId(Guid userId) => _documents.Where(d => d.User_id == userId).ToList();
        public static List<Document> GetByWorkspaceId(Guid workspaceId) => _documents.Where(d => d.Workspace_id == workspaceId).ToList();

        public static void Create(Document document)
        {
            document.Id = Guid.NewGuid(); // Ensure unique ID
            document.Created_at = DateTime.Now;
            document.Updated_at = DateTime.Now;

            // When a new document is created, its initial content also forms a DocumentBody
            var initialDocumentBody = new DocumentBody(document.LocalContentDraft ?? string.Empty, document.Id, "Initial document content");
            initialDocumentBody.IsCurrentVersion = true; // Mark as current
            _documentBodies.Add(initialDocumentBody);

            document.CurrentDocumentBodyId = initialDocumentBody.Id; // Link document to its initial body
            document.HasDraft = false; // Initial content is now a formal version
            document.LocalContentDraft = null; // Clear local draft once versioned

            _documents.Add(document);
        }

        public static void Update(Guid id, Document updatedDocument)
        {
            var existingDocument = GetById(id);
            if (existingDocument != null)
            {
                existingDocument.Title = updatedDocument.Title;
                existingDocument.Description = updatedDocument.Description;
                existingDocument.LocalContentDraft = updatedDocument.LocalContentDraft;
                existingDocument.HasDraft = updatedDocument.HasDraft;
                existingDocument.LastEditedByUserId = updatedDocument.LastEditedByUserId;
                existingDocument.LastEditedAt = updatedDocument.LastEditedAt;
                existingDocument.Updated_at = DateTime.Now;
            }
        }

        public static void Delete(Guid id)
        {
            _documents.RemoveAll(d => d.Id == id);
            _documentBodies.RemoveAll(db => db.DocumentId == id); // Also delete associated document bodies
        }

        // --- DocumentBody (Version) Management ---

        public static DocumentBody? GetDocumentBodyById(Guid id) => _documentBodies.FirstOrDefault(db => db.Id == id);

        // Get all versions for a specific document, ordered by creation date (latest first)
        public static List<DocumentBody> GetVersions(Guid documentId) =>
            _documentBodies.Where(db => db.DocumentId == documentId)
                           .OrderByDescending(db => db.Created_at)
                           .ToList();

        // Creates a new DocumentBody version and updates the Document's CurrentDocumentBody
        public static DocumentBody CreateVersion(Guid documentId, string content, string versionDescription)
        {
            var document = GetById(documentId);
            if (document == null) throw new ArgumentException("Document not found.");

            // Invalidate previous current version
            var previousCurrent = _documentBodies.FirstOrDefault(db => db.DocumentId == documentId && db.IsCurrentVersion);
            if (previousCurrent != null)
            {
                previousCurrent.IsCurrentVersion = false;
            }

            var newDocumentBody = new DocumentBody(content, documentId, versionDescription);
            _documentBodies.Add(newDocumentBody);

            document.CurrentDocumentBodyId = newDocumentBody.Id;
            document.LocalContentDraft = null; // Clear draft after creating a formal version
            document.HasDraft = false; // No more active draft
            document.Updated_at = DateTime.Now;

            return newDocumentBody;
        }

        // Rolls back the Document to a previous DocumentBody version
        public static DocumentBody RollbackToVersion(Guid documentId, Guid documentBodyId)
        {
            var document = GetById(documentId);
            if (document == null) throw new ArgumentException("Document not found.");

            var targetVersion = GetDocumentBodyById(documentBodyId);
            if (targetVersion == null || targetVersion.DocumentId != documentId)
            {
                throw new ArgumentException("Target version not found for this document.");
            }

            // Invalidate current version
            var previousCurrent = _documentBodies.FirstOrDefault(db => db.DocumentId == documentId && db.IsCurrentVersion);
            if (previousCurrent != null)
            {
                previousCurrent.IsCurrentVersion = false;
            }

            targetVersion.IsCurrentVersion = true; // Set selected version as current
            document.CurrentDocumentBodyId = targetVersion.Id;
            document.LocalContentDraft = targetVersion.Content; // Load content into local draft
            document.HasDraft = true; // Indicate a draft exists (which is the rolled back version)
            document.LastEditedAt = DateTime.Now; // Update edit time
            // We might not know who rolled back, but for simplicity, can set to current user if available from context.
            // Or leave LastEditedByUserId as is or null.
            document.Updated_at = DateTime.Now;

            return targetVersion;
        }

        // --- Integration with ResearchRequest (the 'push' functionality) ---
        // This method creates a ResearchRequest from the current local content of a Document.
        public static ResearchRequest SubmitForReview(Guid documentId, Guid userId, string submitterName, string title, string abstractText, string versionDescription = "New submission")
        {
            var document = GetById(documentId);
            if (document == null) throw new ArgumentException("Document not found.");

            // Create a new DocumentBody from the document's current local content (draft)
            // Or if no draft, use the CurrentDocumentBody's content.
            string contentToSubmit = document.LocalContentDraft ?? document.CurrentDocumentBody?.Content ?? string.Empty;
            if (string.IsNullOrEmpty(contentToSubmit))
            {
                throw new InvalidOperationException("Cannot submit an empty document for review.");
            }

            // Create a new DocumentBody to represent this specific submission's content
            // This DocumentBody is *not* immediately marked as current, only when the ResearchRequest is approved.
            var submissionDocumentBody = new DocumentBody(contentToSubmit, documentId, versionDescription)
            {
                IsCurrentVersion = false // This is a proposed version, not current yet
            };
            _documentBodies.Add(submissionDocumentBody);

            // Create the ResearchRequest (the 'pull request')
            var researchRequest = new ResearchRequest(
                Guid.NewGuid(),
                title,
                abstractText,
                submitterName,
                userId,
                documentId,
                submissionDocumentBody.Id // Link to the specific DocumentBody of this submission
            );

            // In a real app, this would add to a ResearchRequestRepository.
            // For this mock, we'll need a static list for ResearchRequests too, or pass it to ResearchRequestManager.
            // For now, let's assume ResearchRequestManager handles the persistence.
            // Or, for the console demo, we can just return it and let the controller handle its mock storage.
            return researchRequest;
        }
    }
}
