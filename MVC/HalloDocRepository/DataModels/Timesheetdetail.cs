using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.DataModels;

[Table("timesheetdetails")]
public partial class Timesheetdetail
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("timesheetid")]
    public int Timesheetid { get; set; }

    [Column("shiftdate", TypeName = "timestamp without time zone")]
    public DateTime? Shiftdate { get; set; }

    [Column("shifthours")]
    public int? Shifthours { get; set; }

    [Column("housecall")]
    public int? Housecall { get; set; }

    [Column("phoneconsult")]
    public int? Phoneconsult { get; set; }

    [Column("isweekend")]
    public bool? Isweekend { get; set; }

    [ForeignKey("Timesheetid")]
    [InverseProperty("Timesheetdetails")]
    public virtual Timesheet Timesheet { get; set; } = null!;
}
