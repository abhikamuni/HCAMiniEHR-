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
            if (!ModelState.IsValid)
            {
                return Page(); // If validation fails, stay on page
            }

            // Save to database via Repository
            _repository.AddPatient(Patient);

            // Redirect back to the list
            return RedirectToPage("./Index");
        }
    }
}

