using System;
using System.ComponentModel.DataAnnotations;

namespace Ghumfir.Domain.Entities
{
    public class ApplicationUser
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(15)]
        public string Mobile { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [StringLength(150)]
        public string? Email { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public bool IsEmailVerified { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
    }
}
