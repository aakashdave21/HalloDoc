using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.DataModels;

[Table("physicianregion")]
public partial class Physicianregion
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("physicianid")]
    public int? Physicianid { get; set; }

    [Column("regionid")]
    public int? Regionid { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [ForeignKey("Physicianid")]
    [InverseProperty("Physicianregions")]
    public virtual Physician? Physician { get; set; }

    [ForeignKey("Regionid")]
    [InverseProperty("Physicianregions")]
    public virtual Region? Region { get; set; }
}
