using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.DataModels;

[Table("timesheetreimbursement")]
public partial class Timesheetreimbursement
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("timesheetid")]
    public int Timesheetid { get; set; }

    [Column("item")]
    public string Item { get; set; } = null!;

    [Column("amount")]
    public int Amount { get; set; }

    [Column("bill")]
    public string? Bill { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? ShiftDate { get; set; }

    [ForeignKey("Timesheetid")]
    [InverseProperty("Timesheetreimbursements")]
    public virtual Timesheet Timesheet { get; set; } = null!;
}
