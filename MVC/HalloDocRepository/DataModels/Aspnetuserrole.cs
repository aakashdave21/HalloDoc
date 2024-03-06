using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.DataModels;

[PrimaryKey("Userid", "Roleid")]
[Table("aspnetuserroles")]
[Index("Id", Name = "aspnetuserroles_id_key", IsUnique = true)]
public partial class Aspnetuserrole
{
    [Column("id")]
    public int Id { get; set; }

    [Key]
    [Column("userid")]
    public int Userid { get; set; }

    [Key]
    [Column("roleid")]
    public int Roleid { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [ForeignKey("Roleid")]
    [InverseProperty("Aspnetuserroles")]
    public virtual Aspnetrole Role { get; set; } = null!;

    [ForeignKey("Userid")]
    [InverseProperty("Aspnetuserroles")]
    public virtual Aspnetuser User { get; set; } = null!;
}
