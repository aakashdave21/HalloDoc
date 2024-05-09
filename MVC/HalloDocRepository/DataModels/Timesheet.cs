using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.DataModels;

[Table("timesheet")]
public partial class Timesheet
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("physicianid")]
    public int Physicianid { get; set; }

    [Column("startdate", TypeName = "timestamp without time zone")]
    public DateTime? Startdate { get; set; }

    [Column("enddate", TypeName = "timestamp without time zone")]
    public DateTime? Enddate { get; set; }

    [Column("status")]
    public string? Status { get; set; }

    [Column("isfinalized")]
    public bool? Isfinalized { get; set; }

    [Column("bonus")]
    public decimal? Bonus { get; set; }

    [Column("description")]
    [StringLength(264)]
    public string? Description { get; set; }

    [ForeignKey("Physicianid")]
    [InverseProperty("Timesheets")]
    public virtual Physician Physician { get; set; } = null!;

    [InverseProperty("Timesheet")]
    public virtual ICollection<Timesheetdetail> Timesheetdetails { get; } = new List<Timesheetdetail>();

    [InverseProperty("Timesheet")]
    public virtual ICollection<Timesheetreimbursement> Timesheetreimbursements { get; } = new List<Timesheetreimbursement>();
}
