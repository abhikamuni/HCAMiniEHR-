using HCAMiniEHR.Models;

namespace HCAMiniEHR.Repositories
{
    public interface IAppointmentRepository
    {
        // Get all appointments (including Patient info)
        List<Appointment> GetAppointmentsByPatientId(int patientId);

        // This will call our Stored Procedure
        void AddAppointmentSP(int patientId, DateTime date, string reason, string doctor);
    }
}
