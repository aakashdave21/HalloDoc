using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.DataModels;

[Table("emaillog")]
public partial class Emaillog
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("emailtemplate", TypeName = "character varying")]
    public string? Emailtemplate { get; set; }

    [Column("subjectname")]
    [StringLength(200)]
    public string? Subjectname { get; set; }

    [Column("emailid")]
    [StringLength(200)]
    public string? Emailid { get; set; }

    [Column("confirmationnumber")]
    [StringLength(200)]
    public string? Confirmationnumber { get; set; }

    [Column("filepath", TypeName = "character varying")]
    public string? Filepath { get; set; }

    [Column("roleid")]
    public int? Roleid { get; set; }

    [Column("requestid")]
    public int? Requestid { get; set; }

    [Column("adminid")]
    public int? Adminid { get; set; }

    [Column("physicianid")]
    public int? Physicianid { get; set; }

    [Column("sentdate", TypeName = "timestamp without time zone")]
    public DateTime? Sentdate { get; set; }

    [Column("isemailsent")]
    public bool? Isemailsent { get; set; }

    [Column("senttries")]
    public int? Senttries { get; set; }

    [Column("action")]
    public int? Action { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime Createdat { get; set; }

    [ForeignKey("Adminid")]
    [InverseProperty("Emaillogs")]
    public virtual Admin? Admin { get; set; }

    [ForeignKey("Physicianid")]
    [InverseProperty("Emaillogs")]
    public virtual Physician? Physician { get; set; }

    [ForeignKey("Requestid")]
    [InverseProperty("Emaillogs")]
    public virtual Request? Request { get; set; }

    [ForeignKey("Roleid")]
    [InverseProperty("Emaillogs")]
    public virtual Role? Role { get; set; }
}
