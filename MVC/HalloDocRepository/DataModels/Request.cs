using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.DataModels;

[Table("request")]
public partial class Request
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("requesttypeid")]
    public int Requesttypeid { get; set; }

    [Column("userid")]
    public int? Userid { get; set; }

    [Column("firstname")]
    [StringLength(100)]
    public string? Firstname { get; set; }

    [Column("lastname")]
    [StringLength(100)]
    public string? Lastname { get; set; }

    [Column("phonenumber")]
    [StringLength(23)]
    public string? Phonenumber { get; set; }

    [Column("email")]
    [StringLength(50)]
    public string? Email { get; set; }

    [Column("status")]
    public short Status { get; set; }

    [Column("physicianid")]
    public int? Physicianid { get; set; }

    [Column("confirmationnumber")]
    [StringLength(20)]
    public string? Confirmationnumber { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime Createddate { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [Column("modifieddate", TypeName = "timestamp without time zone")]
    public DateTime? Modifieddate { get; set; }

    [Column("declinedby")]
    [StringLength(250)]
    public string? Declinedby { get; set; }

    [Column("isurgentemailsent")]
    public bool? Isurgentemailsent { get; set; }

    [Column("lastwellnessdate", TypeName = "timestamp without time zone")]
    public DateTime? Lastwellnessdate { get; set; }

    [Column("ismobile")]
    public bool? Ismobile { get; set; }

    [Column("calltype")]
    public short? Calltype { get; set; }

    [Column("completedbyphysician")]
    public bool? Completedbyphysician { get; set; }

    [Column("lastreservationdate", TypeName = "timestamp without time zone")]
    public DateTime? Lastreservationdate { get; set; }

    [Column("accepteddate", TypeName = "timestamp without time zone")]
    public DateTime? Accepteddate { get; set; }

    [Column("relationname")]
    [StringLength(100)]
    public string? Relationname { get; set; }

    [Column("casenumber")]
    [StringLength(50)]
    public string? Casenumber { get; set; }

    [Column("ip")]
    [StringLength(20)]
    public string? Ip { get; set; }

    [Column("casetag")]
    [StringLength(50)]
    public string? Casetag { get; set; }

    [Column("casetagphysician")]
    [StringLength(50)]
    public string? Casetagphysician { get; set; }

    [Column("patientaccountid")]
    [StringLength(128)]
    public string? Patientaccountid { get; set; }

    [Column("createduserid")]
    public int? Createduserid { get; set; }

    [Column("symptoms")]
    public string? Symptoms { get; set; }

    [Column("roomnoofpatient")]
    [StringLength(250)]
    public string? Roomnoofpatient { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [StringLength(250)]
    public string? PropertyName { get; set; }

    public int? NoOfRequests { get; set; }

    public bool? IsBlocked { get; set; }

    [Column("acceptToken")]
    public string? AcceptToken { get; set; }

    [Column("acceptExpiry")]
    public DateTime? AcceptExpiry { get; set; }

    [InverseProperty("Request")]
    public virtual ICollection<Blockrequest> Blockrequests { get; } = new List<Blockrequest>();

    [ForeignKey("Createduserid")]
    [InverseProperty("RequestCreatedusers")]
    public virtual User? Createduser { get; set; }

    [InverseProperty("Request")]
    public virtual ICollection<Emaillog> Emaillogs { get; } = new List<Emaillog>();

    [InverseProperty("Request")]
    public virtual Encounterform? Encounterform { get; set; }

    [InverseProperty("Request")]
    public virtual ICollection<Orderdetail> Orderdetails { get; } = new List<Orderdetail>();

    [ForeignKey("Physicianid")]
    [InverseProperty("Requests")]
    public virtual Physician? Physician { get; set; }

    [InverseProperty("Request")]
    public virtual ICollection<Requestclient> Requestclients { get; } = new List<Requestclient>();

    [InverseProperty("Request")]
    public virtual ICollection<Requestconcierge> Requestconcierges { get; } = new List<Requestconcierge>();

    [InverseProperty("Request")]
    public virtual Requestnote? Requestnote { get; set; }

    [InverseProperty("Request")]
    public virtual ICollection<Requeststatuslog> Requeststatuslogs { get; } = new List<Requeststatuslog>();

    [ForeignKey("Requesttypeid")]
    [InverseProperty("Requests")]
    public virtual Requesttype Requesttype { get; set; } = null!;

    [InverseProperty("Request")]
    public virtual ICollection<Requestwisefile> Requestwisefiles { get; } = new List<Requestwisefile>();

    [InverseProperty("Request")]
    public virtual ICollection<Smslog> Smslogs { get; } = new List<Smslog>();

    [ForeignKey("Userid")]
    [InverseProperty("RequestUsers")]
    public virtual User? User { get; set; }
}
