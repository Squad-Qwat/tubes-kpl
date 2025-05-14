using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaperNest_API.Models
{
    public class Citation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [Required]
        public CitationType Type { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string PublicationInfo { get; set; } //  Details vary by type (e.g., ISBN, Journal Name, URL)

        public DateTime? PublicationDate { get; set; } // Use nullable DateTime

        public string? AccessDate { get; set; }

        public string? DOI { get; set; }

        // Add more properties as needed (e.g., editor, publisher, volume, issue, pages)

        public Citation(int id, CitationType type, string title, string author, string publicationInfo)
        {
            Id = id;
            Type = type;
            Title = title;
            Author = author;
            PublicationInfo = publicationInfo;
        }
    }

    // Model class required for controller class (move here so the controller class doesn't have to deal with the model directly)
    public class CitationRequestModel
    {
        public CitationType Type { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? PublicationInfo { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string? AccessDate { get; set; }
        public string? DOI { get; set; }
    }

    public class CitationUpdateRequestModel
    {
        public CitationType? Type { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? PublicationInfo { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string? AccessDate { get; set; }
        public string? DOI { get; set; }
    }
}