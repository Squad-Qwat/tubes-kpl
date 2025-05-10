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

            string choice;
            do
            {
                Console.WriteLine("\n--- Bibliography Manager ---");
                Console.WriteLine("1. Isi semua sitasi");
                Console.WriteLine("2. Menampilkan detil sitasi");
                Console.WriteLine("3. Tambah sitasi baru");
                Console.WriteLine("4. Memperbarui sitasi");
                Console.WriteLine("5. Menghapus sitasi");
                Console.WriteLine("6. Membuat Bibliografi");
                Console.WriteLine("7. Membuat teks sitasi (Jenis APA)");
                Console.WriteLine("8. Keluar");
                Console.Write("Masukkan pilihanmu: ");

                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        var allCitations = controller.GenerateBibliography();
                        view.DisplayBibliography(allCitations);
                        break;
                    case "2":
                        Console.Write("Masukkan ID sitasi untuk menampilkan detil: ");
                        if (int.TryParse(Console.ReadLine(), out int showId))
                        {
                            var citation = controller.GetCitation(showId);
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
                        Console.Write("Tipe (Buku, Article jurnal, Website, makalah konferensi, Tesis): ");
                        if (Enum.TryParse<CitationType>(Console.ReadLine(), out CitationType newType))
                        {
                            Console.Write("Judul: ");
                            string newTitle = Console.ReadLine();
                            Console.Write("Pengarang: ");
                            string newAuthor = Console.ReadLine();
                            Console.Write("Info publikasi: ");
                            string newPublicationInfo = Console.ReadLine();

                            controller.AddCitation(newType, newTitle, newAuthor, newPublicationInfo);
                            Console.WriteLine("sitasi berhasil ditambahkan.");
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
                                Console.Write($"Tipe baru ({existingCitation.Type}): ");
                                string? typeInput = Console.ReadLine();
                                CitationType? updatedType = string.IsNullOrEmpty(typeInput) ? existingCitation.Type : Enum.Parse<CitationType>(typeInput);

                                Console.Write($"Judul baru ({existingCitation.Title}): ");
                                string? updatedTitle = string.IsNullOrEmpty(Console.ReadLine()) ? existingCitation.Title : Console.ReadLine();

                                Console.Write($"Pengarang baru ({existingCitation.Author}): ");
                                string? updatedAuthor = string.IsNullOrEmpty(Console.ReadLine()) ? existingCitation.Author : Console.ReadLine();

                                Console.Write($"Info publikasi baru ({existingCitation.PublicationInfo}): ");
                                string? updatedPublicationInfo = string.IsNullOrEmpty(Console.ReadLine()) ? existingCitation.PublicationInfo : Console.ReadLine();

                                Console.Write($"Tanggal publikasi baru (yyyy-mm-dd) ({existingCitation.PublicationDate?.ToShortDateString() ?? "N/A"}): ");
                                DateTime? updatedPublicationDate = DateTime.TryParse(Console.ReadLine(), out DateTime date) ? date : existingCitation.PublicationDate;

                                Console.Write($"Tanggal akses baru ({existingCitation.AccessDate ?? "N/A"}): ");
                                string? updatedAccessDate = string.IsNullOrEmpty(Console.ReadLine()) ? existingCitation.AccessDate : Console.ReadLine();

                                Console.Write($"DOI baru ({existingCitation.DOI ?? "N/A"}): ");
                                string? updatedDOI = string.IsNullOrEmpty(Console.ReadLine()) ? existingCitation.DOI : Console.ReadLine();

                                controller.UpdateCitation(updateId, updatedType, updatedTitle, updatedAuthor, updatedPublicationInfo, updatedPublicationDate, updatedAccessDate, updatedDOI);
                                Console.WriteLine("Sitasi berhasil ditambahkan.");
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
                            controller.DeleteCitation(deleteId);
                            Console.WriteLine($"Sitasi dengan ID {deleteId} dihapus.");
                        }
                        else
                        {
                            Console.WriteLine("ID tidak valid.");
                        }
                        break;
                    case "6":
                        var bibliography = controller.GenerateBibliography();
                        view.DisplayBibliography(bibliography);
                        break;
                    case "7":
                        Console.Write("Masukkan ID sitasi untuk membuat teks jenis APA: ");
                        if (int.TryParse(Console.ReadLine(), out int citationId))
                        {
                            string citationText = controller.GenerateCitationText(citationId);
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