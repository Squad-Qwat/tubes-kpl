using System;
using PaperNest_API.Models;
using PaperNest_API.Views;
using PaperNest_API.Utils;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
namespace PaperNest_API.Controllers
{
    [ApiController]
    [Route("api/citations")]
    public class CitationController : ControllerBase
    {
        private readonly Dictionary<int, Citation> _citations = []; // Setara dengan 'new Dictionary<int, Citation>()';
        private int _nextId = 1;

        public CitationController()
        {
            _citations.Add(_nextId++, new Citation(1, CitationType.Book, "Contoh Judul Buku", "Pengarang Buku", "Penerbit Contoh")
            {
                PublicationDate = new DateTime(2020, 1, 15)
            });
            _citations.Add(_nextId++, new Citation(2, CitationType.JournalArticle, "Artikel Jurnal Bagus", "Penulis Artikel", "Jurnal Sains, Vol. 10, No. 2, pp. 123-145")
            {
                PublicationDate = new DateTime(2022, 5, 10),
                DOI = "10.1234/journal.article.123"
            });
            _citations.Add(_nextId++, new Citation(3, CitationType.Website, "Panduan Online", "Webmaster", "https://example.com/panduan")
            {
                PublicationDate = new DateTime(2023, 3, 20),
                AccessDate = "2024-05-15"
            });
        }


        // The bibliography formatters dictionary should use CitationFormatter
        private readonly Dictionary<CitationType, Func<Citation, string>> _bibliographyFormatters =
            new Dictionary<CitationType, Func<Citation, string>>
            {
                { CitationType.Book, citation => CitationFormatter.GenerateAPAStyle(citation) },
                { CitationType.JournalArticle, citation => CitationFormatter.GenerateAPAStyle(citation) },
                { CitationType.Website, citation => CitationFormatter.GenerateAPAStyle(citation) },
                { CitationType.ConferencePaper, citation => CitationFormatter.GenerateAPAStyle(citation) },
                { CitationType.Thesis, citation => CitationFormatter.GenerateAPAStyle(citation) }
            };


        [HttpPost]
        public IActionResult CreateCitation([FromBody] CitationRequestModel model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Permintaan tidak valid." });
            }

            var newCitation = new Citation(_nextId++, model.Type, model.Title, model.Author, model.PublicationInfo)
            {
                PublicationDate = model.PublicationDate,
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

        [NonAction]
        public Citation? AddCitation(CitationType type, string title, string author, string publicationInfo, DateTime? publicationDate = null, string? accessDate = null, string? doi = null)
        {
            var newCitation = new Citation(_nextId++, type, title, author, publicationInfo)
            {
                PublicationDate = publicationDate,
                AccessDate = accessDate,
                DOI = doi
            };

            _citations.Add(newCitation.Id, newCitation);
            return newCitation;
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

        [NonAction]
        public Citation? UpdateCitation(int id, CitationType? type, string? title, string? author, string? publicationInfo, DateTime? publicationDate, string? accessDate, string? doi)
        {
            var existingCitation = GetCitation(id);
            if (existingCitation == null)
            {
                return null;
            }

            if (type.HasValue) existingCitation.Type = type.Value;
            if (title != null) existingCitation.Title = title;
            if (author != null) existingCitation.Author = author;
            if (publicationInfo != null) existingCitation.PublicationInfo = publicationInfo;
            if (publicationDate.HasValue) existingCitation.PublicationDate = publicationDate.Value;
            if (accessDate != null) existingCitation.AccessDate = accessDate;
            if (doi != null) existingCitation.DOI = doi;

            return existingCitation;
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

        [NonAction]
        public bool EraseCitation(int id)
        {
            return _citations.Remove(id);
        }

        [HttpGet("bibliography")]
        public IActionResult GenerateBibliography()
        {
            var bibliography = new List<string>();
            foreach (var citation in _citations.Values.OrderBy(c => c.Author))
            {
                if (_bibliographyFormatters.TryGetValue(citation.Type, out var formatter))
                {
                    bibliography.Add(formatter(citation));
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

        [NonAction]
        public List<string> GetAllBibliographyItems()
        {
            var bibliography = new List<string>();
            foreach (var citation in _citations.Values.OrderBy(c => c.Author))
            {
                if (_bibliographyFormatters.TryGetValue(citation.Type, out var formatter))
                {
                    bibliography.Add(formatter(citation));
                }
                else
                {
                    bibliography.Add($"Format sitasi tidak didukung untuk tipe: {citation.Type}");
                }
            }
            return bibliography;
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
                data = CitationFormatter.GenerateAPAStyle(citation)
            });
        }

        [NonAction]
        public string GetCitationText(int id)
        {
            var citation = GetCitation(id);
            if (citation == null)
            {
                return "Sitasi tidak ditemukan.";
            }
            return CitationFormatter.GenerateAPAStyle(citation);
        }

        [NonAction]
        public Citation? GetCitation(int id)
        {
            return _citations.TryGetValue(id, out var citation) ? citation : null;
        }
    }
}