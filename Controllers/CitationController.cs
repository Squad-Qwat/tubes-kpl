using System;
using PaperNest_API.Models;
using PaperNest_API.Views;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
namespace PaperNest_API.Controllers
{
    /* 
    Sebelum:
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
            { Setara dengan:
            if (_citations.TryGetValue(id, out var citation))
            {
                return citation;
            }
            return null;
            }
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
            { Setara dengan:
            if (citation == null)
            {
                *    return "Sitasi tidak ada";
            }
            return citation.GenerateAPAStyle();
            }
        }
    }
    */

    [ApiController]
    [Route("api/citations")]
    public class CitationController : ControllerBase
    {
        private readonly Dictionary<int, Citation> _citations = new Dictionary<int, Citation>();
        private int _nextId = 1;

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

        [HttpPost]
        public IActionResult CreateCitation([FromBody] CitationRequestModel model)
        {
            var newCitation = new Citation(
                _nextId++,
                model.Type,
                model.Title,
                model.Author,
                model.PublicationInfo
            )
            {
                PublicationDate = model.PublicationDate,
                // Kalau null, maka variable nilai kiri bakal null. Nggak bakal bikin null error
                AccessDate = model.AccessDate,
                DOI = model.DOI
            };

            _citations.Add(newCitation.Id, newCitation);

            return CreatedAtAction(nameof(GetCitationById), new { id = newCitation.Id }, new
            {
                message = "Sitasi berhasil ditambahkan",
                data = newCitation
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetCitationById(int id)
        {
            var citation = GetCitation(id);

            if (citation == null)
            {
                return NotFound(new
                {
                    message = "Sitasi tidak ditemukan"
                });
            }

            return Ok(new
            {
                message = "Berhasil mendapatkan data sitasi",
                data = citation
            });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCitation(int id, [FromBody] CitationUpdateRequestModel model)
        {
            var existingCitation = GetCitation(id);

            if (existingCitation == null)
            {
                return NotFound(new
                {
                    message = "Sitasi tidak ditemukan"
                });
            }

            if (model.Type.HasValue) existingCitation.Type = model.Type.Value;
            if (model.Title != null) existingCitation.Title = model.Title;
            if (model.Author != null) existingCitation.Author = model.Author;
            if (model.PublicationInfo != null) existingCitation.PublicationInfo = model.PublicationInfo;
            if (model.PublicationDate.HasValue) existingCitation.PublicationDate = model.PublicationDate.Value;
            if (model.AccessDate != null) existingCitation.AccessDate = model.AccessDate;
            if (model.DOI != null) existingCitation.DOI = model.DOI;

            return Ok(new
            {
                message = "Sitasi berhasil diperbarui",
                data = existingCitation
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCitation(int id)
        {
            if (!_citations.ContainsKey(id))
            {
                return NotFound(new
                {
                    message = $"Sitasi dengan ID {id} tidak ditemukan"
                });
            }

            _citations.Remove(id);

            return Ok(new
            {
                message = "Sitasi berhasil dihapus"
            });
        }

        [HttpGet("bibliography")]
        public IActionResult GenerateBibliography()
        {
            var bibliography = new List<string>();
            foreach (var citation in _citations.Values.OrderBy(c => c.Author))
            {
                if (_bibliographyFormatters.TryGetValue(citation.Type, out var formatter))
                {
                    formatter(citation, bibliography);
                }
                else
                {
                    bibliography.Add($"Format sitasi tidak didukung untuk tipe: {citation.Type}");
                }
            }

            return Ok(new
            {
                message = "Berhasil menghasilkan bibliografi",
                data = bibliography
            });
        }

        [HttpGet("{id}/citation-text")]
        public IActionResult GenerateCitationText(int id)
        {
            var citation = GetCitation(id);

            if (citation == null)
            {
                return NotFound(new
                {
                    message = "Sitasi tidak ditemukan"
                });
            }

            return Ok(new
            {
                message = "Berhasil menghasilkan teks sitasi",
                data = citation.GenerateAPAStyle()
            });
        }

        private Citation? GetCitation(int id)
        {
            return _citations.TryGetValue(id, out var citation) ? citation : null;
        }
    }
}