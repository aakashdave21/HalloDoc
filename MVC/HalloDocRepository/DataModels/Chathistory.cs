using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.DataModels;

[PrimaryKey("Sender", "Receiver")]
[Table("chathistory")]
public partial class Chathistory
{
    [Column("id")]
    public int Id { get; set; }

    [Key]
    [Column("sender")]
    public int Sender { get; set; }

    [Key]
    [Column("receiver")]
    public int Receiver { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime Createdat { get; set; }

    [ForeignKey("Receiver")]
    [InverseProperty("ChathistoryReceiverNavigations")]
    public virtual Aspnetuser ReceiverNavigation { get; set; } = null!;

    [ForeignKey("Sender")]
    [InverseProperty("ChathistorySenderNavigations")]
    public virtual Aspnetuser SenderNavigation { get; set; } = null!;
}
