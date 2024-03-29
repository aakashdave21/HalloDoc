using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.DataModels;

[Table("shift")]
public partial class Shift
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("physicianid")]
    public int Physicianid { get; set; }

    [Column("startdate")]
    public DateOnly Startdate { get; set; }

    [Column("isrepeat")]
    public bool Isrepeat { get; set; }

    [Column("weekdays")]
    [StringLength(7)]
    public string? Weekdays { get; set; }

    [Column("repeatupto")]
    public int? Repeatupto { get; set; }

    [Column("createdby")]
    public int Createdby { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [Column("ip")]
    [StringLength(20)]
    public string? Ip { get; set; }

    [ForeignKey("Createdby")]
    [InverseProperty("Shifts")]
    public virtual Aspnetuser CreatedbyNavigation { get; set; } = null!;

    [ForeignKey("Physicianid")]
    [InverseProperty("Shifts")]
    public virtual Physician Physician { get; set; } = null!;

    [InverseProperty("Shift")]
    public virtual Shiftdetail? Shiftdetail { get; set; }
}
