using HCAMiniEHR.Models;

namespace HCAMiniEHR.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly HCAMiniContext _context;

        // Constructor Injection: Use the DB Context we set up in Program.cs
        public PatientRepository(HCAMiniContext context)
        {
            _context = context;
        }

        public List<Patient> GetAllPatients()
        {
            return _context.Patients.ToList();
        }

        public Patient? GetPatientById(int id)
        {
            return _context.Patients.Find(id);
        }

        public void AddPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            _context.SaveChanges();
        }

        public void UpdatePatient(Patient patient)
        {
            _context.Patients.Update(patient);
            _context.SaveChanges();
        }

        public void DeletePatient(int id)
        {
            var patient = _context.Patients.Find(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                _context.SaveChanges();
            }
        }
    }
}
