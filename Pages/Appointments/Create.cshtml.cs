using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Repositories;
using HCAMiniEHR.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; // Needed for SelectList

namespace HCAMiniEHR.Pages.Appointments
{
    public class CreateModel : PageModel
    {
        private readonly IAppointmentRepository _repo;
        private readonly IDoctorRepository _doctorRepo; // NEW

        public CreateModel(IAppointmentRepository repo, IDoctorRepository doctorRepo)
        {
            _repo = repo;
            _doctorRepo = doctorRepo; // NEW
        }

        [BindProperty(SupportsGet = true)]
        public int PatientId { get; set; }

        [BindProperty, Required]
        public DateTime SelectedDate { get; set; } = DateTime.Today.AddDays(1);

        [BindProperty, Required]
        public DateTime SelectedTime { get; set; } = DateTime.Now;

        [BindProperty, Required]
        public string Reason { get; set; }

        [BindProperty, Required]
        public string DoctorName { get; set; }

        // NEW: This holds the list for the dropdown
        public SelectList DoctorOptions { get; set; }

        public void OnGet()
        {
            // Fetch doctors and prepare them for the dropdown
            // Value = FullName (so it works with your old code), Text = FullName + Specialization
            var doctors = _doctorRepo.GetAllDoctors();
            DoctorOptions = new SelectList(doctors, "FullName", "FullName");
        }

        public IActionResult OnPost()
        {
            // RE-LOAD THE LIST if validation fails (otherwise dropdown becomes empty)
            if (!ModelState.IsValid)
            {
                var doctors = _doctorRepo.GetAllDoctors();
                DoctorOptions = new SelectList(doctors, "FullName", "FullName");
                return Page();
            }

            DateTime finalDateTime = SelectedDate.Date + SelectedTime.TimeOfDay;

            // 1. Future Date Check
            if (finalDateTime <= DateTime.Now)
            {
                ModelState.AddModelError("SelectedDate", "Appointments must be in the future.");
                var doctors = _doctorRepo.GetAllDoctors();
                DoctorOptions = new SelectList(doctors, "FullName", "FullName");
                return Page();
            }

            // 2. Doctor Conflict Check (Old Logic - Still Works because we pass the Name!)
            if (!_repo.IsSlotAvailable(DoctorName, finalDateTime))
            {
                ModelState.AddModelError("SelectedTime", $"Dr. {DoctorName} is booked at this time.");
                var doctors = _doctorRepo.GetAllDoctors();
                DoctorOptions = new SelectList(doctors, "FullName", "FullName");
                return Page();
            }

            _repo.AddAppointmentSP(PatientId, finalDateTime, Reason, DoctorName);
            return RedirectToPage("/Patients/Index");
        }
    }
}

