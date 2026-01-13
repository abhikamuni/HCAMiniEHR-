namespace HCAMiniEHR.DTOs
{
    // DTO for the Doctor Productivity Report
    public class DoctorStatsDto
    {
        public string DoctorName { get; set; }
        public int AppointmentCount { get; set; }
    }

    // DTO for the Pending Labs Report (Bonus: only fetching what we need)
    public class PendingLabDto
    {
        public string TestName { get; set; }
        public string PatientName { get; set; }
        public string DateOrdered { get; set; }
        public string Status { get; set; }
    }
}
