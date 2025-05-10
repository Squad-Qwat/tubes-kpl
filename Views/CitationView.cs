using PaperNest_API.Models;
using System.Data;
using System.Linq;
namespace PaperNest_API.Views
{
    public class CitationView
    {
        public void DisplayCitation(Citation citation)
        {
            Console.WriteLine("Citation Details:");
            Console.WriteLine($"  ID: {citation.Id}");
            Console.WriteLine($"  Tipe: {citation.Type}");
            Console.WriteLine($"  Judul: {citation.Title}");
            Console.WriteLine($"  Pengarang: {citation.Author}");
            Console.WriteLine($"  Info publikasi: {citation.PublicationInfo}");
            Console.WriteLine($"  Tanggal publikasi: {citation.PublicationDate?.ToShortDateString() ?? "N/A"}"); // null-conditional
            Console.WriteLine($"  Tanggal akses: {citation.AccessDate ?? "N/A"}");
            Console.WriteLine($"  DOI: {citation.DOI ?? "N/A"}");
        }

        public void DisplayBibliography(List<string> bibliography)
        {
            Console.WriteLine("Bibliografi:");
            foreach (var entry in bibliography)
            {
                Console.WriteLine(entry);
            }
        }

        public void DisplayCitationText(string citationText)
        {
            Console.WriteLine("\nSitasi (jenis APA):");
            Console.WriteLine(citationText);
        }
    }
}