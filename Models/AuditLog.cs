using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HCAMiniEHR.Models;

[Table("AuditLog", Schema = "Healthcare")]
public partial class AuditLog
{
    [Key]
    public int LogId { get; set; }

    [StringLength(10)]
    public string? ActionType { get; set; }

    [StringLength(50)]
    public string? TableName { get; set; }

    public int? RecordId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LogDate { get; set; }

    public string? Details { get; set; }
}
