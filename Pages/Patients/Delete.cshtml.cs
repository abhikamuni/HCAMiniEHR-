using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Models;
using HCAMiniEHR.Repositories;
using Microsoft.EntityFrameworkCore; // Needed for DbUpdateException

namespace HCAMiniEHR.Pages.Patients
{
    public class DeleteModel : PageModel
    {
        private readonly IPatientRepository _repository;

        public DeleteModel(IPatientRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public Patient Patient { get; set; }

        public IActionResult OnGet(int id)
        {
            // 1. Find the patient so we can show their name on the confirmation screen
            Patient = _repository.GetPatientById(id);

            if (Patient == null)
            {
                return RedirectToPage("./Index");
            }
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            try
            {
                // 2. Attempt the delete
                _repository.DeletePatient(id);
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException) // <--- This catches the SQL "Foreign Key" error
            {
                // 3. If it fails, define the error message
                TempData["ErrorMessage"] = "ACTION BLOCKED: You cannot delete this patient because they have existing medical appointments or lab orders. Please delete those records first.";

                // 4. Send them back to the list
                return RedirectToPage("./Index");
            }
        }
    }
}
