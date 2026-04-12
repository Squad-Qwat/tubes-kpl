using PaperNest_API.Controllers;
using PaperNest_API.Models;
using PaperNest_API.Views;
using System.Globalization; // For DateTime parsing

namespace PaperNest_API
{
    public class CitationMain
    {
        public static void Main(string[] args)
        {
            // Initialize the controller and view
            // We need to pass the dummy data dictionary to the controller for the console app
            // Or remove the dummy data from the controller and add it via the console app.
            // For simplicity in this console app, we'll keep the dummy data in the controller's constructor.
            CitationController controller = new CitationController();
            CitationView view = new CitationView();

            string? choice; // Use nullable string for Console.ReadLine()

            do
            {
                Console.WriteLine("\n--- Bibliography Manager ---");
                Console.WriteLine("1. Tampilkan semua sitasi (sebagai bibliografi)");
                Console.WriteLine("2. Menampilkan detil sitasi berdasarkan ID");
                Console.WriteLine("3. Tambah sitasi baru");
                Console.WriteLine("4. Memperbarui sitasi");
                Console.WriteLine("5. Menghapus sitasi");
                Console.WriteLine("6. Membuat Bibliografi"); // This is redundant with option 1, but kept as per original request.
                Console.WriteLine("7. Membuat teks sitasi (Jenis APA) berdasarkan ID");
                Console.WriteLine("8. Keluar");
                Console.Write("Masukkan pilihanmu: ");

                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                    case "6": // Both options will now display the full bibliography
                        var allCitationsBibliography = controller.GetAllBibliographyItems();
                        view.DisplayBibliography(allCitationsBibliography);
                        break;
                    case "2":
                        Console.Write("Masukkan ID sitasi untuk menampilkan detil: ");
                        if (int.TryParse(Console.ReadLine(), out int showId))
                        {
                            var citation = controller.GetCitation(showId); // Directly call the helper
                            if (citation != null)
                            {
                                view.DisplayCitation(citation);
                            }
                            else
                            {
                                Console.WriteLine($"Sitasi dengan ID {showId} tidak ada.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("ID tidak valid.");
                        }
                        break;
                    case "3":
                        Console.WriteLine("Masukkan detil pada sitasi baru:");
                        Console.Write("Tipe (Book, JournalArticle, Website, ConferencePaper, Thesis): ");
                        if (Enum.TryParse<CitationType>(Console.ReadLine(), true, out CitationType newType)) // 'true' for case-insensitive parsing
                        {
                            Console.Write("Judul: ");
                            string? newTitle = Console.ReadLine();
                            Console.Write("Pengarang: ");
                            string? newAuthor = Console.ReadLine();
                            Console.Write("Info publikasi: ");
                            string? newPublicationInfo = Console.ReadLine();

                            DateTime? publicationDate = null;
                            Console.Write("Tanggal publikasi (yyyy-mm-dd, kosongkan jika tidak ada): ");
                            string? pubDateInput = Console.ReadLine();
                            if (DateTime.TryParseExact(pubDateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedPubDate))
                            {
                                publicationDate = parsedPubDate;
                            }

                            Console.Write("Tanggal akses (YYYY-MM-DD, kosongkan jika tidak ada): ");
                            string? accessDate = Console.ReadLine();

                            Console.Write("DOI (kosongkan jika tidak ada): ");
                            string? doi = Console.ReadLine();

                            // Validate required fields
                            if (string.IsNullOrEmpty(newTitle) || string.IsNullOrEmpty(newAuthor) || string.IsNullOrEmpty(newPublicationInfo))
                            {
                                Console.WriteLine("Judul, Pengarang, dan Info Publikasi harus diisi.");
                                break;
                            }

                            var addedCitation = controller.AddCitation(newType, newTitle, newAuthor, newPublicationInfo, publicationDate, accessDate, doi);
                            if (addedCitation != null)
                            {
                                Console.WriteLine($"Sitasi berhasil ditambahkan dengan ID: {addedCitation.Id}.");
                            }
                            else
                            {
                                Console.WriteLine("Gagal menambahkan sitasi.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Tipe sitasi tidak valid.");
                        }
                        break;
                    case "4":
                        Console.Write("Masukkan ID sitasi yang akan diperbarui: ");
                        if (int.TryParse(Console.ReadLine(), out int updateId))
                        {
                            var existingCitation = controller.GetCitation(updateId);
                            if (existingCitation != null)
                            {
                                Console.WriteLine("Masukkan detil baru (Biarkan kosong untuk tidak mengubahnya):");

                                // Type
                                Console.Write($"Tipe baru ({existingCitation.Type}): ");
                                string? typeInput = Console.ReadLine();
                                CitationType? updatedType = null;
                                if (!string.IsNullOrEmpty(typeInput) && Enum.TryParse<CitationType>(typeInput, true, out CitationType parsedType))
                                {
                                    updatedType = parsedType;
                                }
                                else if (string.IsNullOrEmpty(typeInput))
                                {
                                    updatedType = existingCitation.Type;
                                }
                                else
                                {
                                    Console.WriteLine("Tipe sitasi baru tidak valid. Tipe tidak akan diubah.");
                                    updatedType = existingCitation.Type; // Keep existing type on invalid input
                                }


                                // Title
                                Console.Write($"Judul baru ({existingCitation.Title}): ");
                                string? updatedTitle = Console.ReadLine();
                                updatedTitle = string.IsNullOrEmpty(updatedTitle) ? existingCitation.Title : updatedTitle; // Use existing if input is empty


                                // Author
                                Console.Write($"Pengarang baru ({existingCitation.Author}): ");
                                string? updatedAuthor = Console.ReadLine();
                                updatedAuthor = string.IsNullOrEmpty(updatedAuthor) ? existingCitation.Author : updatedAuthor;


                                // Publication Info
                                Console.Write($"Info publikasi baru ({existingCitation.PublicationInfo}): ");
                                string? updatedPublicationInfo = Console.ReadLine();
                                updatedPublicationInfo = string.IsNullOrEmpty(updatedPublicationInfo) ? existingCitation.PublicationInfo : updatedPublicationInfo;


                                // Publication Date
                                DateTime? updatedPublicationDate = existingCitation.PublicationDate; // Default to existing
                                Console.Write($"Tanggal publikasi baru (yyyy-mm-dd) ({existingCitation.PublicationDate?.ToShortDateString() ?? "N/A"}): ");
                                string? pubDateUpdateInput = Console.ReadLine();
                                if (!string.IsNullOrEmpty(pubDateUpdateInput) && DateTime.TryParseExact(pubDateUpdateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedUpdatedPubDate))
                                {
                                    updatedPublicationDate = parsedUpdatedPubDate;
                                }
                                else if (!string.IsNullOrEmpty(pubDateUpdateInput) && !DateTime.TryParseExact(pubDateUpdateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                                {
                                    Console.WriteLine("Format tanggal publikasi tidak valid. Tanggal publikasi tidak akan diubah.");
                                }


                                // Access Date
                                Console.Write($"Tanggal akses baru ({existingCitation.AccessDate ?? "N/A"}): ");
                                string? updatedAccessDate = Console.ReadLine();
                                updatedAccessDate = string.IsNullOrEmpty(updatedAccessDate) ? existingCitation.AccessDate : updatedAccessDate;


                                // DOI
                                Console.Write($"DOI baru ({existingCitation.DOI ?? "N/A"}): ");
                                string? updatedDOI = Console.ReadLine();
                                updatedDOI = string.IsNullOrEmpty(updatedDOI) ? existingCitation.DOI : updatedDOI;

                                var updatedCitation = controller.UpdateCitation(updateId, updatedType, updatedTitle, updatedAuthor, updatedPublicationInfo, updatedPublicationDate, updatedAccessDate, updatedDOI);
                                if (updatedCitation != null)
                                {
                                    Console.WriteLine("Sitasi berhasil diperbarui.");
                                }
                                else
                                {
                                    Console.WriteLine("Gagal memperbarui sitasi (mungkin ID tidak ditemukan).");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Sitasi dengan ID {updateId} tidak ada.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("ID tidak valid.");
                        }
                        break;
                    case "5":
                        Console.Write("Masukkan ID sitasi yang akan dihapus: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteId))
                        {
                            if (controller.EraseCitation(deleteId)) // Directly call the helper
                            {
                                Console.WriteLine($"Sitasi dengan ID {deleteId} berhasil dihapus.");
                            }
                            else
                            {
                                Console.WriteLine($"Sitasi dengan ID {deleteId} tidak ditemukan.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("ID tidak valid.");
                        }
                        break;
                    case "7":
                        Console.Write("Masukkan ID sitasi untuk membuat teks jenis APA: ");
                        if (int.TryParse(Console.ReadLine(), out int citationId))
                        {
                            string citationText = controller.GetCitationText(citationId); // Directly call the helper
                            view.DisplayCitationText(citationText);
                        }
                        else
                        {
                            Console.WriteLine("ID tidak valid.");
                        }
                        break;
                    case "8":
                        Console.WriteLine("Keluar dari Bibliography Manager....");
                        return;
                    default:
                        Console.WriteLine("Pilihan tidak valid, silahkan coba lagi.");
                        break;
                }
            } while (choice != "8");
        }
    }
}