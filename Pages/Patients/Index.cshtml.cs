using Microsoft.AspNetCore.Mvc.RazorPages;
using HCAMiniEHR.Models;
using HCAMiniEHR.Repositories;

namespace HCAMiniEHR.Pages.Patients
{
    public class IndexModel : PageModel
    {
        private readonly IPatientRepository _repository;

        // Constructor Injection: Ask for the Repository
        public IndexModel(IPatientRepository repository)
        {
            _repository = repository;
        }

        // Variable to hold the list of patients so the HTML can see it
        public List<Patient> PatientList { get; set; } = new List<Patient>();

        public void OnGet()
        {
            // Use the repository to get data
            PatientList = _repository.GetAllPatients();
        }
    }
}
