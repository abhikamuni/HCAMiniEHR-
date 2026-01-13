using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HCAMiniEHR.Models;

[Table("Appointment", Schema = "Healthcare")]
public partial class Appointment
{
    [Key]
    public int AppointmentId { get; set; }

    public int PatientId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime AppointmentDate { get; set; }

    [StringLength(200)]
    public string? Reason { get; set; }

    [StringLength(20)]
    public string? Status { get; set; }

    [StringLength(50)]
    public string? DoctorName { get; set; }

    [InverseProperty("Appointment")]
    public virtual ICollection<LabOrder> LabOrders { get; set; } = new List<LabOrder>();

    [ForeignKey("PatientId")]
    [InverseProperty("Appointments")]
    public virtual Patient Patient { get; set; } = null!;
}
