using System;
using System.ComponentModel.DataAnnotations;

namespace Ghumfir.Domain.Entities
{
    public class ApplicationUser
    {
        public Guid Id { get; set; }

        public string Mobile { get; set; }

        public string FullName { get; set; }

        public string Password { get; set; }

        public string? Email { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsEmailVerified { get; set; }

        public string Role { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public bool ForceChangePassword { get; set; }
    }
}
