using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.DataModels;

[Table("chat")]
public partial class Chat
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("senderaspnetuserid")]
    public int? Senderaspnetuserid { get; set; }

    [Column("receiveraspnetuserid")]
    public int? Receiveraspnetuserid { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime? Createddate { get; set; }

    [Column("textcontent")]
    public string? Textcontent { get; set; }

    [ForeignKey("Receiveraspnetuserid")]
    [InverseProperty("ChatReceiveraspnetusers")]
    public virtual Aspnetuser? Receiveraspnetuser { get; set; }

    [ForeignKey("Senderaspnetuserid")]
    [InverseProperty("ChatSenderaspnetusers")]
    public virtual Aspnetuser? Senderaspnetuser { get; set; }
}
