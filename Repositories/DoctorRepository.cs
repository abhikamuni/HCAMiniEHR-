using HCAMiniEHR.Models;

namespace HCAMiniEHR.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HCAMiniContext _context;

        public DoctorRepository(HCAMiniContext context)
        {
            _context = context;
        }

        public List<Doctor> GetAllDoctors()
        {
            return _context.Doctors.ToList();
        }
    }
}
