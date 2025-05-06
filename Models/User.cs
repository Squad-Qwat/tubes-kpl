using System.ComponentModel.DataAnnotations;

namespace PaperNest_API.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; }

        public string Role { get; set; }

        public DateTime Created_at { get; private set; } = DateTime.Now;
        public DateTime Updated_at { get; set; }
    }
}
