using PaperNest_API.Models;
using PaperNest_API.Views;
using System.Data;
using System.Linq;
namespace PaperNest_API.Controllers
{
    public class BibliographyController
    {
        private BibliographyModel model;
        private BibliographyView view;
        private int nextCitationId = 1;

        public BibliographyController()
        {
            model = new BibliographyModel();
            view = new BibliographyView();
        }

        public void ListCitations()
        {
            DataTable citations = model.GetCitations();
            view.DisplayCitations(citations);
        }

        public void ShowCitationDetails(int id)
        {
            Citation citation = model.GetCitationById(id);
            view.DisplayCitationDetails(citation);
        }

        
        // Menambahkan sitasi sesuai input pengguna
        public void AddNewCitation()
        {
            Console.WriteLine("Enter details for the new citation:");
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

            Citation newCitation = new Citation
            {
                Id = nextCitationId++,
                Author = author,
                Title = title,
                Publication = publication,
                Year = year,
                Page = page
            };
            model.AddCitation(newCitation);
            view.DisplayMessage($"Citation '{newCitation.Title}' added successfully with ID: {newCitation.Id}");
            ListCitations();
        }

        // Memperbaharui sitasi yang sudah ada sesuai input pengguna
        public void UpdateExistingCitation(int id)
        {
            Citation existingCitation = model.GetCitationById(id);
            if (existingCitation != null)
            {
                Console.WriteLine($"Enter updated details for citation ID: {id}");
                Console.Write("Author ({0}): ", existingCitation.Author);
                string author = Console.ReadLine();
                Console.Write("Title ({0}): ", existingCitation.Title);
                string title = Console.ReadLine();
                Console.Write("Publication ({0}): ", existingCitation.Publication);
                string publication = Console.ReadLine();
                Console.Write("Year ({0}): ", existingCitation.Year);
                int year;
                if (!int.TryParse(Console.ReadLine(), out year))
                {
                    year = existingCitation.Year;
                    Console.WriteLine($"Invalid year. Keeping the existing year: {year}");
                }
                Console.Write("Page ({0}): ", existingCitation.Page);
                string page = Console.ReadLine();

                Citation updatedCitation = new Citation
                {
                    Id = id,
                    Author = string.IsNullOrEmpty(author) ? existingCitation.Author : author,
                    Title = string.IsNullOrEmpty(title) ? existingCitation.Title : title,
                    Publication = string.IsNullOrEmpty(publication) ? existingCitation.Publication : publication,
                    Year = year,
                    Page = string.IsNullOrEmpty(page) ? existingCitation.Page : page
                };
                model.UpdateCitation(updatedCitation);
                view.DisplayMessage($"Citation with ID: {id} updated successfully.");
                ShowCitationDetails(id);
            }
            else
            {
                view.DisplayMessage($"Citation with ID: {id} not found.");
            }
        }


        public void DeleteCitation(int id)
        {
            Citation citationToDelete = model.GetCitationById(id);
            if (citationToDelete != null)
            {
                model.DeleteCitation(id);
                view.DisplayMessage($"Citation '{citationToDelete.Title}' (ID: {id}) deleted successfully.");
                ListCitations();
            }
            else
            {
                view.DisplayMessage($"Citation with ID: {id} not found.");
            }
        }
    }
}