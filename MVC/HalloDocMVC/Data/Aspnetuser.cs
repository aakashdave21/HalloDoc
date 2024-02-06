using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocMVC.Data;

[Table("aspnetusers")]
[Index("Email", Name = "aspnetusers_email_key", IsUnique = true)]
[Index("Username", Name = "aspnetusers_username_key", IsUnique = true)]
public partial class Aspnetuser
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("username")]
    [StringLength(256)]
    public string Username { get; set; } = null!;

    [Column("passwordhash")]
    [StringLength(128)]
    public string Passwordhash { get; set; } = null!;

    [Column("securitystamp")]
    [StringLength(128)]
    public string? Securitystamp { get; set; }

    [Column("email")]
    [StringLength(256)]
    public string Email { get; set; } = null!;

    [Column("emailconfirmed")]
    public bool Emailconfirmed { get; set; }

    [Column("phonenumber")]
    [StringLength(128)]
    public string? Phonenumber { get; set; }

    [Column("phonenumberconfirmed")]
    public bool Phonenumberconfirmed { get; set; }

    [Column("twofactorenabled")]
    public bool Twofactorenabled { get; set; }

    [Column("lockoutenddateutc", TypeName = "timestamp without time zone")]
    public DateTime? Lockoutenddateutc { get; set; }

    [Column("lockoutenabled")]
    public bool Lockoutenabled { get; set; }

    [Column("accessfailedcount")]
    public int Accessfailedcount { get; set; }

    [Column("ip")]
    [StringLength(20)]
    public string? Ip { get; set; }

    [Column("corepasswordhash")]
    [StringLength(128)]
    public string? Corepasswordhash { get; set; }

    [Column("hashversion")]
    public int? Hashversion { get; set; }

    [Column("modifieddate", TypeName = "timestamp without time zone")]
    public DateTime? Modifieddate { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }
}
