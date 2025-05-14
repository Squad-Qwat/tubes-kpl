using PaperNest_API.Controllers;
using PaperNest_API.Models;
using PaperNest_API.Utils;
using PaperNest_API.Services;
using PaperNest_API.Views;
using Microsoft.AspNetCore.Mvc;

namespace PaperNest_API.View
{
    public class CLIView
    {
        private readonly UserController _userController;
        private readonly AuthController _authController;
        private readonly WorkspaceController _workspaceController;
        private readonly DocumentController _documentController;
        private readonly ResearchRequestController _researchRequestController;
        private readonly ReviewService _researchRequestManager;
        private readonly AuthStateMachine _authState;
        private User? _currentUser;
        private Workspace? _currentWorkspace;
        private readonly CitationView _citationView; // Add this line

        public CLIView()
        {
            _userController = new UserController();
            _authController = new AuthController();
            _workspaceController = new WorkspaceController();
            _documentController = new DocumentController();
            _researchRequestManager = new ReviewService();
            _researchRequestController = new ResearchRequestController(); 
            _authState = new AuthStateMachine();
            _currentUser = null;
            _currentWorkspace = null;
            _citationView = new CitationView(); // Initialize CitationView
        }

        public void Start()
        {
            bool isRunning = true;
            
            Console.WriteLine("=== PaperNest - Sistem Manajemen Karya Tulis Ilmiah ===");
            
            while (isRunning)
            {
                if (_authState.GetCurrentState() == AuthStateMachine.AuthState.BELUM_LOGIN)
                {
                    DisplayLoginMenu();
                }
                else
                {
                    DisplayMainMenu();
                }
                
                Console.WriteLine("\nTekan tombol apa saja untuk melanjutkan...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void DisplayLoginMenu()
        {
            Console.WriteLine("\n=== Menu Autentikasi ===");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("0. Keluar");
            Console.Write("Pilih menu: ");
            
            string? choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1":
                    Login();
                    break;
                case "2":
                    Register();
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Menu tidak valid. Silakan coba lagi.");
                    break;
            }
        }

        private void DisplayMainMenu()
        {
            Console.WriteLine($"\n=== Menu Utama - Selamat datang, {_currentUser?.Name} ===");
            if (_currentUser?.Role == "Mahasiswa")
            {
                Console.WriteLine("1. Buat Workspace");
                Console.WriteLine("2. Lihat Workspace");
                Console.WriteLine("3. Kelola Workspace");
            }
            else if (_currentUser?.Role == "Dosen")
            {
                Console.WriteLine("1. Lihat Workspace");
                Console.WriteLine("2. Lihat Permintaan Review");
                Console.WriteLine("3. Kelola Review");
                Console.WriteLine("4. Bergabung dengan Workspace");
            }else{
                Console.WriteLine("Role tidak dikenali. Silakan coba lagi.");
            }
            
            Console.WriteLine("8. Lihat Profil");
            Console.WriteLine("9. Logout");
            Console.Write("Pilih menu: ");
            
            string? choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1":
                    if (_currentUser?.Role == "Mahasiswa")
                    {
                        CreateNewWorkspace();
                    }
                    else if (_currentUser?.Role == "Dosen")
                    {
                        ViewWorkspaces();
                    }
                    else
                    {
                        Console.WriteLine("Menu tidak valid untuk role Anda.");
                    }
                    break;
                case "2":
                    if (_currentUser?.Role == "Mahasiswa")
                    {
                        ViewWorkspaces();
                    }
                    else if (_currentUser?.Role == "Dosen")
                    {
                        ViewPendingReviews();
                    }
                    else
                    {
                        Console.WriteLine("Menu tidak valid untuk role Anda.");
                    }
                    break;
                case "3":
                    if (_currentUser?.Role == "Mahasiswa")
                    {
                        ManageWorkspaces();
                    }
                    else if (_currentUser?.Role == "Dosen")
                    {
                        ManageReviews();
                    }
                    else
                    {
                        Console.WriteLine("Menu tidak valid untuk role Anda.");
                    }
                    break;
                case "4":
                    if (_currentUser?.Role == "Dosen")
                    {
                        JoinWorkspace();
                    }
                    else
                    {
                        Console.WriteLine("Menu tidak valid untuk role Anda.");
                    }
                    break;
                case "8":
                    DisplayUserProfile();
                    break;
                case "9":
                    Logout();
                    break;
                default:
                    Console.WriteLine("Menu tidak valid. Silakan coba lagi.");
                    break;
            }
        }

        private void Login()
        {
            Console.WriteLine("\n=== Login ===");
            Console.Write("Username: ");
            string? username = Console.ReadLine();
            
            Console.Write("Password: ");
            string? password = Console.ReadLine();
            
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Username dan password tidak boleh kosong!");
                return;
            }
            
            var result = _authController.Login(username, password);
            
            if (result is OkObjectResult okResult)
            {
                _authState.ActivateTrigger(AuthStateMachine.Trigger.LOGIN);
                Console.WriteLine("Login berhasil!");
                
                // Ambil data user
                var user = PaperNest_API.Repository.UserRepository.userRepository.FirstOrDefault(u => u.Username.Equals(username) && u.Password.Equals(password));
                _currentUser = user;
            }
            else if (result is UnauthorizedObjectResult)
            {
                Console.WriteLine("Username atau password salah. Silakan coba lagi.");
            }
        }

        private void Register()
        {
            Console.WriteLine("\n=== Register ===");
            
            Console.Write("Nama: ");
            string? name = Console.ReadLine();
            
            Console.Write("Email: ");
            string? email = Console.ReadLine();
            
            Console.Write("Username: ");
            string? username = Console.ReadLine();
            
            Console.Write("Password: ");
            string? password = Console.ReadLine();
            
            Console.Write("Role (Mahasiswa/Dosen): ");
            string? role = Console.ReadLine();
            
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || 
                string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(role))
            {
                Console.WriteLine("Semua field harus diisi!");
                return;
            }
            
            if (role != "Mahasiswa" && role != "Dosen")
            {
                Console.WriteLine("Role harus Mahasiswa atau Dosen!");
                return;
            }
            
            var user = new User
            {
                Name = name,
                Email = email,
                Username = username,
                Password = password,
                Role = role
            };
            
            var result = _authController.Register(user);
            
            if (result is OkObjectResult)
            {
                Console.WriteLine("Registrasi berhasil! Silakan login.");
            }
        }

        private void Logout()
        {
            _currentUser = null;
            _authState.ActivateTrigger(AuthStateMachine.Trigger.LOGOUT);
            Console.WriteLine("Logout berhasil.");
        }

        private void DisplayUserProfile()
        {
            if (_currentUser == null)
            {
                Console.WriteLine("Tidak ada user yang sedang login.");
                return;
            }
            
            Console.WriteLine("\n=== Profil User ===");
            Console.WriteLine($"ID: {_currentUser.Id}");
            Console.WriteLine($"Nama: {_currentUser.Name}");
            Console.WriteLine($"Email: {_currentUser.Email}");
            Console.WriteLine($"Username: {_currentUser.Username}");
            Console.WriteLine($"Role: {_currentUser.Role}");
            Console.WriteLine($"Dibuat pada: {_currentUser.Created_at}");
        }

        // Method untuk membuat workspace baru
        private void CreateNewWorkspace()
        {
            if (_currentUser == null)
            {
                Console.WriteLine("Anda harus login terlebih dahulu.");
                return;
            }
            
            Console.WriteLine("\n=== Buat Workspace Baru ===");
            
            Console.Write("Nama Workspace: ");
            string? name = Console.ReadLine();
            
            Console.Write("Deskripsi (opsional): ");
            string? description = Console.ReadLine();
            
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Nama workspace tidak boleh kosong!");
                return;
            }
            
            var workspace = new Workspace
            {
                Name = name,
                Description = description,
                User_id = _currentUser.Id,
                Updated_at = DateTime.Now
            };
            
            var result = _workspaceController.CreateWorkspace(workspace);
            
            if (result is ObjectResult okResult && okResult.StatusCode >= 200 && okResult.StatusCode < 300)
            {
                Console.WriteLine("Workspace berhasil dibuat!");
            }
            else
            {
                Console.WriteLine("Gagal membuat workspace.");
            }
        }
        
        // Method untuk melihat semua workspace
        private void ViewWorkspaces()
        {
            if (_currentUser == null)
            {
                Console.WriteLine("Anda harus login terlebih dahulu.");
                return;
            }
            
            Console.WriteLine("\n=== Daftar Workspace ===");
            
            IActionResult result;
            
            if (_currentUser.Role == "Dosen")
            {
                // Untuk dosen, tampilkan workspace yang diikuti
                result = _workspaceController.GetJoinedWorkspaces(_currentUser.Id);
            }
            else
            {
                // Untuk mahasiswa, tampilkan workspace yang dimiliki
                result = _workspaceController.GetWorkspacesByUserId(_currentUser.Id);
            }
            
            if (result is OkObjectResult okResult)
            {
                dynamic? resultData = okResult.Value;
                var workspaces = resultData?.data as IEnumerable<Workspace>;
                
                if (workspaces == null || !workspaces.Any())
                {
                    if (_currentUser.Role == "Dosen")
                    {
                        Console.WriteLine("Anda belum bergabung dengan workspace manapun.");
                        Console.WriteLine("Gunakan menu 'Bergabung dengan Workspace' untuk bergabung.");
                    }
                    else
                    {
                        Console.WriteLine("Belum ada workspace.");
                    }
                    return;
                }
                
                int index = 1;
                foreach (var workspace in workspaces)
                {
                    string description = workspace.Description ?? "Tidak ada deskripsi";
                    string createdAt = workspace.Created_at.ToString("dd/MM/yyyy HH:mm:ss");
                    
                    Console.WriteLine($"{index}. {workspace.Name} [ID: {workspace.Id}]");
                    Console.WriteLine($"   Deskripsi: {description}");
                    Console.WriteLine($"   Dibuat pada: {createdAt}");
                    Console.WriteLine();
                    index++;
                }
                
                Console.Write("Pilih workspace (nomor) atau 0 untuk kembali: ");
                if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= workspaces.Count())
                {
                    _currentWorkspace = workspaces.ElementAt(choice - 1);
                    WorkspaceMenu();
                }
            }
            else
            {
                Console.WriteLine("Gagal mendapatkan daftar workspace.");
            }
        }
        
        // Method untuk mengelola workspace
        private void ManageWorkspaces()
        {
            if (_currentUser == null)
            {
                Console.WriteLine("Anda harus login terlebih dahulu.");
                return;
            }
            
            ViewWorkspaces(); // Tampilkan daftar workspace saat user tidak null
        }
        
        // Menu untuk workspace yang dipilih
        private void WorkspaceMenu()
        {
            if (_currentWorkspace == null)
            {
                Console.WriteLine("Tidak ada workspace yang dipilih.");
                return;
            }
            
            bool backToMainMenu = false;
            
            while (!backToMainMenu)
            {
                Console.WriteLine($"\n=== Workspace: {_currentWorkspace.Name} ===");
                
                // Tampilkan menu yang berbeda berdasarkan role
                if (_currentUser?.Role == "Dosen")
                {
                    // Menu terbatas untuk dosen - hanya lihat dokumen
                    Console.WriteLine("1. Lihat Dokumen");
                    Console.WriteLine("0. Kembali ke Menu Utama");
                }
                else
                {
                    // Menu lengkap untuk mahasiswa
                    Console.WriteLine("1. Lihat Dokumen");
                    Console.WriteLine("2. Buat Dokumen Baru");
                    Console.WriteLine("3. Edit Info Workspace");
                    Console.WriteLine("4. Hapus Workspace");
                    Console.WriteLine("0. Kembali ke Menu Utama");
                }
                
                Console.Write("Pilih menu: ");
                
                string? choice = Console.ReadLine();
                
                if (_currentUser?.Role == "Dosen")
                {
                    // Pilihan terbatas untuk dosen
                    switch (choice)
                    {
                        case "1":
                            ViewDocuments();
                            break;
                        case "0":
                            backToMainMenu = true;
                            _currentWorkspace = null;
                            break;
                        default:
                            Console.WriteLine("Menu tidak valid. Silakan coba lagi.");
                            break;
                    }
                }
                else
                {
                    // Pilihan lengkap untuk mahasiswa
                    switch (choice)
                    {
                        case "1":
                            ViewDocuments();
                            break;
                        case "2":
                            CreateNewDocument();
                            break;
                        case "3":
                            EditWorkspace();
                            break;
                        case "4":
                            if (DeleteWorkspace())
                            {
                                backToMainMenu = true;
                            }
                            break;
                        case "0":
                            backToMainMenu = true;
                            _currentWorkspace = null;
                            break;
                        default:
                            Console.WriteLine("Menu tidak valid. Silakan coba lagi.");
                            break;
                    }
                }
                
                if (!backToMainMenu)
                {
                    Console.WriteLine("\nTekan tombol apa saja untuk melanjutkan...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
        
        // Method untuk melihat dokumen dalam workspace
        private void ViewDocuments()
        {
            if (_currentWorkspace == null)
            {
                Console.WriteLine("Tidak ada workspace yang dipilih.");
                return;
            }
            
            Console.WriteLine($"\n=== Dokumen di Workspace: {_currentWorkspace.Name} ===");
            
            var result = _documentController.GetDocumentsByWorkspaceId(_currentWorkspace.Id);
            
            if (result is OkObjectResult okResult)
            {
                dynamic? resultData = okResult.Value;
                // Sebelum perubahan: IEnumerable<Document>? documents = resultData?.data as IEnumerable<Document>;

                if (resultData?.data is not IEnumerable<Document> documents || !documents.Any()) // Sebelum perubahan: 'documents == null || !documents.Any()'
                {
                    Console.WriteLine("Belum ada dokumen di workspace ini.");
                    return;
                }

                int index = 1;
                foreach (var document in documents)
                {
                    // Cek status draft dari properti HasDraft
                    string draftInfo = "";
                    
                    if (document.HasDraft && document.LastEditedByUserId.HasValue)
                    {
                        var lastEditor = Repository.UserRepository.userRepository.FirstOrDefault(u => u.Id == document.LastEditedByUserId.Value);
                        string lastEditorName = lastEditor?.Name ?? "Pengguna lain";
                        draftInfo = $"[Draft tersedia - terakhir diedit oleh {lastEditorName}]";
                    }
                    
                    Console.WriteLine($"{index}. {document.Title} {draftInfo}");
                    Console.WriteLine($"   Deskripsi: {document.Description ?? "Tidak ada deskripsi"}");
                    Console.WriteLine($"   Dibuat pada: {document.Created_at.ToString("dd/MM/yyyy HH:mm:ss")}");
                    Console.WriteLine();
                    index++;
                }
                
                Console.Write("Pilih dokumen (nomor) atau 0 untuk kembali: ");
                if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= documents.Count())
                {
                    var selectedDocument = documents.ElementAt(choice - 1);
                    DocumentMenu(selectedDocument);
                }
            }
            else
            {
                Console.WriteLine("Gagal mendapatkan daftar dokumen.");
            }
        }
        
        // Method untuk membuat dokumen baru
        private void CreateNewDocument()
        {
            if (_currentUser == null || _currentWorkspace == null)
            {
                Console.WriteLine("Anda harus login dan memilih workspace terlebih dahulu.");
                return;
            }
            
            Console.WriteLine("\n=== Buat Dokumen Baru ===");
            
            Console.Write("Judul: ");
            string? title = Console.ReadLine();
            
            Console.Write("Deskripsi (opsional): ");
            string? description = Console.ReadLine();
            
            Console.Write("Konten: ");
            string? content = Console.ReadLine();            
            
            if (string.IsNullOrEmpty(title))
            {
                Console.WriteLine("Judul tidak boleh kosong!");
                return;
            }

            /*
            var id = Guid.NewGuid(); // Generate a new ID for the document

            var document = new Document(id, title, _currentUser.Id, _currentWorkspace.Id)
            {
                Id = id,
                Title = title,
                Description = description,
                Content = content,
                User_id = _currentUser.Id,
                Workspace_id = _currentWorkspace.Id,
                Updated_at = DateTime.Now
            };
            */

            var document = new DocumentCreateDto
            {
                Title = title,
                Description = description,
                UserId = _currentUser.Id,
                WorkspaceId = _currentWorkspace.Id
            };

            var result = _documentController.CreateDocument(document);
            
            if (result is ObjectResult okResult && okResult.StatusCode >= 200 && okResult.StatusCode < 300)
            {
                Console.WriteLine("Dokumen berhasil dibuat!");
            }
            else
            {
                Console.WriteLine("Gagal membuat dokumen.");
            }
        }

        // Menu untuk dokumen yang dipilih
        private void DocumentMenu(Document? document)
        {
            if (document == null)
            {
                Console.WriteLine("Tidak ada dokumen yang dipilih.");
                return;
            }

            bool backToWorkspaceMenu = false;

            while (!backToWorkspaceMenu)
            {
                // Tampilkan informasi tentang draft jika ada
                string draftInfo = "";

                // Kode ini kena peringatan karena ada potensi nilai null, jadi gw kasih null checking biar dia nggak null
                if (document != null && document.HasDraft == true && document.LastEditedByUserId.HasValue)
                {
                    var lastEditor = Repository.UserRepository.userRepository.FirstOrDefault(u => u.Id == document.LastEditedByUserId.Value);
                    string lastEditorName = lastEditor?.Name ?? "Pengguna lain";
                    draftInfo = $"\nAda draft tersedia (terakhir diedit oleh {lastEditorName} pada {document.LastEditedAt?.ToString("dd/MM/yyyy HH:mm:ss") ?? "waktu tidak diketahui"})";
                }
                
                Console.WriteLine($"\n=== Dokumen: {document?.Title} ===");
                Console.WriteLine($"Deskripsi: {document?.Description ?? "Tidak ada deskripsi"}");
                Console.WriteLine($"Konten: {document?.Content ?? "Tidak ada konten"}");
                Console.WriteLine($"Dibuat pada: {document?.Created_at.ToString("dd/MM/yyyy HH:mm:ss")}");
                
                if (_currentUser?.Role != "Dosen")
                {
                    Console.WriteLine(draftInfo);
                }
                
                // Tampilkan menu yang berbeda berdasarkan role
                if (_currentUser?.Role == "Dosen")
                {
                    // Menu terbatas untuk dosen
                    Console.WriteLine("\n1. Lihat Versi Dokumen");
                    Console.WriteLine("2. Review Dokumen");
                    Console.WriteLine("0. Kembali ke Menu Workspace");
                }
                else
                {
                    // Menu lengkap untuk mahasiswa
                    Console.WriteLine("\n1. Edit Dokumen (Judul, Deskripsi)");
                    Console.WriteLine("2. Edit Konten Dokumen");
                    Console.WriteLine("3. Hapus Dokumen");
                    Console.WriteLine("4. Manajemen Versi Dokumen");
                    Console.WriteLine("0. Kembali ke Menu Workspace");
                }
                
                Console.Write("Pilih menu: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        if (document == null)
                        {
                            Console.WriteLine("Tidak ada dokumen yang dipilih.");
                            break;
                        }

                        EditDocumentMetadata(document);
                        // Refresh document data
                        var refreshResult = _documentController.GetDocumentById(document.Id);
                        if (refreshResult is OkObjectResult okResult)
                        {
                            dynamic? resultData = okResult.Value;
                            document = resultData?.data as Document;
                        }
                        break;
                    case "2":
                        if(document == null)
                        {
                            Console.WriteLine("Tidak ada dokumen yang dipilih.");
                            break;
                        }

                        EditDocumentContent(document);
                        // Refresh document data
                        var refreshContentResult = _documentController.GetDocumentById(document.Id);
                        if (refreshContentResult is OkObjectResult contentOkResult)
                        {
                            dynamic? resultData = contentOkResult.Value;
                            document = resultData?.data as Document;
                        }
                        break;
                    case "3":
                        if(document == null)
                        {
                            Console.WriteLine("Tidak ada dokumen yang dipilih.");
                            break;
                        }

                        if (DeleteDocument(document.Id))
                        {
                            backToWorkspaceMenu = true;
                        }
                        break;
                    case "4":
                        ManageDocumentVersions(document);
                        break;
                    case "5": // Added
                        if (document == null)
                        {
                            Console.WriteLine("Tidak ada dokumen yang dipilih.");
                            break;
                        }

                        GenerateCitation(document);
                        break;
                    case "6":
                        // Added
                        if (document == null)
                        {
                            Console.WriteLine("Tidak ada dokumen yang dipilih.");
                            break;
                        }
                        SubmitDocumentForReview(document);
                        break;
                    case "0":
                        backToWorkspaceMenu = true;
                        break;
                    default:
                        Console.WriteLine("Menu tidak valid. Silakan coba lagi.");
                        break;
                }

                if (!backToWorkspaceMenu)
                {
                    Console.WriteLine("\nTekan tombol apa saja untuk melanjutkan...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        // Dikasih null checking (?) karena area input (pemanggilan kode di atas) berpotensi nilai null.
        // Method untuk mengedit metadata dokumen (judul, deskripsi, status)
        private void EditDocumentMetadata(Document? document)
        {
            if (document == null)
            {
                Console.WriteLine("Tidak ada dokumen yang dipilih.");
                return;
            }
            
            Console.WriteLine($"\n=== Edit Dokumen: {document.Title} ===");
            
            Console.Write($"Judul Baru (kosongkan untuk tetap '{document.Title}'): ");
            string? title = Console.ReadLine();
            
            Console.Write($"Deskripsi Baru (kosongkan untuk tetap '{document.Description ?? "kosong"}'): ");
            string? description = Console.ReadLine();
            
            // Hapus pertanyaan Is_public karena sudah tidak digunakan lagi
            
            if (!string.IsNullOrEmpty(title))
            {
                document.Title = title;
            }
            
            if (!string.IsNullOrEmpty(description))
            {
                document.Description = description;
            }
            
            document.Updated_at = DateTime.Now;
            
            var result = _documentController.UpdateDocument(document.Id, document);
            
            if (result is OkObjectResult)
            {
                Console.WriteLine("Metadata dokumen berhasil diperbarui!");
            }
            else
            {
                Console.WriteLine("Gagal memperbarui metadata dokumen.");
            }
        }

        // Method untuk mengedit konten dokumen
        private void EditDocumentContent(Document? document)
        {
            if (document == null)
            {
                Console.WriteLine("Tidak ada dokumen yang dipilih.");
                return;
            }
            
            if (_currentUser == null)
            {
                Console.WriteLine("Anda harus login terlebih dahulu.");
                return;
            }
            
            Console.WriteLine($"\n=== Edit Konten Dokumen: {document.Title} ===");
            
            // Ambil konten dari dokumen saat ini
            string initialContent = document.Content ?? string.Empty;
            
            // Tampilkan info jika dokumen memiliki draft (konten yang belum diversion)
            if (document.HasDraft && document.LastEditedByUserId.HasValue)
            {
                var lastEditor = Repository.UserRepository.userRepository.FirstOrDefault(u => u.Id == document.LastEditedByUserId);
                string lastEditorName = lastEditor?.Name ?? "Pengguna lain";
                
                Console.WriteLine($"Melanjutkan pengeditan draft dari {lastEditorName}");
                if (document.LastEditedAt.HasValue)
                {
                    Console.WriteLine($"Terakhir diubah: {document.LastEditedAt.Value}");
                }
            }
            else
            {
                Console.WriteLine($"Konten Sebelumnya:\n{initialContent}\n");
            }
            
            Console.WriteLine("Masukkan konten baru (bisa dimodifikasi dari konten sebelumnya):");
            
            // Gunakan hanya metode interaktif
            string newContent = ReadAndEditMultilineText(initialContent);
            
            if (newContent == initialContent)
            {
                Console.WriteLine("Tidak ada perubahan pada konten.");
                return;
            }
            
            // Simpan draft langsung ke document.Content
            document.Content = newContent;
            document.LastEditedByUserId = _currentUser.Id;
            document.LastEditedAt = DateTime.Now;
            document.HasDraft = true;  // Tandai bahwa ini adalah draft
            document.Updated_at = DateTime.Now;
            
            var result = _documentController.UpdateDocument(document.Id, document);
            
            if (result is OkObjectResult)
            {
                Console.WriteLine("Konten dokumen berhasil diperbarui!");
                Console.WriteLine("Catatan: Versi baru belum dibuat. Untuk membuat versi baru, silakan pilih menu 'Kirim Versi Baru' di menu Manajemen Versi Dokumen.");
                Console.WriteLine("Draft tersimpan dan dapat dilihat oleh semua anggota workspace.");
            }
            else
            {
                Console.WriteLine("Gagal memperbarui konten dokumen.");
            }
        }

        // Method untuk manajemen versi dokumen
        private void ManageDocumentVersions(Document? document)
        {
            if (document == null)
            {
                Console.WriteLine("Tidak ada dokumen yang dipilih.");
                return;
            }
            
            bool backToDocumentMenu = false;
            
            while (!backToDocumentMenu)
            {
                Console.WriteLine($"\n=== Manajemen Versi Dokumen: {document.Title} ===");
                Console.WriteLine("1. Lihat Semua Versi");
                Console.WriteLine("2. Kirim Versi Baru");
                Console.WriteLine("3. Rollback ke Versi Sebelumnya");
                Console.WriteLine("0. Kembali ke Menu Dokumen");
                Console.Write("Pilih menu: ");
                
                string? choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        ViewDocumentVersions(document.Id);
                        break;
                    case "2":
                        CreateNewDocumentVersion(document);
                        break;
                    case "3":
                        RollbackDocumentVersion(document);
                        break;
                    case "0":
                        backToDocumentMenu = true;
                        break;
                    default:
                        Console.WriteLine("Menu tidak valid. Silakan coba lagi.");
                        break;
                }
                
                if (!backToDocumentMenu)
                {
                    Console.WriteLine("\nTekan tombol apa saja untuk melanjutkan...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        // Method untuk membuat versi baru dokumen tanpa mengedit konten
        private void CreateNewDocumentVersion(Document document)
        {
            if (document == null)
            {
                Console.WriteLine("Tidak ada dokumen yang dipilih.");
                return;
            }
            
            Console.WriteLine($"\n=== Kirim Versi Baru Dokumen: {document.Title} ===");
            
            // Periksa apakah versi sebelumnya sudah direview
            if (!DocumentBodyService.CanCreateNewVersion(document.Id))
            {
                Console.WriteLine("Tidak dapat membuat versi baru karena masih ada versi yang belum direview oleh dosen.");
                Console.WriteLine("Harap tunggu hingga versi sebelumnya direview sebelum membuat versi baru.");
                return;
            }
            
            // Gunakan konten dari dokumen saat ini (yang mungkin berisi draft)
            string currentContent = document.Content ?? string.Empty;
            bool hasDraft = document.HasDraft;
            
            if (hasDraft && document.LastEditedByUserId.HasValue)
            {
                var lastEditor = Repository.UserRepository.userRepository.FirstOrDefault(u => u.Id == document.LastEditedByUserId.Value);
                string lastEditorName = lastEditor?.Name ?? "Pengguna lain";
                
                Console.WriteLine($"Menggunakan draft yang diedit oleh {lastEditorName} pada {document.LastEditedAt}:");
            }
            else
            {
                Console.WriteLine("Menggunakan konten saat ini (belum ada draft):");
            }
            
            Console.WriteLine(currentContent);
            
            if (string.IsNullOrWhiteSpace(currentContent))
            {
                Console.WriteLine("Tidak dapat membuat versi baru karena konten kosong.");
                return;
            }
            
            // Periksa apakah sudah ada versi sebelumnya dengan konten yang sama
            var versions = DocumentBodyService.GetVersions(document.Id);
            if (versions != null && versions.Any())
            {
                var latestVersion = versions.FirstOrDefault();
                if (latestVersion != null && latestVersion.Content == currentContent)
                {
                    Console.WriteLine("Tidak dapat mengirim versi baru karena konten sama dengan versi sebelumnya.");
                    Console.WriteLine("Silakan ubah konten dokumen terlebih dahulu.");
                    return;
                }
            }
            
            Console.Write("\nDeskripsi versi baru (seperti commit message): ");
            string description = Console.ReadLine() ?? "New version";
            
            var newVersion = DocumentBodyService.CreateVersion(document.Id, currentContent, description);
            document.Updated_at = DateTime.Now;
            
            // Reset draft status setelah konten dijadikan versi
            if (hasDraft)
            {
                document.HasDraft = false;
            }
            
            var result = _documentController.UpdateDocument(document.Id, document);
            
            if (result is OkObjectResult)
            {
                Console.WriteLine("Versi baru dokumen berhasil dikirim!");
                Console.WriteLine("Versi ini perlu direview oleh dosen sebelum Anda dapat membuat versi baru lagi.");
                
                // Informasikan bahwa draft telah menjadi versi baru
                if (hasDraft)
                {
                    Console.WriteLine("Draft telah dikonversi menjadi versi baru.");
                }
            }
            else
            {
                Console.WriteLine("Gagal mengirim versi baru dokumen.");
            }
        }
        
        // Method untuk melihat semua versi dokumen
        private void ViewDocumentVersions(Guid documentId)
        {
            Console.WriteLine("\n=== Daftar Versi Dokumen ===");
            
            var versions = DocumentBodyService.GetVersions(documentId);
            
            if (versions == null || !versions.Any())
            {
                Console.WriteLine("Belum ada versi dokumen.");
                return;
            }
            
            int index = 1;
            foreach (var version in versions)
            {
                Console.WriteLine($"{index}. Versi dari {version.Created_at.ToString("dd/MM/yyyy HH:mm:ss")}");
                Console.WriteLine($"   {(version.IsCurrentVersion ? "[AKTIF]" : "")}");
                Console.WriteLine($"   Deskripsi: {version.VersionDescription}");
                // Tampilkan preview konten (maksimal 50 karakter)
                string contentPreview = version.Content.Length > 50 
                    ? version.Content.Substring(0, 50) + "..." 
                    : version.Content;
                Console.WriteLine($"   Preview: {contentPreview}");
                Console.WriteLine();
                index++;
            }
            
            Console.Write("Pilih versi untuk melihat detail (nomor) atau 0 untuk kembali: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= versions.Count())
            {
                var selectedVersion = versions.ElementAt(choice - 1);
                ViewVersionDetail(selectedVersion);
            }
        }

        // Method untuk melihat detail versi
        private void ViewVersionDetail(DocumentBody version)
        {
            if (version == null)
            {
                Console.WriteLine("Versi tidak ditemukan.");
                return;
            }
            
            Console.WriteLine($"\n=== Detail Versi {version.Id} ===");
            Console.WriteLine($"Dibuat pada: {version.Created_at.ToString("dd/MM/yyyy HH:mm:ss")}");
            Console.WriteLine($"Status: {(version.IsCurrentVersion ? "Aktif" : "Tidak Aktif")}");
            Console.WriteLine($"Deskripsi: {version.VersionDescription}");
            Console.WriteLine("\nKonten:");
            Console.WriteLine(version.Content);
        }

        // Method untuk rollback ke versi sebelumnya
        private void RollbackDocumentVersion(Document document)
        {
            if (document == null)
            {
                Console.WriteLine("Tidak ada dokumen yang dipilih.");
                return;
            }
            
            Console.WriteLine($"\n=== Rollback Dokumen: {document.Title} ===");
            
            var versions = DocumentBodyService.GetVersions(document.Id);
            
            if (versions == null || !versions.Any())
            {
                Console.WriteLine("Belum ada versi dokumen untuk rollback.");
                return;
            }
            
            int index = 1;
            foreach (var version in versions)
            {
                if (!version.IsCurrentVersion) // Tampilkan hanya versi non-aktif
                {
                    Console.WriteLine($"{index}. Versi dari {version.Created_at.ToString("dd/MM/yyyy HH:mm:ss")}");
                    Console.WriteLine($"   Deskripsi: {version.VersionDescription}");
                    // Tampilkan preview konten (maksimal 50 karakter)
                    string contentPreview = version.Content.Length > 50 
                        ? version.Content.Substring(0, 50) + "..." 
                        : version.Content;
                    Console.WriteLine($"   Preview: {contentPreview}");
                    Console.WriteLine();
                }
                index++;
            }
            
            Console.Write("Pilih versi untuk rollback (nomor) atau 0 untuk kembali: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= versions.Count())
            {
                var selectedVersion = versions.ElementAt(choice - 1);
                
                Console.Write($"Anda yakin ingin rollback ke versi dari {selectedVersion.Created_at}? (y/n): ");
                string? confirmation = Console.ReadLine();
                
                if (confirmation?.ToLower() == "y")
                {
                    var newVersion = DocumentBodyService.RollbackToVersion(document.Id, selectedVersion.Id);
                    document.Content = newVersion.Content;
                    document.Updated_at = DateTime.Now;
                    
                    var result = _documentController.UpdateDocument(document.Id, document);
                    
                    if (result is OkObjectResult)
                    {
                        Console.WriteLine("Rollback berhasil!");
                    }
                    else
                    {
                        Console.WriteLine("Gagal melakukan rollback.");
                    }
                }
                else
                {
                    Console.WriteLine("Rollback dibatalkan.");
                }
            }
        }

        // Method to submit a document for review (the "push" action)
        private void SubmitDocumentForReview(Document? document)
        {
            if (_currentUser == null)
            {
                Console.WriteLine("Anda harus login terlebih dahulu.");
                return;
            }
            if (document == null)
            {
                Console.WriteLine("Tidak ada dokumen yang dipilih.");
                return;
            }

            Console.WriteLine($"\n=== Ajukan Dokumen untuk Review: {document.Title} ===");
            Console.WriteLine("Dokumen akan diajukan dengan konten draft saat ini (jika ada) atau konten aktif.");

            Console.Write("Judul Pengajuan (kosongkan untuk menggunakan judul dokumen): ");
            string? submissionTitle = Console.ReadLine();

            Console.Write("Abstrak Pengajuan: ");
            string? submissionAbstract = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(submissionAbstract))
            {
                Console.WriteLine("Abstrak pengajuan tidak boleh kosong.");
                return;
            }

            var submissionDto = new ResearchRequestSubmissionDto
            {
                UserId = _currentUser.Id,
                Title = string.IsNullOrWhiteSpace(submissionTitle) ? document.Title : submissionTitle,
                AbstractText = submissionAbstract
            };

            var result = _documentController.SubmitDocumentForReview(document.Id, submissionDto);

            if (result is CreatedAtActionResult createdResult)
            {
                dynamic? resultData = createdResult.Value;
                Console.WriteLine($"Dokumen berhasil diajukan untuk review! Request ID: {resultData?.Id}");
            }
            else if (result is ObjectResult errorResult)
            {
                dynamic? errorData = errorResult.Value;
                Console.WriteLine($"Gagal mengajukan dokumen untuk review: {errorData?.message}");
            }
            else
            {
                Console.WriteLine("Gagal mengajukan dokumen untuk review.");
            }
        }

        // Method to view review requests (for lecturers)
        private void ViewReviewRequests()
        {
            if (_currentUser == null || _currentUser.Role != "Dosen")
            {
                Console.WriteLine("Anda harus login sebagai dosen untuk melihat permintaan review.");
                return;
            }

            Console.WriteLine("\n=== Daftar Permintaan Review ===");

            var result = _researchRequestController.GetAllRequests(); // Get all requests

            List<ResearchRequest>? requests = null;
            if (result is OkObjectResult okResult && okResult.Value is { } value && value.GetType().GetProperty("data")?.GetValue(value) is List<ResearchRequest> requestList)
            {
                requests = requestList.Where(r => r.State is SubmittedState || r.State is UnderReviewState || r.State is NeedsRevisionState).ToList();
            }

            if (requests == null || !requests.Any())
            {
                Console.WriteLine("Tidak ada permintaan review aktif.");
                return;
            }

            int index = 1;
            foreach (var req in requests)
            {
                Console.WriteLine($"{index}. Judul: {req.Title}");
                Console.WriteLine($"   Peneliti: {req.ResearcherName}");
                Console.WriteLine($"   Tanggal Pengajuan: {req.SubmissionDate.ToShortDateString()}");
                Console.WriteLine($"   Keadaan: {req.State.Name}");
                Console.WriteLine($"   ID Permintaan: {req.Id}");
                Console.WriteLine();
                index++;
            }

            Console.Write("Pilih permintaan review (nomor) untuk diproses atau 0 untuk kembali: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= requests.Count())
            {
                var selectedRequest = requests.ElementAt(choice - 1);
                ProcessReviewRequest(selectedRequest);
            }
        }

        // Method untuk mengedit workspace
        private void EditWorkspace()
        {
            if (_currentWorkspace == null)
            {
                Console.WriteLine("Tidak ada workspace yang dipilih.");
                return;
            }
            
            Console.WriteLine($"\n=== Edit Workspace: {_currentWorkspace.Name} ===");
            
            Console.Write($"Nama Baru (kosongkan untuk tetap '{_currentWorkspace.Name}'): ");
            string? name = Console.ReadLine();
            
            Console.Write($"Deskripsi Baru (kosongkan untuk tetap '{_currentWorkspace.Description ?? "kosong"}'): ");
            string? description = Console.ReadLine();
            
            // Menghapus pertanyaan tentang public/private karena sudah tidak digunakan
            
            if (!string.IsNullOrEmpty(name))
            {
                _currentWorkspace.Name = name;
            }
            
            if (!string.IsNullOrEmpty(description))
            {
                _currentWorkspace.Description = description;
            }
            
            // Menghapus pengaturan Is_public
            
            _currentWorkspace.Updated_at = DateTime.Now;
            
            var result = _workspaceController.UpdateWorkspace(_currentWorkspace.Id, _currentWorkspace);
            
            if (result is OkObjectResult)
            {
                Console.WriteLine("Workspace berhasil diperbarui!");
            }
            else
            {
                Console.WriteLine("Gagal memperbarui workspace.");
            }
        }
        
        // Method untuk menghapus workspace
        private bool DeleteWorkspace()
        {
            if (_currentWorkspace == null)
            {
                Console.WriteLine("Tidak ada workspace yang dipilih.");
                return false;
            }
            
            Console.WriteLine($"\n=== Hapus Workspace: {_currentWorkspace.Name} ===");
            Console.Write("Anda yakin ingin menghapus workspace ini? (y/n): ");
            string? confirmation = Console.ReadLine();
            
            if (confirmation?.ToLower() != "y")
            {
                Console.WriteLine("Penghapusan workspace dibatalkan.");
                return false;
            }
            
            var result = _workspaceController.DeleteWorkspace(_currentWorkspace.Id);
            
            if (result is OkObjectResult)
            {
                Console.WriteLine("Workspace berhasil dihapus!");
                _currentWorkspace = null;
                return true;
            }
            else
            {
                Console.WriteLine("Gagal menghapus workspace.");
                return false;
            }
        }

        private void GenerateCitation(Document document)
        {
            if (document == null)
            {
                Console.WriteLine("Tidak ada dokumen yang dipilih.");
                return;
            }

            Console.WriteLine("\n=== Generate Citation ===");
            Console.WriteLine("1. Tampilkan Sitasi");
            Console.WriteLine("2. Tampilkan Bibliografi");
            Console.WriteLine("3. Tampilkan Sitasi Text");
            Console.Write("Pilih jenis format sitasi: ");

            string? choice = Console.ReadLine();

            if(_currentUser == null)
            {
                Console.WriteLine("Anda harus login terlebih dahulu.");
                return;
            }

            var citation = new Citation(123, CitationType.JournalArticle, document.Title, author: _currentUser.Name, publicationInfo: ""); // Replace with actual publication info
            /*
             {
                Id = 123,
                Type = CitationType.JournalArticle,
                Title = document.Title,
                Author = _currentUser.Name,  // Replace with actual author logic
                PublicationInfo = "Jurnal Ilmiah XYZ, Vol. 10, No. 1, pp. 45-60, 2023", // Make dynamic
                PublicationDate = DateTime.Now, // Make dynamic
                AccessDate = "2024-07-24", // Make dynamic
                DOI = "10.1234/xyz.12345" // Make dynamic
            };
            */
            var bibliography = new List<string>
            {
                $"{citation.Author}. ({citation.PublicationDate?.Year}). {citation.Title}. {citation.PublicationInfo}.",
                 // Add more bibliography entries as needed.
            };
            string citationText = $"{citation.Author} ({citation.PublicationDate?.Year}). {citation.Title}."; // Example, remove page section because it's non-existent

            switch (choice)
            {
                case "1":
                    _citationView.DisplayCitation(citation);
                    break;
                case "2":
                    _citationView.DisplayBibliography(bibliography);
                    break;
                case "3":
                    _citationView.DisplayCitationText(citationText);
                    break;
                default:
                    Console.WriteLine("Pilihan tidak valid.");
                    break;
            }
        }

        // Method untuk menghapus dokumen
        private bool DeleteDocument(Guid documentId)
        {
            Console.WriteLine("\n=== Hapus Dokumen ===");
            Console.Write("Anda yakin ingin menghapus dokumen ini? (y/n): ");
            string? confirmation = Console.ReadLine();
            
            if (confirmation?.ToLower() != "y")
            {
                Console.WriteLine("Penghapusan dokumen dibatalkan.");
                return false;
            }
            
            var result = _documentController.DeleteDocument(documentId);
            
            if (result is OkObjectResult)
            {
                Console.WriteLine("Dokumen berhasil dihapus!");
                return true;
            }
            else
            {
                Console.WriteLine("Gagal menghapus dokumen.");
                return false;
            }
        }

        // Metode pengeditan teks interaktif untuk multiline text
        private string ReadAndEditMultilineText(string initialText)
        {
            // Split text into lines for multiline support
            List<string> lines = initialText.Split(Environment.NewLine).ToList();
            if (lines.Count == 0) lines.Add(string.Empty);
            
            int currentLine = 0;
            bool editing = true;
            
            Console.Clear();
            Console.WriteLine("==== EDITOR INTERAKTIF ====");
            Console.WriteLine("Navigasi: ↑↓ = pindah baris, ←→ = pindah karakter");
            Console.WriteLine("Esc = keluar dan simpan, Ctrl+C = keluar tanpa simpan");
            Console.WriteLine("===============================");
            
            // Display all lines
            Console.WriteLine();
            for (int i = 0; i < lines.Count; i++)
            {
                Console.WriteLine($"{i+1}: {lines[i]}");
            }
            
            // Position cursor at the beginning of first line
            Console.SetCursorPosition(3, 5); // Adjust for header and line numbers
            int cursorX = 3;  // Line number + ": " = 3 characters
            int cursorY = 5;  // Starting line after header
            
            while (editing)
            {
                // Read a key press without displaying it
                var key = Console.ReadKey(true);
                
                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        editing = false;  // Done editing
                        break;
                        
                    case ConsoleKey.UpArrow:
                        if (currentLine > 0)
                        {
                            currentLine--;
                            cursorY--;
                            // Make sure cursor X position is valid for the new line
                            cursorX = Math.Min(lines[currentLine].Length + 3, cursorX);
                            Console.SetCursorPosition(cursorX, cursorY);
                        }
                        break;
                        
                    case ConsoleKey.DownArrow:
                        if (currentLine < lines.Count - 1)
                        {
                            currentLine++;
                            cursorY++;
                            // Make sure cursor X position is valid for the new line
                            cursorX = Math.Min(lines[currentLine].Length + 3, cursorX);
                            Console.SetCursorPosition(cursorX, cursorY);
                        }
                        break;
                        
                    case ConsoleKey.LeftArrow:
                        if (cursorX > 3)  // Don't move left of the line number prefix
                        {
                            cursorX--;
                            Console.SetCursorPosition(cursorX, cursorY);
                        }
                        break;
                        
                    case ConsoleKey.RightArrow:
                        if (cursorX < lines[currentLine].Length + 3)
                        {
                            cursorX++;
                            Console.SetCursorPosition(cursorX, cursorY);
                        }
                        break;
                        
                    case ConsoleKey.Enter:
                        // Insert a new line
                        string currentLineText = lines[currentLine];
                        int posInLine = cursorX - 3;
                        
                        string beforeCursor = posInLine > 0 ? currentLineText.Substring(0, posInLine) : "";
                        string afterCursor = posInLine < currentLineText.Length ? currentLineText.Substring(posInLine) : "";
                        
                        lines[currentLine] = beforeCursor;
                        lines.Insert(currentLine + 1, afterCursor);
                        
                        // Redraw all lines after the current one
                        for (int i = currentLine; i < lines.Count; i++)
                        {
                            Console.SetCursorPosition(0, i + 5);
                            Console.Write(new string(' ', Console.WindowWidth));  // Clear the line
                            Console.SetCursorPosition(0, i + 5);
                            Console.Write($"{i+1}: {lines[i]}");
                        }
                        
                        currentLine++;
                        cursorY++;
                        cursorX = 3;  // Move to beginning of the new line
                        Console.SetCursorPosition(cursorX, cursorY);
                        break;
                        
                    case ConsoleKey.Backspace:
                        int positionInLine = cursorX - 3;
                        if (positionInLine > 0)
                        {
                            // Remove character from current line
                            string lineText = lines[currentLine];
                            lines[currentLine] = lineText.Remove(positionInLine - 1, 1);
                            
                            // Redraw the current line
                            Console.SetCursorPosition(0, cursorY);
                            Console.Write(new string(' ', Console.WindowWidth));  // Clear the line
                            Console.SetCursorPosition(0, cursorY);
                            Console.Write($"{currentLine+1}: {lines[currentLine]}");
                            
                            // Move cursor back one position
                            cursorX--;
                            Console.SetCursorPosition(cursorX, cursorY);
                        }
                        else if (currentLine > 0)
                        {
                            // At beginning of line, join with previous line
                            string previousLine = lines[currentLine - 1];
                            string lineToJoin = lines[currentLine];
                            
                            // Remove the current line and append its content to the previous line
                            lines.RemoveAt(currentLine);
                            int previousLineLength = previousLine.Length;
                            lines[currentLine - 1] = previousLine + lineToJoin;
                            
                            // Redraw all lines from previous line onwards
                            for (int i = currentLine - 1; i < lines.Count; i++)
                            {
                                Console.SetCursorPosition(0, i + 5);
                                Console.Write(new string(' ', Console.WindowWidth));  // Clear the line
                                Console.SetCursorPosition(0, i + 5);
                                Console.Write($"{i+1}: {lines[i]}");
                            }
                            
                            // Clear the last line that's now empty
                            Console.SetCursorPosition(0, lines.Count + 5);
                            Console.Write(new string(' ', Console.WindowWidth));
                            
                            // Update cursor position
                            currentLine--;
                            cursorY--;
                            cursorX = 3 + previousLineLength;
                            Console.SetCursorPosition(cursorX, cursorY);
                        }
                        break;
                        
                    default:
                        // For regular key presses, insert the character at the cursor position
                        if (char.IsLetterOrDigit(key.KeyChar) || char.IsPunctuation(key.KeyChar) || key.KeyChar == ' ')
                        {
                            int pos = cursorX - 3;  // Adjust for line number prefix
                            string line = lines[currentLine];
                            
                            // Insert the character
                            if (pos >= 0 && pos <= line.Length)
                            {
                                lines[currentLine] = line.Insert(pos, key.KeyChar.ToString());
                                
                                // Redraw the current line
                                Console.SetCursorPosition(0, cursorY);
                                Console.Write(new string(' ', Console.WindowWidth));  // Clear the line
                                Console.SetCursorPosition(0, cursorY);
                                Console.Write($"{currentLine+1}: {lines[currentLine]}");
                                
                                // Move cursor forward one position
                                cursorX++;
                                Console.SetCursorPosition(cursorX, cursorY);
                            }
                        }
                        break;
                }
            }
            
            Console.Clear();  // Clear the screen after editing is done
            return string.Join(Environment.NewLine, lines);
        }

        // Tambahkan metode-metode baru untuk dosen
        private void ViewPendingReviews()
        {
            Console.WriteLine("\n=== Permintaan Review Tertunda ===");

            // In a real application, you'd use Dependency Injection for the controller.
            // For this console app context, instantiating directly is acceptable for demo.
            var controller = new ResearchRequestController();

            if (_currentUser == null || _currentUser.Role != "Dosen")
            {
                Console.WriteLine("Anda harus login sebagai dosen untuk melihat permintaan review.");
                return;
            }

            // Call the controller method and cast the result to OkObjectResult to access the Value
            var actionResult = controller.GetRequestsByLecturer(_currentUser.Id);
            List<ResearchRequest>? pendingRequests = null;

            if (actionResult is OkObjectResult okResult)
            {
                // Assuming the 'data' property holds the list of ResearchRequest
                // You might need to cast okResult.Value to an anonymous type or dictionary
                // to access 'data', or define a specific response DTO for the controller.
                // For simplicity, let's assume 'data' is directly accessible from the anonymous object.
                dynamic responseData = okResult.Value;
                pendingRequests = responseData.data as List<ResearchRequest>;
            }
            else if (actionResult is NotFoundObjectResult notFoundResult)
            {
                Console.WriteLine(notFoundResult.Value.GetType().GetProperty("message")?.GetValue(notFoundResult.Value));
                return;
            }
            else
            {
                Console.WriteLine("Terjadi kesalahan saat mengambil permintaan review.");
                return;
            }

            if (pendingRequests == null || pendingRequests.Count == 0)
            {
                Console.WriteLine("Tidak ada permintaan review tertunda.");
                return;
            }

            foreach (var request in pendingRequests)
            {
                Console.WriteLine($"ID: {request.Id} | Judul: {request.Title} | Peneliti: {request.ResearcherName} | Status: {request.State.Name}");
            }

            Console.Write("\nMasukkan ID permintaan untuk melihat detail (kosongkan untuk kembali): ");
            string? input = Console.ReadLine();

            if (!string.IsNullOrEmpty(input) && Guid.TryParse(input, out Guid requestId))
            {
                var selectedRequest = pendingRequests.FirstOrDefault(r => r.Id == requestId);
                if (selectedRequest != null)
                {
                    DisplayRequestDetails(selectedRequest);
                }
                else
                {
                    Console.WriteLine("Permintaan dengan ID tersebut tidak ditemukan.");
                }
            }
        }

private void ManageReviews()
        {
            Console.WriteLine("\n=== Kelola Review ===");

            var controller = new ResearchRequestController();

            // Call the controller method and extract data similarly
            var actionResult = controller.GetRequestsByLecturer(_currentUser.Id);
            List<ResearchRequest>? pendingReviews = null;

            if (actionResult is OkObjectResult okResult)
            {
                dynamic responseData = okResult.Value;
                pendingReviews = responseData.data as List<ResearchRequest>;
            }
            else
            {
                Console.WriteLine("Tidak dapat mengambil daftar permintaan review. Pastikan Anda login sebagai dosen.");
                return;
            }

            if (pendingReviews == null || pendingReviews.Count == 0)
            {
                Console.WriteLine("Tidak ada permintaan review tertunda.");
                return;
            }

            Console.WriteLine("Permintaan review yang tersedia:");
            foreach (var request in pendingReviews)
            {
                Console.WriteLine($"ID: {request.Id} | Judul: {request.Title} | Peneliti: {request.ResearcherName} | Status: {request.State.Name}");
            }

            Console.Write("\nMasukkan ID permintaan untuk mengelola review (kosongkan untuk kembali): ");
            string? input = Console.ReadLine();

            if (!string.IsNullOrEmpty(input) && Guid.TryParse(input, out Guid requestId))
            {
                var selectedRequest = pendingReviews.FirstOrDefault(r => r.Id == requestId);
                if (selectedRequest != null)
                {
                    ProcessReviewRequest(selectedRequest);
                }
                else
                {
                    Console.WriteLine("Permintaan dengan ID tersebut tidak ditemukan.");
                }
            }
        }

        private void ProcessReviewRequest(ResearchRequest request)
        {
            Console.WriteLine($"\n=== Proses Review untuk '{request.Title}' ===");
            Console.WriteLine($"Status saat ini: {request.State.Name}");

            var controller = new ResearchRequestController(); // Controller instance for calls

            if (request.State is SubmittedState)
            {
                Console.WriteLine("1. Mulai review");
                Console.WriteLine("0. Kembali");

                Console.Write("Pilihan: ");
                string? choice = Console.ReadLine();

                if (choice == "1")
                {
                    var actionResult = controller.StartReview(request.Id);
                    if (actionResult is OkObjectResult okResult)
                    {
                        dynamic responseData = okResult.Value;
                        Console.WriteLine(responseData?.message); // Print success message from API
                    }
                    else if (actionResult is BadRequestObjectResult badRequestResult)
                    {
                        dynamic errorData = badRequestResult.Value;
                        Console.WriteLine(errorData?.message); // Print error message from API
                    }
                    else
                    {
                        Console.WriteLine("Gagal memulai review. Terjadi kesalahan tak terduga.");
                    }
                }
            }
            else if (request.State is UnderReviewState || request.State is NeedsRevisionState)
            {
                Console.WriteLine("1. Setujui");
                Console.WriteLine("2. Perlu revisi");
                Console.WriteLine("3. Tolak");
                Console.WriteLine("0. Kembali");

                Console.Write("Pilihan: ");
                string? choice = Console.ReadLine();

                if (choice == "1" || choice == "2" || choice == "3")
                {
                    Console.Write("Masukkan komentar: ");
                    string? comment = Console.ReadLine() ?? "";

                    ReviewResult result = ReviewResult.Approved;

                    switch (choice)
                    {
                        case "1":
                            result = ReviewResult.Approved;
                            break;
                        case "2":
                            result = ReviewResult.NeedsRevision;
                            break;
                        case "3":
                            result = ReviewResult.Rejected;
                            break;
                    }

                    // Create the DTO for ProcessReview
                    var reviewDto = new ProcessReviewDto
                    {
                        ReviewerId = _currentUser.Id,
                        Result = result,
                        ReviewerComment = comment
                    };

                    var actionResult = controller.ProcessReview(request.Id, reviewDto);
                    if (actionResult is OkObjectResult okResult)
                    {
                        dynamic responseData = okResult.Value;
                        Console.WriteLine(responseData?.message); // Print success message from API
                    }
                    else if (actionResult is BadRequestObjectResult badRequestResult)
                    {
                        dynamic errorData = badRequestResult.Value;
                        Console.WriteLine(errorData?.message); // Print error message from API
                    }
                    else if (actionResult is NotFoundObjectResult notFoundResult)
                    {
                        dynamic errorData = notFoundResult.Value;
                        Console.WriteLine(errorData?.message); // Print error message from API
                    }
                    else
                    {
                        Console.WriteLine("Gagal memproses review. Terjadi kesalahan tak terduga.");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Permintaan review dalam status {request.State.Name} tidak dapat diproses.");
                Console.WriteLine("Tekan tombol apa saja untuk kembali...");
                Console.ReadKey();
            }
        }

        private void DisplayRequestDetails(ResearchRequest request)
        {
            Console.WriteLine($"\n=== Detail Permintaan ===");
            Console.WriteLine($"ID: {request.Id}");
            Console.WriteLine($"Judul: {request.Title}");
            Console.WriteLine($"Abstrak: {request.Abstract}");
            Console.WriteLine($"Penulis: {request.ResearcherName}");
            Console.WriteLine($"Tanggal pengajuan: {request.SubmissionDate}");
            Console.WriteLine($"Status: {request.State.Name}");
            
            // Tampilkan review yang sudah diberikan
            if (request.Reviews.Count > 0)
            {
                Console.WriteLine("\n=== Review yang telah diberikan ===");
                foreach (var review in request.Reviews)
                {
                    Console.WriteLine($"Reviewer: {review.ReviewerName}");
                    Console.WriteLine($"Tanggal: {review.ReviewDate}");
                    Console.WriteLine($"Hasil: {review.Result}");
                    Console.WriteLine($"Komentar: {review.Comment}");
                    Console.WriteLine("---------------------");
                }
            }
            else
            {
                Console.WriteLine("\nBelum ada review yang diberikan.");
            }
            
            Console.WriteLine("\nTekan tombol apa saja untuk kembali...");
            Console.ReadKey();
        }

        // Method untuk dosen bergabung dengan workspace
        private void JoinWorkspace()
        {
            if (_currentUser == null)
            {
                Console.WriteLine("Anda harus login terlebih dahulu.");
                return;
            }
            
            if (_currentUser.Role != "Dosen")
            {
                Console.WriteLine("Fitur ini hanya tersedia untuk dosen.");
                return;
            }
            
            Console.WriteLine("\n=== Bergabung dengan Workspace ===");
            Console.WriteLine("Masukkan ID Workspace untuk bergabung.");
            Console.WriteLine("Anda dapat meminta ID Workspace dari mahasiswa pengelola workspace.");
            
            Console.Write("\nID Workspace: ");
            string? workspaceIdStr = Console.ReadLine();
            
            if (string.IsNullOrEmpty(workspaceIdStr))
            {
                Console.WriteLine("ID Workspace tidak boleh kosong!");
                return;
            }
            
            // Coba parse ID workspace
            if (!Guid.TryParse(workspaceIdStr, out Guid workspaceId))
            {
                Console.WriteLine("ID Workspace tidak valid. Pastikan format ID benar.");
                return;
            }
            
            // Cek apakah workspace ada
            var result = _workspaceController.GetWorkspaceById(workspaceId);
            
            if (result is OkObjectResult okResult)
            {
                dynamic? resultData = okResult.Value;
                var workspace = resultData?.data as Workspace;
                
                if (workspace != null)
                {
                    Console.WriteLine($"Workspace ditemukan: {workspace.Name}");
                    Console.WriteLine($"Deskripsi: {workspace.Description ?? "Tidak ada deskripsi"}");
                    Console.WriteLine($"Pemilik: {workspace.User?.Name ?? "Unknown"}");
                    
                    Console.Write("\nApakah Anda yakin ingin bergabung dengan workspace ini? (y/n): ");
                    string? confirmation = Console.ReadLine()?.ToLower();
                    
                    if (confirmation == "y")
                    {
                        // Bergabung dengan workspace
                        var joinResult = _workspaceController.JoinWorkspace(workspaceId, _currentUser.Id);
                        
                        if (joinResult is OkObjectResult joinOkResult)
                        {
                            Console.WriteLine("\nBerhasil bergabung dengan workspace!");
                            Console.WriteLine("Anda sekarang dapat melihat dokumen dan memberikan review pada workspace ini.");
                        }
                        else
                        {
                            Console.WriteLine("\nGagal bergabung dengan workspace. Silakan coba lagi nanti.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nBergabung dengan workspace dibatalkan.");
                    }
                }
                else
                {
                    Console.WriteLine("Workspace tidak ditemukan. Silakan periksa ID workspace.");
                }
            }
            else
            {
                Console.WriteLine("Workspace tidak ditemukan. Silakan periksa ID workspace.");
            }
        }

        // Method baru untuk dosen mereview versi dokumen
        private void ReviewDocumentVersions(Guid documentId)
        {
            Console.WriteLine("\n=== Review Versi Dokumen ===");
            
            // Dapatkan versi dokumen yang sudah di-commit (bukan draft)
            var versions = DocumentBodyService.GetVersions(documentId)
                   .Where(v => v.IsCurrentVersion || (!v.IsCurrentVersion && v.ReviewId == Guid.Empty))
                   .ToList();
            
            if (versions == null || !versions.Any())
            {
                Console.WriteLine("Belum ada versi dokumen yang perlu di-review.");
                return;
            }
            
            int index = 1;
            foreach (var version in versions)
            {
                string reviewStatus = "";
                // Jika reviewId adalah empty, berarti belum direview
                if (version.ReviewId == Guid.Empty)
                {
                    reviewStatus = "[PERLU REVIEW]";
                }
                
                Console.WriteLine($"{index}. Versi dari {version.Created_at.ToString("dd/MM/yyyy HH:mm:ss")} {reviewStatus}");
                Console.WriteLine($"   {(version.IsCurrentVersion ? "[AKTIF]" : "")}");
                Console.WriteLine($"   Deskripsi: {version.VersionDescription}");
                // Tampilkan preview konten (maksimal 50 karakter)
                string contentPreview = version.Content.Length > 50 
                    ? version.Content.Substring(0, 50) + "..." 
                    : version.Content;
                Console.WriteLine($"   Preview: {contentPreview}");
                Console.WriteLine();
                index++;
            }
            
            Console.Write("Pilih versi untuk review (nomor) atau 0 untuk kembali: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= versions.Count())
            {
                var selectedVersion = versions.ElementAt(choice - 1);
                ReviewVersion(selectedVersion);
            }
        }

        // Method untuk mereview satu versi dokumen
        private void ReviewVersion(DocumentBody version)
        {
            if (version == null)
            {
                Console.WriteLine("Versi tidak ditemukan.");
                return;
            }

            Console.WriteLine($"\n=== Review Versi {version.Id} ===");
            Console.WriteLine($"Dibuat pada: {version.Created_at.ToString("dd/MM/yyyy HH:mm:ss")}");
            Console.WriteLine($"Status: {(version.IsCurrentVersion ? "Aktif" : "Tidak Aktif")}");
            Console.WriteLine($"Deskripsi: {version.VersionDescription}");
            Console.WriteLine("\nKonten:");
            Console.WriteLine(version.Content);

            // If already reviewed, display review information
            if (version.IsReviewed && version.ReviewId != Guid.Empty)
            {
                Console.WriteLine("\nDokumen ini sudah direview sebelumnya.");
                // We need to fetch the review details from the ReviewService (or ResearchRequestManager)
                // assuming ReviewService has a GetReviewById method.
                var reviewService = new ReviewService(); // Instantiate the ReviewService
                var existingReview = reviewService.GetReviewById(version.ReviewId);

                if (existingReview != null)
                {
                    Console.WriteLine($"Hasil review: {existingReview.Result}");
                    Console.WriteLine($"Komentar: {existingReview.Comment}");
                    Console.WriteLine($"Direview oleh: {existingReview.ReviewerName}");
                    Console.WriteLine($"Pada: {existingReview.ReviewDate:dd/MM/yyyy HH:mm:ss}"); // Setara dengan 'existingReview.ReviewDate.ToString("dd/MM/yyyy HH:mm:ss")'
                }
                else
                {
                    Console.WriteLine($"Hasil review: {version.ReviewResult}"); // Fallback to DocumentBody's stored result
                }

                Console.WriteLine("\nApakah Anda ingin membuat review baru? (y/n): ");
                string? choice = Console.ReadLine()?.ToLower();
                if (choice != "y")
                {
                    return;
                }
            }

            Console.WriteLine("\n=== Buat Review ===");
            Console.WriteLine("1. Setujui (Approve)");
            Console.WriteLine("2. Perlu Revisi (Needs Revision)");
            Console.WriteLine("3. Tolak (Reject)");
            Console.WriteLine("0. Kembali tanpa Review");

            Console.Write("Pilihan: ");
            string? reviewChoice = Console.ReadLine();

            if (reviewChoice == "0")
            {
                return;
            }

            // Determine result based on choice
            ReviewResult result;
            switch (reviewChoice)
            {
                case "1":
                    result = ReviewResult.Approved;
                    break;
                case "2":
                    result = ReviewResult.NeedsRevision;
                    break;
                case "3":
                    result = ReviewResult.Rejected;
                    break;
                default:
                    Console.WriteLine("Pilihan tidak valid.");
                    return;
            }

            Console.Write("Masukkan komentar untuk review: ");
            string comment = Console.ReadLine() ?? "";

            // Get the associated Document for the DocumentBody
            var document = DocumentService.GetById(version.DocumentId); // Assuming DocumentService.GetById is the correct method
            if (document == null)
            {
                Console.WriteLine("Dokumen utama tidak ditemukan untuk versi ini.");
                return;
            }

            var researchRequestController = new ResearchRequestController();
            Guid requestId;
            ResearchRequest? existingRequest = null;

            // To check for an existing research request, we need to call GetAllRequests and extract the data
            var allRequestsResult = researchRequestController.GetAllRequests();
            if (allRequestsResult is OkObjectResult okAllRequestsResult)
            {
                dynamic allRequestsData = okAllRequestsResult.Value;
                List<ResearchRequest>? allRequests = allRequestsData.data as List<ResearchRequest>;
                if (allRequests != null)
                {
                    // Find existing request for this DocumentBody.Id (version.Id)
                    existingRequest = allRequests.FirstOrDefault(r => r.DocumentBodyId == version.Id);
                }
            }

            if (existingRequest == null)
            {
                Console.WriteLine("Membuat permintaan review baru...");
                // Create a new ResearchRequestDto to send to the API
                var newRequestDto = new ResearchRequestDto
                {
                    Title = $"Review for {document.Title} - {version.VersionDescription}",
                    AbstractText = $"Review document version from {version.Created_at}",
                    ResearcherName = document.User?.Name ?? "Unknown", // Assuming document.User is available
                    UserId = document.User_id,
                    DocumentId = document.Id,
                    DocumentBodyId = version.Id
                };

                // Call the AddRequest method on the controller and handle its IActionResult
                var addRequestResult = researchRequestController.AddRequest(newRequestDto);
                if (addRequestResult is CreatedAtActionResult createdResult)
                {
                    dynamic createdData = createdResult.Value;
                    ResearchRequest createdRequest = createdData.data; // Assuming 'data' contains the created ResearchRequest
                    requestId = createdRequest.Id;
                    Console.WriteLine($"Permintaan review baru berhasil dibuat dengan ID: {requestId}");
                }
                else if (addRequestResult is BadRequestObjectResult badRequest)
                {
                    dynamic errorData = badRequest.Value;
                    Console.WriteLine($"Gagal membuat permintaan review: {errorData.message}");
                    return;
                }
                else
                {
                    Console.WriteLine("Gagal membuat permintaan review. Terjadi kesalahan tak terduga.");
                    return;
                }
            }
            else
            {
                requestId = existingRequest.Id;
                Console.WriteLine($"Menggunakan permintaan review yang sudah ada dengan ID: {requestId}");
            }

            // Start review if the request is in the Submitted state
            var currentRequestState = researchRequestController.GetRequestById(requestId);
            if (currentRequestState is OkObjectResult okCurrentRequest)
            {
                dynamic requestData = okCurrentRequest.Value;
                ResearchRequest req = requestData.data;

                if (req.State is SubmittedState)
                {
                    Console.WriteLine("Memulai proses review...");
                    var startReviewResult = researchRequestController.StartReview(requestId);
                    if (startReviewResult is OkObjectResult okStartReview)
                    {
                        dynamic startReviewData = okStartReview.Value;
                        Console.WriteLine(startReviewData.message);
                    }
                    else if (startReviewResult is BadRequestObjectResult badRequest)
                    {
                        dynamic errorData = badRequest.Value;
                        Console.WriteLine($"Gagal memulai review: {errorData.message}");
                    }
                }
                else
                {
                    Console.WriteLine($"Permintaan review saat ini dalam keadaan '{req.State.Name}'. Melewatkan langkah 'mulai review'.");
                }
            }


            // Process review
            Console.WriteLine("Memproses review...");
            var processReviewDto = new ProcessReviewDto
            {
                ReviewerId = _currentUser.Id, // Assuming _currentUser is available and has an Id
                Result = result,
                ReviewerComment = comment
            };
            var processReviewResult = researchRequestController.ProcessReview(requestId, processReviewDto);

            if (processReviewResult is OkObjectResult okProcessResult)
            {
                dynamic processData = okProcessResult.Value;
                Console.WriteLine(processData.message);
            }
            else if (processReviewResult is BadRequestObjectResult badRequestProcess)
            {
                dynamic errorData = badRequestProcess.Value;
                Console.WriteLine($"Gagal memproses review: {errorData.message}");
                return;
            }
            else if (processReviewResult is NotFoundObjectResult notFoundProcess)
            {
                dynamic errorData = notFoundProcess.Value;
                Console.WriteLine($"Gagal memproses review: {errorData.message}");
                return;
            }
            else
            {
                Console.WriteLine("Gagal memproses review. Terjadi kesalahan tak terduga.");
                return;
            }

            // Update ReviewId in DocumentBody
            // Assuming DocumentBodyService.MarkVersionAsReviewed needs the actual Review ID and Result
            // The Review ID should ideally come from the Review object created by the AddReview in the ReviewService
            // For now, let's assume it gets the requestId and the result from the ProcessReview.
            // However, the current API doesn't return the Review object upon process, so you might need to fetch it
            // or rely on ReviewService to manage this internal link.

            // Let's assume for simplicity, the ReviewService.MarkVersionAsReviewed can work with the existing requestId and result.
            var success = DocumentBodyService.MarkVersionAsReviewed(version.Id, requestId, result); // Assuming requestId is now the ReviewId

            if (success)
            {
                Console.WriteLine("\nReview berhasil disimpan!");
                Console.WriteLine($"Status review: {result}");
                if (result == ReviewResult.Approved)
                {
                    Console.WriteLine("Dokumen telah disetujui. Mahasiswa dapat melanjutkan versi berikutnya.");
                }
                else if (result == ReviewResult.NeedsRevision)
                {
                    Console.WriteLine("Dokumen perlu revisi. Mahasiswa akan diminta untuk membuat perubahan.");
                }
                else
                {
                    Console.WriteLine("Dokumen ditolak. Mahasiswa perlu membuat perubahan besar sebelum mengajukan ulang.");
                }
            }
            else
            {
                Console.WriteLine("\nGagal memperbarui status review pada dokumen.");
            }
        }
    }
}

