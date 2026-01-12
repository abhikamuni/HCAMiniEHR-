using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HCAMiniEHR.Models;

[Table("LabOrder", Schema = "Healthcare")]
public partial class LabOrder
{
    [Key]
    public int OrderId { get; set; }

    public int AppointmentId { get; set; }

    [StringLength(100)]
    public string TestName { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? OrderDate { get; set; }

    [StringLength(20)]
    public string? Status { get; set; }

    public string? Result { get; set; }

    [ForeignKey("AppointmentId")]
    [InverseProperty("LabOrders")]
    public virtual Appointment Appointment { get; set; } = null!;
}
