using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.DataModels;

[Table("payrate")]
public partial class Payrate
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("physicianid")]
    public int Physicianid { get; set; }

    [Column("nightshiftweekend")]
    public decimal? Nightshiftweekend { get; set; }

    [Column("shift")]
    public decimal? Shift { get; set; }

    [Column("housecallnightweekend")]
    public decimal? Housecallnightweekend { get; set; }

    [Column("phoneconsult")]
    public decimal? Phoneconsult { get; set; }

    [Column("phoneconsultnightweekend")]
    public decimal? Phoneconsultnightweekend { get; set; }

    [Column("batchtesting")]
    public decimal? Batchtesting { get; set; }

    [Column("housecall")]
    public decimal? Housecall { get; set; }

    [ForeignKey("Physicianid")]
    [InverseProperty("Payrates")]
    public virtual Physician Physician { get; set; } = null!;
}
