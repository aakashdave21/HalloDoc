using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.DataModels;

[Table("shiftdetail")]
public partial class Shiftdetail
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("shiftid")]
    public int Shiftid { get; set; }

    [Column("shiftdate", TypeName = "timestamp without time zone")]
    public DateTime Shiftdate { get; set; }

    [Column("regionid")]
    public int? Regionid { get; set; }

    [Column("starttime")]
    public TimeOnly Starttime { get; set; }

    [Column("endtime")]
    public TimeOnly Endtime { get; set; }

    [Column("status")]
    public short Status { get; set; }

    [Column("lastrunningdate", TypeName = "timestamp without time zone")]
    public DateTime? Lastrunningdate { get; set; }

    [Column("eventid")]
    [StringLength(100)]
    public string? Eventid { get; set; }

    [Column("issync")]
    public bool? Issync { get; set; }

    [Column("createdby")]
    public int Createdby { get; set; }

    [Column("modifiedby")]
    public int? Modifiedby { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [Column("isdeleted")]
    public bool Isdeleted { get; set; }

    [ForeignKey("Createdby")]
    [InverseProperty("ShiftdetailCreatedbyNavigations")]
    public virtual Aspnetuser CreatedbyNavigation { get; set; } = null!;

    [ForeignKey("Modifiedby")]
    [InverseProperty("ShiftdetailModifiedbyNavigations")]
    public virtual Aspnetuser? ModifiedbyNavigation { get; set; }

    [ForeignKey("Regionid")]
    [InverseProperty("Shiftdetails")]
    public virtual Region? Region { get; set; }

    [ForeignKey("Shiftid")]
    [InverseProperty("Shiftdetails")]
    public virtual Shift Shift { get; set; } = null!;

    [InverseProperty("Shiftdetail")]
    public virtual ICollection<Shiftdetailregion> Shiftdetailregions { get; } = new List<Shiftdetailregion>();
}
