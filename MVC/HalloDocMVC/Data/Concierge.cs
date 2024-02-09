using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocMVC.Data;

[Table("concierge")]
public partial class Concierge
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("conciergename")]
    [StringLength(100)]
    public string? Conciergename { get; set; }

    [Column("address")]
    [StringLength(150)]
    public string? Address { get; set; }

    [Column("street")]
    [StringLength(50)]
    public string? Street { get; set; }

    [Column("city")]
    [StringLength(50)]
    public string? City { get; set; }

    [Column("state")]
    [StringLength(50)]
    public string? State { get; set; }

    [Column("zipcode")]
    [StringLength(50)]
    public string? Zipcode { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime Createddate { get; set; }

    [Column("regionid")]
    public int? Regionid { get; set; }

    [Column("ip")]
    [StringLength(20)]
    public string? Ip { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [ForeignKey("Regionid")]
    [InverseProperty("Concierges")]
    public virtual Region? Region { get; set; }

    [InverseProperty("Concierge")]
    public virtual ICollection<Requestconcierge> Requestconcierges { get; } = new List<Requestconcierge>();
}
