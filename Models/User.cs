using System.ComponentModel.DataAnnotations;

namespace PaperNest_API.Models
{
    public class User : BaseEntity
    {

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        public string Role { get; set; } = "Mahasiswa";

        
        public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
        public virtual ICollection<Workspace> Workspaces { get; set; } = new List<Workspace>();
        public virtual ICollection<UserWorkspace> UserWorkspaces { get; set; } = new List<UserWorkspace>();
        public virtual ICollection<ResearchRequest> ResearchRequests { get; set; } = new List<ResearchRequest>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
