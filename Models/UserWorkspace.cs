using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PaperNest_API.Models
{
    public class UserWorkspace : BaseEntity
    {
        [Required]
        public Guid UserId { get; set; } 

        [Required]
        public Guid WorkspaceId { get; set; } 

        [Required]
        public RoleType Role { get; set; } = RoleType.Member;

        public List<Permissions> Permissions { get; set; } = new List<Permissions>();
        public string Description { get; set; } = "Project Description";
        
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
        
        [ForeignKey("WorkspaceId")]
        public virtual Workspace Workspace { get; set; } = null!;
    }
}
