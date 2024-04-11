using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.DataModels;

[Table("blockrequests")]
public partial class Blockrequest
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("requestid")]
    public int? Requestid { get; set; }

    [Column("phonenumber")]
    [StringLength(50)]
    public string? Phonenumber { get; set; }

    [Column("email")]
    [StringLength(50)]
    public string? Email { get; set; }

    [Column("isactive")]
    public bool? Isactive { get; set; }

    [Column("reason")]
    [StringLength(500)]
    public string? Reason { get; set; }

    [Column("ip")]
    [StringLength(20)]
    public string? Ip { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [ForeignKey("Requestid")]
    [InverseProperty("Blockrequests")]
    public virtual Request? Request { get; set; }
}
