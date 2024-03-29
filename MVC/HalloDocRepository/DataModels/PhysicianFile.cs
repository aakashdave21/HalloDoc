using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.DataModels;

[Table("physicianfile")]
[Index("Physicianid", Name = "physicianfile_physicianid_key", IsUnique = true)]
public partial class Physicianfile
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("physicianid")]
    public int Physicianid { get; set; }

    [Column("ica")]
    public string? Ica { get; set; }

    [Column("backgroundcheck")]
    public string? Backgroundcheck { get; set; }

    [Column("hipaa")]
    public string? Hipaa { get; set; }

    [Column("nda")]
    public string? Nda { get; set; }

    [Column("license")]
    public string? License { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [ForeignKey("Physicianid")]
    [InverseProperty("Physicianfile")]
    public virtual Physician Physician { get; set; } = null!;
}
