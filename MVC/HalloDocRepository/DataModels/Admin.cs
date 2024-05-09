using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.DataModels;

[Table("admin")]
public partial class Admin
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("aspnetuserid")]
    public int Aspnetuserid { get; set; }

    [Column("firstname")]
    [StringLength(100)]
    public string Firstname { get; set; } = null!;

    [Column("lastname")]
    [StringLength(100)]
    public string? Lastname { get; set; }

    [Column("email")]
    [StringLength(50)]
    public string Email { get; set; } = null!;

    [Column("mobile")]
    [StringLength(20)]
    public string? Mobile { get; set; }

    [Column("address1")]
    [StringLength(500)]
    public string? Address1 { get; set; }

    [Column("address2")]
    [StringLength(500)]
    public string? Address2 { get; set; }

    [Column("city")]
    [StringLength(100)]
    public string? City { get; set; }

    [Column("regionid")]
    public int? Regionid { get; set; }

    [Column("zip")]
    [StringLength(10)]
    public string? Zip { get; set; }

    [Column("altphone")]
    [StringLength(20)]
    public string? Altphone { get; set; }

    [Column("createdby")]
    public int? Createdby { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime Createddate { get; set; }

    [Column("modifiedby")]
    public int? Modifiedby { get; set; }

    [Column("modifieddate", TypeName = "timestamp without time zone")]
    public DateTime? Modifieddate { get; set; }

    [Column("status")]
    public short? Status { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [Column("roleid")]
    public int? Roleid { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [InverseProperty("Admin")]
    public virtual ICollection<Adminregion> Adminregions { get; } = new List<Adminregion>();

    [ForeignKey("Aspnetuserid")]
    [InverseProperty("AdminAspnetusers")]
    public virtual Aspnetuser Aspnetuser { get; set; } = null!;

    [ForeignKey("Createdby")]
    [InverseProperty("AdminCreatedbyNavigations")]
    public virtual Aspnetuser? CreatedbyNavigation { get; set; }

    [InverseProperty("Admin")]
    public virtual ICollection<Emaillog> Emaillogs { get; } = new List<Emaillog>();

    [ForeignKey("Modifiedby")]
    [InverseProperty("AdminModifiedbyNavigations")]
    public virtual Aspnetuser? ModifiedbyNavigation { get; set; }

    [ForeignKey("Regionid")]
    [InverseProperty("Admins")]
    public virtual Region? Region { get; set; }

    [InverseProperty("Admin")]
    public virtual ICollection<Requeststatuslog> Requeststatuslogs { get; } = new List<Requeststatuslog>();

    [InverseProperty("Admin")]
    public virtual ICollection<Requestwisefile> Requestwisefiles { get; } = new List<Requestwisefile>();

    [ForeignKey("Roleid")]
    [InverseProperty("Admins")]
    public virtual Role? Role { get; set; }

    [InverseProperty("Admin")]
    public virtual ICollection<Smslog> Smslogs { get; } = new List<Smslog>();
}
