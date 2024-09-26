using System.ComponentModel.DataAnnotations;

namespace KBMGrpcService.Repositories.Context.Models
{
    public class Organization
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Address { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        // Navigation property
        public ICollection<User> Users { get; set; }
    }
}
