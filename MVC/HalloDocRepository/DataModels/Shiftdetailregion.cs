using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.DataModels;

[Table("shiftdetailregion")]
public partial class Shiftdetailregion
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("shiftdetailid")]
    public int Shiftdetailid { get; set; }

    [Column("regionid")]
    public int Regionid { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [Column("isdeleted")]
    public bool Isdeleted { get; set; }

    [ForeignKey("Regionid")]
    [InverseProperty("Shiftdetailregions")]
    public virtual Region Region { get; set; } = null!;

    [ForeignKey("Shiftdetailid")]
    [InverseProperty("Shiftdetailregions")]
    public virtual Shiftdetail Shiftdetail { get; set; } = null!;
}
