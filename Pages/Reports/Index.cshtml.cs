using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HCAMiniEHR.Models;

namespace HCAMiniEHR.Pages.Reports
{
    public class IndexModel : PageModel
    {
        private readonly HCAMiniContext _context;

        public IndexModel(HCAMiniContext context)
        {
            _context = context;
        }

        // --- Data Containers for our Reports ---
        public List<LabOrder> PendingLabs { get; set; } = new();
        public List<DoctorStat> DoctorStats { get; set; } = new();
        public List<Patient> PatientsNeedingAppointment { get; set; } = new();

        public void OnGet()
        {
            // REPORT 1: Pending Lab Orders (Using Where + Include)
            // Finds all orders that are not completed yet.
            PendingLabs = _context.LabOrders
                .Include(l => l.Appointment)
                .ThenInclude(a => a.Patient)
                .Where(l => l.Status == "Pending")
                .ToList();

            // REPORT 2: Doctor Productivity (Using GroupBy + Select)
            // counts how many appointments each doctor has handled.
            DoctorStats = _context.Appointments
                .GroupBy(a => a.DoctorName)
                .Select(g => new DoctorStat
                {
                    DoctorName = g.Key ?? "Unassigned",
                    AppointmentCount = g.Count()
                })
                .OrderByDescending(d => d.AppointmentCount)
                .ToList();

            // REPORT 3: Patients with No Future Appointments (Using !Any - "Not Exists")
            // Finds patients who do NOT have any appointment scheduled in the future.
            PatientsNeedingAppointment = _context.Patients
                .Where(p => !p.Appointments.Any(a => a.AppointmentDate >= DateTime.Today))
                .ToList();
        }

        // Simple helper class for the GroupBy report
        public class DoctorStat
        {
            public string DoctorName { get; set; }
            public int AppointmentCount { get; set; }
        }
    }
}


