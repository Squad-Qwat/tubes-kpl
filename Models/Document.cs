using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaperNest_API.Models
{
    public class Document
    {
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Required, MaxLength(200)]
        public string Title { get; set; }

        public string? Description { get; set; }
        public string? Content { get; set; }

        [Required]
        public Guid User_id { get; set; }


        [Required]
        public Guid Workspace_id { get; set; }

        public bool Is_public { get; set; }
        public DateTime Created_at { get; private set; } = DateTime.Now;
        public DateTime Updated_at { get; set; }
    }
}
