using Microsoft.EntityFrameworkCore;
using HCAMiniEHR.Models;

namespace HCAMiniEHR.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly HCAMiniContext _context;

        public AppointmentRepository(HCAMiniContext context)
        {
            _context = context;
        }

        public List<Appointment> GetAppointmentsByPatientId(int patientId)
        {
            // We use .Include() to join with the Patient table so we can show patient names
            return _context.Appointments
                           .Include(a => a.Patient)
                           .Where(a => a.PatientId == patientId)
                           .ToList();
        }

        public void AddAppointmentSP(int patientId, DateTime date, string reason, string doctor)
        {
            // REQUIREMENT #6: Invoke Primary SQL Stored Procedure from C#
            // Syntax: EXEC [Healthcare].[sp_CreateAppointment] @p0, @p1, @p2, @p3

            _context.Database.ExecuteSqlRaw(
                "EXEC [Healthcare].[sp_CreateAppointment] {0}, {1}, {2}, {3}",
                patientId, date, reason, doctor
            );
        }

        public bool IsSlotAvailable(string doctorName, DateTime date)
        {
            // Check if THIS doctor has ANY appointment at THIS exact time
            return !_context.Appointments.Any(a =>
                a.DoctorName == doctorName &&
                a.AppointmentDate == date);
        }
        public List<Appointment> GetAllAppointments()
        {
            // .Include(a => a.Patient) joins the tables so we can see the Patient's Name
            return _context.Appointments
                           .Include(a => a.Patient)
                           .OrderByDescending(a => a.AppointmentDate) // Newest first
                           .ToList();
        }


    }
}
