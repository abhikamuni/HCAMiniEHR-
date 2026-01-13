using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Repositories;
using HCAMiniEHR.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq; // Needed for .Any() check

namespace HCAMiniEHR.Pages.LabOrders
{
    public class CreateModel : PageModel
    {
        private readonly ILabOrderRepository _repo;
        private readonly HCAMiniContext _context; // Inject Context for validation

        public CreateModel(ILabOrderRepository repo, HCAMiniContext context)
        {
            _repo = repo;
            _context = context;
        }

        // Changed: Removed "SupportsGet=true" because we will enter it manually now
        [BindProperty, Required(ErrorMessage = "Appointment ID is required")]
        public int AppointmentId { get; set; }

        [BindProperty, Required]
        public string TestName { get; set; }

        public void OnGet()
        {
            // Load blank page
        }

        public IActionResult OnPost()
        {
            // 1. Check if Appointment Exists in Database
            bool appointmentExists = _context.Appointments.Any(a => a.AppointmentId == AppointmentId);

            if (!appointmentExists)
            {
                // Error key must match the property name ("AppointmentId") to show next to the box
                ModelState.AddModelError("AppointmentId", $"Error: Appointment ID {AppointmentId} does not exist in the system.");
            }

            if (!ModelState.IsValid) return Page();

            // 2. Create the Order
            var newOrder = new LabOrder
            {
                AppointmentId = AppointmentId,
                TestName = TestName,
                OrderDate = DateTime.Now,
                Status = "Pending",
                Result = null
            };

            _repo.AddLabOrder(newOrder);

            return RedirectToPage("/Reports/Index");
        }
    }
}

