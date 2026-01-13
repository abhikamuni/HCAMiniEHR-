using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Models;
using HCAMiniEHR.Repositories;

namespace HCAMiniEHR.Pages.Appointments
{
    public class IndexModel : PageModel
    {
        private readonly IAppointmentRepository _repository;

        public IndexModel(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        public List<Appointment> Appointments { get; set; } = new List<Appointment>();

        public void OnGet()
        {
            Appointments = _repository.GetAllAppointments();
        }
    }
}
