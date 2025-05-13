using PaperNest_API.Models;
using PaperNest_API.Repository;

namespace PaperNest_API.Services
{
    public class WorkspaceService
    {
        public static void Create(Workspace workspace)
        {
            WorkspaceRepository.workspaceList.Add(workspace);   
        }

        public static IEnumerable<Workspace> GetAll()
        {
            return WorkspaceRepository.workspaceList;
        }

        public static Workspace? GetById(Guid id)
        {
            return WorkspaceRepository.workspaceList.FirstOrDefault(w => w.Id == id);
        }

        public static IEnumerable<Workspace> GetByUserId(Guid userId)
        {
            return WorkspaceRepository.workspaceList.Where(w => w.User_id == userId);
        }

        public static void Update(Guid id, Workspace workspace)
        {
            var existingWorkspace = GetById(id);

            if (existingWorkspace != null)
            {
                existingWorkspace.Name = workspace.Name;
                existingWorkspace.Description = workspace.Description;
                existingWorkspace.Updated_at = DateTime.Now;
            }
        }

        public static void Delete(Guid id)
        {
            var existingWorkspace = GetById(id);

            if (existingWorkspace != null)
            {
                WorkspaceRepository.workspaceList.Remove(existingWorkspace);
            }
        }
    }
}
