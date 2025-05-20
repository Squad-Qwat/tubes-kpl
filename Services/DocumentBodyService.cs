using PaperNest_API.Models;
using PaperNest_API.Repository;

namespace PaperNest_API.Services
{
    public class DocumentBodyService
    {
        private static List<DocumentBody> _documentVersions = new List<DocumentBody>();
        
        public static DocumentBody CreateVersion(Guid documentId, string content, string description)
        {
            // Cek apakah ada versi yang sedang menjadi versi utama
            var currentVersion = _documentVersions.FirstOrDefault(v => v.DocumentId == documentId && v.IsCurrentVersion);
            
            // Jika ada, buat tidak menjadi versi utama lagi
            if (currentVersion != null)
            {
                currentVersion.IsCurrentVersion = false;
            }
            
            // Buat versi baru
            var newVersion = new DocumentBody
            {
                DocumentId = documentId,
                Content = content,
                VersionDescription = description,
                IsCurrentVersion = true,
                Updated_at = DateTime.Now
            };
            
            _documentVersions.Add(newVersion);
            
            return newVersion;
        }
        
        public static IEnumerable<DocumentBody> GetVersions(Guid documentId)
        {
            return _documentVersions.Where(v => v.DocumentId == documentId)
                                   .OrderByDescending(v => v.Created_at);
        }
        
        public static DocumentBody? GetCurrentVersion(Guid documentId)
        {
            return _documentVersions.FirstOrDefault(v => v.DocumentId == documentId && v.IsCurrentVersion);
        }
        
        public static DocumentBody? GetVersionById(Guid versionId)
        {
            return _documentVersions.FirstOrDefault(v => v.Id == versionId);
        }
        
        public static DocumentBody RollbackToVersion(Guid documentId, Guid versionId)
        {
            // Cek apakah versi yang dimaksud ada
            var version = _documentVersions.FirstOrDefault(v => v.Id == versionId);
            
            if (version == null)
            {
                throw new Exception("Versi tidak ditemukan");
            }
            
            // Buat versi baru berdasarkan versi yang dipilih
            return CreateVersion(documentId, version.Content, $"Rollback ke versi {version.Created_at.ToString("dd/MM/yyyy HH:mm:ss")}");
        }
        
        public static Document? GetDocumentById(Guid documentId)
        {
            // Cari dokumen berdasarkan ID-nya
            return Repository.DocumentRepository.documents.FirstOrDefault(d => d.Id == documentId);
        }
        
        public static bool MarkVersionAsReviewed(Guid versionId, Guid reviewId, ReviewResult result)
        {
            var version = _documentVersions.FirstOrDefault(v => v.Id == versionId);
            
            if (version == null)
            {
                return false;
            }
            
            version.ReviewId = reviewId;
            version.IsReviewed = true;
            version.ReviewResult = result;
            version.Updated_at = DateTime.Now;
            
            return true;
        }
        
        public static bool HasPendingReview(Guid documentId)
        {
            return _documentVersions.Any(v => v.DocumentId == documentId && !v.IsReviewed);
        }
        
        public static bool CanCreateNewVersion(Guid documentId)
        {
            // Cek apakah ada versi yang belum direview
            var lastVersion = _documentVersions.Where(v => v.DocumentId == documentId)
                                .OrderByDescending(v => v.Created_at)
                                .FirstOrDefault();
                                
            // Jika tidak ada versi sebelumnya, atau versi terakhir sudah direview, bisa buat versi baru
            return lastVersion == null || lastVersion.IsReviewed;
        }
    }
}