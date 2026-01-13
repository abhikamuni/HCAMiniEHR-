
using HCAMiniEHR.Models;

namespace HCAMiniEHR.Repositories
{
    public interface IPatientRepository
    {
        // Define what we can do with Patients
        List<Patient> GetAllPatients();
        Patient? GetPatientById(int id);
        void AddPatient(Patient patient);
        void UpdatePatient(Patient patient);
        void DeletePatient(int id);
    }
}
