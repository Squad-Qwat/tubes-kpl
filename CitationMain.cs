using PaperNest_API.Controllers;
using PaperNest_API.Models;
using PaperNest_API.Views;

namespace PaperNest_API
{
    public class CitationMain
    {
        public static void Main(string[] args)
        {
            CitationController controller = new(); // Setara 'new CitationController()'
            CitationView view = new(); // Setara 'new CitationView()'

            while (true)
            {
                Console.WriteLine("\n--- Bibliography Manager ---");
                Console.WriteLine("1. List All Citations");
                Console.WriteLine("2. Show Citation Details");
                Console.WriteLine("3. Add New Citation");
                Console.WriteLine("4. Update Citation");
                Console.WriteLine("5. Delete Citation");
                Console.WriteLine("6. Generate Bibliography");
                Console.WriteLine("7. Generate Citation Text (APA Style)");
                Console.WriteLine("8. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        var allCitations = controller.GenerateBibliography();
                        view.DisplayBibliography(allCitations);
                        break;
                    case "2":
                        Console.Write("Enter the ID of the citation to show details: ");
                        if (int.TryParse(Console.ReadLine(), out int showId))
                        {
                            var citation = controller.GetCitation(showId);
                            if (citation != null)
                            {
                                view.DisplayCitation(citation);
                            }
                            else
                            {
                                Console.WriteLine($"Citation with ID {showId} not found.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid ID.");
                        }
                        break;
                    case "3":
                        Console.WriteLine("Enter details for the new citation:");
                        Console.Write("Type (Book, JournalArticle, Website, ConferencePaper, Thesis): ");
                        if (Enum.TryParse<CitationType>(Console.ReadLine(), out CitationType newType))
                        {
                            Console.Write("Title: ");
                            string newTitle = Console.ReadLine();
                            Console.Write("Author: ");
                            string newAuthor = Console.ReadLine();
                            Console.Write("Publication Info: ");
                            string newPublicationInfo = Console.ReadLine();

                            controller.AddCitation(newType, newTitle, newAuthor, newPublicationInfo);
                            Console.WriteLine("Citation added successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid citation type.");
                        }
                        break;
                    case "4":
                        Console.Write("Enter the ID of the citation to update: ");
                        if (int.TryParse(Console.ReadLine(), out int updateId))
                        {
                            var existingCitation = controller.GetCitation(updateId);
                            if (existingCitation != null)
                            {
                                Console.WriteLine("Enter new details (leave blank to keep current):");
                                Console.Write($"New Type ({existingCitation.Type}): ");
                                string? typeInput = Console.ReadLine();
                                CitationType? updatedType = string.IsNullOrEmpty(typeInput) ? existingCitation.Type : Enum.Parse<CitationType>(typeInput);

                                Console.Write($"New Title ({existingCitation.Title}): ");
                                string? updatedTitle = string.IsNullOrEmpty(Console.ReadLine()) ? existingCitation.Title : Console.ReadLine();

                                Console.Write($"New Author ({existingCitation.Author}): ");
                                string? updatedAuthor = string.IsNullOrEmpty(Console.ReadLine()) ? existingCitation.Author : Console.ReadLine();

                                Console.Write($"New Publication Info ({existingCitation.PublicationInfo}): ");
                                string? updatedPublicationInfo = string.IsNullOrEmpty(Console.ReadLine()) ? existingCitation.PublicationInfo : Console.ReadLine();

                                Console.Write($"New Publication Date (yyyy-mm-dd) ({existingCitation.PublicationDate?.ToShortDateString() ?? "N/A"}): ");
                                DateTime? updatedPublicationDate = DateTime.TryParse(Console.ReadLine(), out DateTime date) ? date : existingCitation.PublicationDate;

                                Console.Write($"New Access Date ({existingCitation.AccessDate ?? "N/A"}): ");
                                string? updatedAccessDate = string.IsNullOrEmpty(Console.ReadLine()) ? existingCitation.AccessDate : Console.ReadLine();

                                Console.Write($"New DOI ({existingCitation.DOI ?? "N/A"}): ");
                                string? updatedDOI = string.IsNullOrEmpty(Console.ReadLine()) ? existingCitation.DOI : Console.ReadLine();

                                controller.UpdateCitation(updateId, updatedType, updatedTitle, updatedAuthor, updatedPublicationInfo, updatedPublicationDate, updatedAccessDate, updatedDOI);
                                Console.WriteLine("Citation updated successfully.");
                            }
                            else
                            {
                                Console.WriteLine($"Citation with ID {updateId} not found.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid ID.");
                        }
                        break;
                    case "5":
                        Console.Write("Enter the ID of the citation to delete: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteId))
                        {
                            controller.DeleteCitation(deleteId);
                            Console.WriteLine($"Citation with ID {deleteId} deleted.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid ID.");
                        }
                        break;
                    case "6":
                        var bibliography = controller.GenerateBibliography();
                        view.DisplayBibliography(bibliography);
                        break;
                    case "7":
                        Console.Write("Enter the ID of the citation to generate APA style text: ");
                        if (int.TryParse(Console.ReadLine(), out int citationId))
                        {
                            string citationText = controller.GenerateCitationText(citationId);
                            view.DisplayCitationText(citationText);
                        }
                        else
                        {
                            Console.WriteLine("Invalid ID.");
                        }
                        break;
                    case "8":
                        Console.WriteLine("Exiting Bibliography Manager.");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
    }
}