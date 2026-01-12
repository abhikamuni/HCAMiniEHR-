using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Repositories;
using System.ComponentModel.DataAnnotations;

namespace HCAMiniEHR.Pages.Appointments
{
    public class CreateModel : PageModel
    {
        private readonly IAppointmentRepository _repo;

        public CreateModel(IAppointmentRepository repo)
        {
            _repo = repo;
        }

        [BindProperty(SupportsGet = true)]
        public int PatientId { get; set; }

        // SEPARATE INPUTS for cleaner UI
        [BindProperty, Required]
        public DateTime SelectedDate { get; set; } = DateTime.Today.AddDays(1);

        [BindProperty, Required]
        public DateTime SelectedTime { get; set; } = DateTime.Now;

        [BindProperty, Required]
        public string Reason { get; set; }

        [BindProperty, Required]
        public string DoctorName { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            // MERGE Date and Time back together
            // We take the Date from SelectedDate and the Time from SelectedTime
            DateTime finalDateTime = SelectedDate.Date + SelectedTime.TimeOfDay;

            // Call the Stored Procedure with the merged DateTime
            _repo.AddAppointmentSP(PatientId, finalDateTime, Reason, DoctorName);

            return RedirectToPage("/Patients/Index");
        }
    }
}

