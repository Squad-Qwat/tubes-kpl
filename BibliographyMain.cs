using PaperNest_API.Controllers;

namespace PaperNest_API
{
    public class BibliographyMain
    {
        public static void Main(string[] args)
        {
            BibliographyController controller = new BibliographyController();

            while (true)
            {
                Console.WriteLine("\n--- Bibliography Manager ---");
                Console.WriteLine("1. List All Citations");
                Console.WriteLine("2. Show Citation Details");
                Console.WriteLine("3. Add New Citation");
                Console.WriteLine("4. Update Citation");
                Console.WriteLine("5. Delete Citation");
                Console.WriteLine("6. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        controller.ListCitations();
                        break;
                    case "2":
                        Console.Write("Masukkan ID sitasi untuk menunjukkan detil dari sitasi: ");
                        if (int.TryParse(Console.ReadLine(), out int showId))
                        {
                            controller.ShowCitationDetails(showId);
                        }
                        else
                        {
                            Console.WriteLine("Invalid ID.");
                        }
                        break;
                    case "3":
                        controller.AddNewCitation();
                        break;
                    case "4":
                        Console.Write("Enter the ID of the citation to update: ");
                        if (int.TryParse(Console.ReadLine(), out int updateId))
                        {
                            controller.UpdateExistingCitation(updateId);
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
                        }
                        else
                        {
                            Console.WriteLine("Invalid ID.");
                        }
                        break;
                    case "6":
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