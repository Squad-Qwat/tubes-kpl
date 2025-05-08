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
            Console.WriteLine($"  Type: {citation.Type}");
            Console.WriteLine($"  Title: {citation.Title}");
            Console.WriteLine($"  Author: {citation.Author}");
            Console.WriteLine($"  Publication Info: {citation.PublicationInfo}");
            Console.WriteLine($"  Publication Date: {citation.PublicationDate?.ToShortDateString() ?? "N/A"}"); // null-conditional
            Console.WriteLine($"  Access Date: {citation.AccessDate ?? "N/A"}");
            Console.WriteLine($"  DOI: {citation.DOI ?? "N/A"}");
        }

        public void DisplayBibliography(List<string> bibliography)
        {
            Console.WriteLine("Bibliography:");
            foreach (var entry in bibliography)
            {
                Console.WriteLine(entry);
            }
        }

        public void DisplayCitationText(string citationText)
        {
            Console.WriteLine("\nCitation (APA Style):");
            Console.WriteLine(citationText);
        }
    }
}