using System.Data;
using System.Linq;
namespace PaperNest_API.Models
{
    public class Citation : BaseEntity
    {
        public CitationType Type { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string PublicationInfo { get; set; } //  Details vary by type (e.g., ISBN, Journal Name, URL)
        public DateTime? PublicationDate { get; set; } // Use nullable DateTime
        public string AccessDate { set; get; }
        public string DOI { get; set; }
        // Add more properties as needed (e.g., editor, publisher, volume, issue, pages)

        public Citation(int id, CitationType type, string title, string author, string publicationInfo)
        {
            Id = id;
            Type = type;
            Title = title;
            Author = author;
            PublicationInfo = publicationInfo;
        }

        public string GenerateAPAStyle()
        {
            string apaString = "";
            switch (Type)
            {
                case CitationType.Book:
                    apaString = $"{Author}. ({PublicationDate?.Year}). *{Title}*. {PublicationInfo}.";
                    break;
                case CitationType.JournalArticle:
                    apaString = $"{Author}. ({PublicationDate?.Year}). *{Title}*. *{PublicationInfo}*.";
                    break;
                case CitationType.Website:
                    apaString = $"{Author}. ({PublicationDate?.Year}). *{Title}*. {PublicationInfo}. Diakses dari {AccessDate}";
                    break;
                case CitationType.ConferencePaper:
                    apaString = $"{Author}. ({PublicationDate?.Year}). *{Title}*. In *{PublicationInfo}*.";
                    break;
                case CitationType.Thesis:
                    apaString = $"{Author}. ({PublicationDate?.Year}). *{Title}*. {PublicationInfo}.";
                    break;
                default:
                    apaString = "Format sitasi tidak didukung.";
                    break;
            }
            return apaString;
        }
    }
}