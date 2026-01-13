using HCAMiniEHR.Models;

namespace HCAMiniEHR.Repositories
{
    public interface IDoctorRepository
    {
        List<Doctor> GetAllDoctors();
    }
}

