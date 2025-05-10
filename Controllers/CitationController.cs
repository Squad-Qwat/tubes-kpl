using PaperNest_API.Models;
using PaperNest_API.Views;
using System.Data;
using System.Linq;
namespace PaperNest_API.Controllers
{
    public class CitationController
    {
        private readonly Dictionary<int, Citation> _citations = new Dictionary<int, Citation>();
        private int _nextId = 1;

        // Stair-step table for generating bibliography entries.
        //  [CitationType] =>  Action<Citation>
        private readonly Dictionary<CitationType, Action<Citation, List<string>>> _bibliographyFormatters =
        new Dictionary<CitationType, Action<Citation, List<string>>>
        {
            { CitationType.Book, (citation, bibliography) =>
                {
                    string entry = $"{citation.Author}. ({citation.PublicationDate?.Year}). {citation.Title}. {citation.PublicationInfo}.";
                    bibliography.Add(entry);
                }
            },
            { CitationType.JournalArticle, (citation, bibliography) =>
                {
                   string entry = $"{citation.Author}. ({citation.PublicationDate?.Year}). {citation.Title}. {citation.PublicationInfo}.";
                    bibliography.Add(entry);
                }
            },
            { CitationType.Website, (citation, bibliography) =>
                {
                    string entry = $"{citation.Author}. ({citation.PublicationDate?.Year}). {citation.Title}. {citation.PublicationInfo}. Retrieved from {citation.AccessDate}";
                    bibliography.Add(entry);
                }
            },
             { CitationType.ConferencePaper, (citation, bibliography) =>
                {
                    string entry = $"{citation.Author}. ({citation.PublicationDate?.Year}). {citation.Title}. In {citation.PublicationInfo}.";
                    bibliography.Add(entry);
                }
            },
            { CitationType.Thesis, (citation, bibliography) =>
                {
                    string entry = $"{citation.Author}. ({citation.PublicationDate?.Year}). {citation.Title}. {citation.PublicationInfo}.";
                    bibliography.Add(entry);
                }
            },
            // Add other citation types as needed
        };

        public int AddCitation(CitationType type, string title, string author, string publicationInfo)
        {
            var newCitation = new Citation(_nextId++, type, title, author, publicationInfo);
            _citations.Add(newCitation.Id, newCitation);
            return newCitation.Id;
        }

        public Citation? GetCitation(int id)
        {
            return _citations.TryGetValue(id, out var citation) ? citation : null;
            /* Setara dengan:
             * if (_citations.TryGetValue(id, out var citation))
             * {
             *    return citation;
             * }
             * return null;
            */
        }

        public void UpdateCitation(int id, CitationType? type = null, string title = null, string author = null, string publicationInfo = null, DateTime? publicationDate = null, string accessDate = null, string doi = null)
        {
            var citation = GetCitation(id);
            if (citation != null)
            {
                if (type.HasValue) citation.Type = type.Value;
                if (title != null) citation.Title = title;
                if (author != null) citation.Author = author;
                if (publicationInfo != null) citation.PublicationInfo = publicationInfo;
                if (publicationDate.HasValue) citation.PublicationDate = publicationDate.Value;
                if (accessDate != null) citation.AccessDate = accessDate;
                if (doi != null) citation.DOI = doi;
            }
        }

        public void DeleteCitation(int id)
        {
            try
            {
                if (_citations.ContainsKey(id))
                {
                    _citations.Remove(id);
                    Console.WriteLine($"Sitasi dengan ID {id} berhasil dihapus.");
                }
                else
                {
                    throw new Exception($"Sitasi dengan ID {id} tidak ada.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Terjadi kesalahan saat menghapus sitasi: {ex.Message}");
            }
        }

        public List<string> GenerateBibliography()
        {
            var bibliography = new List<string>();
            // Iterate through all citations and use the table to format them.
            foreach (var citation in _citations.Values.OrderBy(c => c.Author)) // Order by author
            {
                if (_bibliographyFormatters.TryGetValue(citation.Type, out var formatter))
                {
                    formatter(citation, bibliography); // Use the delegate to format.
                }
                else
                {
                    bibliography.Add($"Format sitasi tidak didukung untuk tipe: {citation.Type}"); // error
                }
            }
            return bibliography;
        }

        public string GenerateCitationText(int citationId)
        {
            var citation = GetCitation(citationId);
            return (citation == null) ? "Sitasi tidak ada" : citation.GenerateAPAStyle();
            /*
             * Setara dengan:
             * if(citation == null) 
             * {
             *    return "Sitasi tidak ada";
             * }
             * return citation.GenerateAPAStyle();
            */
        }
    }
}