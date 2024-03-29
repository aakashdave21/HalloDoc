using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.DataModels;

[Table("rolemenu")]
public partial class Rolemenu
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("roleid")]
    public int Roleid { get; set; }

    [Column("menuid")]
    public int Menuid { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [ForeignKey("Menuid")]
    [InverseProperty("Rolemenus")]
    public virtual Menu Menu { get; set; } = null!;

    [ForeignKey("Roleid")]
    [InverseProperty("Rolemenus")]
    public virtual Role Role { get; set; } = null!;
}
