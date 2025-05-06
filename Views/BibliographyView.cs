using PaperNest_API.Models;
using System.Data;
using System.Linq;
namespace PaperNest_API.Views
{
    public class BibliographyView
    {
        public void DisplayCitations(DataTable citations)
        {
            Console.WriteLine("--- Bibliography ---");
            if (citations.Rows.Count == 0)
            {
                Console.WriteLine("No citations available.");
                return;
            }

            foreach (DataRow row in citations.Rows)
            {
                Console.WriteLine($"ID: {row["Id"]}, Author: {row["Author"]}, Title: {row["Title"]}, Publication: {row["Publication"]}, Year: {row["Year"]}, Page: {row["Page"]}");
            }
            Console.WriteLine("--------------------");
        }

        public void DisplayCitationDetails(Citation citation)
        {
            if (citation != null)
            {
                Console.WriteLine("--- Citation Details ---");
                Console.WriteLine($"ID: {citation.Id}");
                Console.WriteLine($"Author: {citation.Author}");
                Console.WriteLine($"Title: {citation.Title}");
                Console.WriteLine($"Publication: {citation.Publication}");
                Console.WriteLine($"Year: {citation.Year}");
                Console.WriteLine($"Page: {citation.Page}");
                Console.WriteLine("------------------------");
            }
            else
            {
                Console.WriteLine("Citation not found.");
            }
        }

        public Citation GetUserInputForCitation(int? id = null)
        {
            Console.WriteLine($"{(id.HasValue ? "Update" : "Add")} Citation:");
            if (id.HasValue)
            {
                Console.Write("ID: ");
                Console.WriteLine(id.Value);
            }
            Console.Write("Author: ");
            string author = Console.ReadLine();
            Console.Write("Title: ");
            string title = Console.ReadLine();
            Console.Write("Publication: ");
            string publication = Console.ReadLine();
            Console.Write("Year: ");
            if (!int.TryParse(Console.ReadLine(), out int year))
            {
                Console.WriteLine("Invalid year. Using current year.");
                year = DateTime.Now.Year;
            }
            Console.Write("Page: ");
            string page = Console.ReadLine();

            return new Citation
            {
                Id = id ?? 0, // If updating, keep the ID
                Author = author,
                Title = title,
                Publication = publication,
                Year = year,
                Page = page
            };
        }

        public int GetCitationIdFromUser(string action)
        {
            Console.Write($"Enter the ID of the citation to {action}: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                return id;
            }
            Console.WriteLine("Invalid ID.");
            return -1; // Indicate invalid input
        }

        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}