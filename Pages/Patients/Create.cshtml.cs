using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Models;
using HCAMiniEHR.Repositories;

namespace HCAMiniEHR.Pages.Patients
{
    public class CreateModel : PageModel
    {
        private readonly IPatientRepository _repository;

        public CreateModel(IPatientRepository repository)
        {
            _repository = repository;
        }

        // [BindProperty] makes the form data automatically fill this object
        [BindProperty]
        public Patient Patient { get; set; } = new Patient();

        public void OnGet()
        {
            // Nothing needed here, just show the blank form
        }

        public IActionResult OnPost()
        {
            // CUSTOM VALIDATION: Check for Future Date
            if (Patient.DateOfBirth > DateTime.Today)
            {
                // This adds a specific error to the "DateOfBirth" field
                ModelState.AddModelError("Patient.DateOfBirth", "Date of Birth cannot be in the future.");
            }

            // CUSTOM VALIDATION: Check for reasonable age (optional, e.g., not older than 120)
            if (Patient.DateOfBirth < DateTime.Today.AddYears(-120))
            {
                ModelState.AddModelError("Patient.DateOfBirth", "Please enter a valid Date of Birth.");
            }

            // Check if any validation failed (Regex, Required, or our Custom Date check)
            if (!ModelState.IsValid)
            {
                return Page(); // Stops saving and shows errors on the form
            }

            // If valid, save to database
            _repository.AddPatient(Patient);

            return RedirectToPage("./Index");
        }

    }
}

