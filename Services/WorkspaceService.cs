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
        
        public static UserWorkspace? JoinWorkspace(Guid workspaceId, Guid userId, RoleType role = RoleType.Lecturer)
        {
            // Cek apakah workspace ada
            var workspace = GetById(workspaceId);
            if (workspace == null)
            {
                return null;
            }
            
            // Cek apakah user sudah ada di workspace
            var existingUserWorkspace = Repository.UserRepository.userWorkspaceRepository
                .FirstOrDefault(uw => uw.WorkspaceId == workspaceId && uw.UserId == userId);
                
            if (existingUserWorkspace != null)
            {
                return existingUserWorkspace; // User sudah ada di workspace
            }
            
            // Tambahkan user ke workspace dengan role yang ditentukan
            var userWorkspace = new UserWorkspace
            {
                UserId = userId,
                WorkspaceId = workspaceId,
                Role = role,
                Description = "Bergabung sebagai dosen pembimbing",
                Updated_at = DateTime.Now
            };
            
            // Tambahkan izin default untuk dosen (hanya view dan comment)
            userWorkspace.Permissions.Add(Permissions.View);
            userWorkspace.Permissions.Add(Permissions.Comment);
            
            // Simpan ke repository
            Repository.UserRepository.userWorkspaceRepository.Add(userWorkspace);
            
            return userWorkspace;
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
        
        public static IEnumerable<Workspace> GetJoinedWorkspaces(Guid userId)
        {
            // Dapatkan semua UserWorkspace untuk user
            var userWorkspaces = Repository.UserRepository.userWorkspaceRepository
                .Where(uw => uw.UserId == userId)
                .Select(uw => uw.WorkspaceId);
                
            // Dapatkan semua workspace yang terkait
            return WorkspaceRepository.workspaceList.Where(w => userWorkspaces.Contains(w.Id));
        }
    }
}
