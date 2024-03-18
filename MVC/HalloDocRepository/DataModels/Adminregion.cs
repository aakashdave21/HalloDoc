using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.DataModels;

[Table("adminregion")]
public partial class Adminregion
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("adminid")]
    public int Adminid { get; set; }

    [Column("regionid")]
    public int Regionid { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [ForeignKey("Adminid")]
    [InverseProperty("Adminregions")]
    public virtual Admin Admin { get; set; } = null!;

    [ForeignKey("Regionid")]
    [InverseProperty("Adminregions")]
    public virtual Region Region { get; set; } = null!;
}
