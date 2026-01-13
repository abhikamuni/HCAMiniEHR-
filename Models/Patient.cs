using System.ComponentModel.DataAnnotations;

namespace HCAMiniEHR.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50)]
        // Regex: Only letters (a-z), spaces, and hyphens allowed. No numbers.
        [RegularExpression(@"^[a-zA-Z\s-]+$", ErrorMessage = "First Name cannot contain numbers or special characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50)]
        // Regex: Same rule for Last Name
        [RegularExpression(@"^[a-zA-Z\s-]+$", ErrorMessage = "Last Name cannot contain numbers or special characters.")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        // We will add the "No Future Date" check in the PageModel (Step 2)
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address (e.g., user@example.com)")]
        public string? Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        // Navigation Property (needed for relationships)
        public ICollection<Appointment>? Appointments { get; set; }
    }
}

