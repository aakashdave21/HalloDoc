using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.DataModels;

[Table("role")]
public partial class Role
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string? Name { get; set; }

    [Column("accounttype")]
    public short? Accounttype { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [Column("ip")]
    [StringLength(20)]
    public string? Ip { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [Column("createdBy")]
    public int? CreatedBy { get; set; }

    [Column("updatedBy")]
    public int? UpdatedBy { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<Admin> Admins { get; } = new List<Admin>();

    [InverseProperty("Role")]
    public virtual ICollection<Emaillog> Emaillogs { get; } = new List<Emaillog>();

    [InverseProperty("Role")]
    public virtual ICollection<Physician> Physicians { get; } = new List<Physician>();

    [InverseProperty("Role")]
    public virtual ICollection<Rolemenu> Rolemenus { get; } = new List<Rolemenu>();

    [InverseProperty("Role")]
    public virtual ICollection<Smslog> Smslogs { get; } = new List<Smslog>();
}
