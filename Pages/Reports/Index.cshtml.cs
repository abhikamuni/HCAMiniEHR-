using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HCAMiniEHR.Models;
using HCAMiniEHR.DTOs; // Add this namespace

namespace HCAMiniEHR.Pages.Reports
{
    public class IndexModel : PageModel
    {
        private readonly HCAMiniContext _context;

        public IndexModel(HCAMiniContext context)
        {
            _context = context;
        }

        // UPDATED: Using DTOs instead of Entity Models
        public List<PendingLabDto> PendingLabs { get; set; } = new();
        public List<DoctorStatsDto> DoctorStats { get; set; } = new();

        // (We can keep this one as Entity for simplicity, or make a DTO if you really want)
        public List<Patient> PatientsNeedingAppointment { get; set; } = new();

        public void OnGet()
        {
            // REPORT 1: Pending Labs mapped to DTO
            PendingLabs = _context.LabOrders
                .Include(l => l.Appointment)
                .ThenInclude(a => a.Patient)
                .Where(l => l.Status == "Pending")
                .Select(l => new PendingLabDto // Projection
                {
                    TestName = l.TestName,
                    PatientName = l.Appointment.Patient.LastName + ", " + l.Appointment.Patient.FirstName,
                    DateOrdered = l.OrderDate.HasValue ? l.OrderDate.Value.ToString("MM/dd/yyyy") : "N/A",
                    Status = l.Status
                })
                .ToList();

            // REPORT 2: Doctor Stats mapped to DTO
            DoctorStats = _context.Appointments
                .GroupBy(a => a.DoctorName)
                .Select(g => new DoctorStatsDto
                {
                    DoctorName = g.Key ?? "Unassigned",
                    AppointmentCount = g.Count()
                })
                .OrderByDescending(d => d.AppointmentCount)
                .ToList();

            // REPORT 3 (Kept as is)
            PatientsNeedingAppointment = _context.Patients
                .Where(p => !p.Appointments.Any(a => a.AppointmentDate >= DateTime.Today))
                .ToList();
        }
    }
}
