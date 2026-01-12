using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Repositories;
using HCAMiniEHR.Models;
using System.ComponentModel.DataAnnotations;

namespace HCAMiniEHR.Pages.LabOrders
{
    public class CreateModel : PageModel
    {
        private readonly ILabOrderRepository _repo;

        public CreateModel(ILabOrderRepository repo)
        {
            _repo = repo;
        }

        [BindProperty(SupportsGet = true)]
        public int AppointmentId { get; set; }

        [BindProperty, Required]
        public string TestName { get; set; }

        public void OnGet()
        {
            // Load page
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var newOrder = new LabOrder
            {
                AppointmentId = AppointmentId,
                TestName = TestName,
                OrderDate = DateTime.Now,
                Status = "Pending", // Default status
                Result = null       // No result yet
            };

            _repo.AddLabOrder(newOrder);

            // Go to the Reports page so we can see it in the "Pending Labs" tab immediately
            return RedirectToPage("/Reports/Index");
        }
    }
}

