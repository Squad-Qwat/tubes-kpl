// PaperNest_API.Models.Document.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaperNest_API.Models
{
    public class Document : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        // Points to the *currently active/approved* DocumentBody (like HEAD in Git)
        public Guid? CurrentDocumentBodyId { get; set; }

        [ForeignKey("CurrentDocumentBodyId")]
        public virtual DocumentBody? CurrentDocumentBody { get; set; }

        // For local edits (drafts) that are not yet a formal DocumentBody version
        public string? LocalContentDraft { get; set; }
        public bool HasDraft { get; set; } = false;
        public Guid? LastEditedByUserId { get; set; }
        public DateTime? LastEditedAt { get; set; }

        public string? Content { get; set; }

        [Required]
        public Guid User_id { get; set; } // Creator of the document

        [ForeignKey("User_id")]
        public virtual User User { get; set; } = null!;

        [Required]
        public Guid Workspace_id { get; set; }

        [ForeignKey("Workspace_id")]
        public virtual Workspace Workspace { get; set; } = null!; // Assuming Workspace model exists

        public virtual ICollection<DocumentBody> DocumentBodies { get; set; } = new List<DocumentBody>();

        // Constructor for EF
        protected Document() { }

        public Document(Guid id, string title, Guid userId, Guid workspaceId, string? description = null, string? initialContent = null)
        {
            Id = id;
            Title = title;
            Description = description;
            User_id = userId;
            Workspace_id = workspaceId;
            Created_at = DateTime.Now;
            Updated_at = DateTime.Now;
            LocalContentDraft = initialContent; // Initial content is also a draft
            HasDraft = true;
            LastEditedByUserId = userId;
            LastEditedAt = DateTime.Now;
        }
    }

    // DTO for creating a document
    public class DocumentCreateDto
    {
        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? InitialContent { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid WorkspaceId { get; set; }
    }

    // DTO for updating document metadata (title, description)
    public class DocumentUpdateMetadataDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
    }

    // DTO for updating document content (draft)
    public class DocumentUpdateContentDto
    {
        [Required]
        public string Content { get; set; }
        [Required]
        public Guid EditorId { get; set; }
    }

    // DTO for submitting a document for review (the 'push' action)
    public class ResearchRequestSubmissionDto
    {
        [Required]
        public Guid UserId { get; set; }
        public string? Title { get; set; } // Optional: can override document title
        [Required]
        public string AbstractText { get; set; }
    }
}