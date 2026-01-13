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
            // 1. Combine Date and Time
            DateTime finalDateTime = SelectedDate.Date + SelectedTime.TimeOfDay;

            // RULE: Booking must be in the future
            if (finalDateTime <= DateTime.Now)
            {
                ModelState.AddModelError("SelectedDate", "Appointments must be scheduled for a future date and time.");
                return Page();
            }

            // RULE: Check Doctor Availability
            if (!_repo.IsSlotAvailable(DoctorName, finalDateTime))
            {
                ModelState.AddModelError("SelectedTime", $"Dr. {DoctorName} is already booked at {SelectedTime:HH:mm}. Please choose a different time.");
                return Page();
            }

            if (!ModelState.IsValid) return Page();

            _repo.AddAppointmentSP(PatientId, finalDateTime, Reason, DoctorName);
            return RedirectToPage("/Patients/Index");
        }

    }
}

