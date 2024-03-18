using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.DataModels;

[Table("encounterform")]
[Index("RequestId", Name = "encounterform_request_id_key", IsUnique = true)]
public partial class Encounterform
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("request_id")]
    public int? RequestId { get; set; }

    [Column("dateofservice")]
    public DateOnly? Dateofservice { get; set; }

    [Column("historyofpresentillness")]
    public string? Historyofpresentillness { get; set; }

    [Column("medicalhistory")]
    public string? Medicalhistory { get; set; }

    [Column("medications")]
    public string? Medications { get; set; }

    [Column("allergies")]
    public string? Allergies { get; set; }

    [Column("temperature")]
    [Precision(5, 2)]
    public decimal? Temperature { get; set; }

    [Column("heartrate")]
    public int? Heartrate { get; set; }

    [Column("respiratoryrate")]
    public int? Respiratoryrate { get; set; }

    [Column("bloodpressure")]
    [StringLength(20)]
    public string? Bloodpressure { get; set; }

    [Column("o2")]
    public int? O2 { get; set; }

    [Column("pain")]
    public int? Pain { get; set; }

    [Column("heent")]
    public string? Heent { get; set; }

    [Column("cv")]
    public string? Cv { get; set; }

    [Column("chest")]
    public string? Chest { get; set; }

    [Column("abd")]
    public string? Abd { get; set; }

    [Column("extr")]
    public string? Extr { get; set; }

    [Column("skin")]
    public string? Skin { get; set; }

    [Column("neuro")]
    public string? Neuro { get; set; }

    [Column("other")]
    public string? Other { get; set; }

    [Column("diagnosis")]
    public string? Diagnosis { get; set; }

    [Column("treatmentplan")]
    public string? Treatmentplan { get; set; }

    [Column("medicationdispensed")]
    public string? Medicationdispensed { get; set; }

    [Column("procedures")]
    public string? Procedures { get; set; }

    [Column("followup")]
    public string? Followup { get; set; }

    [Column("isfinalized")]
    public bool? Isfinalized { get; set; }

    [Column("finalizeddate", TypeName = "timestamp without time zone")]
    public DateTime? Finalizeddate { get; set; }

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime? Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime? Updatedat { get; set; }

    [ForeignKey("RequestId")]
    [InverseProperty("Encounterform")]
    public virtual Request? Request { get; set; }
}
