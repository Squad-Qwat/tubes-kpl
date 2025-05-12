using PaperNest_API.Models;
using PaperNest_API.Repository;

namespace PaperNest_API.Services
{
    public class DocumentService
    {
        public static void Create(Document document)
        {
            DocumentRepository.documentList.Add(document);
        }

        public static IEnumerable<Document> GetAll()
        {
            return DocumentRepository.documentList;
        }

        public static Document? GetById(Guid id)
        {
            return DocumentRepository.documentList.FirstOrDefault(d => d.Id == id);
        }

        public static IEnumerable<Document> GetByUserId(Guid userId)
        {
            return DocumentRepository.documentList.Where(d => d.User_id == userId);
        }

        public static IEnumerable<Document> GetByWorkspaceId(Guid workspaceId)
        {
            return DocumentRepository.documentList.Where(d => d.Workspace_id == workspaceId);
        }

        public static void Update(Guid id, Document document)
        {
            var existingDocument = GetById(id);

            if (existingDocument != null)
            {
                existingDocument.Title = document.Title;
                existingDocument.Description = document.Description;
                existingDocument.Content = document.Content;
                existingDocument.Updated_at = DateTime.Now;
            }
        }

        public static void Delete(Guid id)
        {
            var existingDocument = GetById(id);

            if (existingDocument != null)
            {
                DocumentRepository.documentList.Remove(existingDocument);
            }
        }
    }
}
