using PaperNest_API.Models;
using PaperNest_API.Repository;

namespace PaperNest_API.Services
{
    public class DocumentBodyService
    {
        private static List<DocumentBody> _documentBodies = new List<DocumentBody>();
        
        public static DocumentBody CreateVersion(Guid documentId, string content, string description)
        {
            // Nonaktifkan semua versi saat ini
            var currentVersions = _documentBodies
                .Where(db => db.DocumentId == documentId && db.IsCurrentVersion)
                .ToList();
                
            foreach(var version in currentVersions)
            {
                version.IsCurrentVersion = false;
            }
            
            // Buat versi baru
            var newVersion = new DocumentBody
            {
                DocumentId = documentId,
                Content = content,
                IsCurrentVersion = true,
                VersionDescription = description,
            };
            
            _documentBodies.Add(newVersion);
            return newVersion;
        }
        
        public static IEnumerable<DocumentBody> GetVersions(Guid documentId)
        {
            return _documentBodies
                .Where(db => db.DocumentId == documentId)
                .OrderByDescending(db => db.Created_at)
                .ToList();
        }
        
        public static DocumentBody? GetCurrentVersion(Guid documentId)
        {
            return _documentBodies
                .FirstOrDefault(db => db.DocumentId == documentId && db.IsCurrentVersion);
        }
        
        public static DocumentBody? GetVersionById(Guid versionId)
        {
            return _documentBodies.FirstOrDefault(db => db.Id == versionId);
        }
        
        public static DocumentBody RollbackToVersion(Guid documentId, Guid versionId)
        {
            var version = GetVersionById(versionId);
            if (version == null)
            {
                throw new InvalidOperationException("Version not found");
            }
            
            // Buat versi baru berdasarkan konten versi sebelumnya
            return CreateVersion(documentId, version.Content, $"Rollback to version from {version.Created_at}");
        }
    }
}