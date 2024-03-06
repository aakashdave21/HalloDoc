using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.DataModels;

[Table("orderdetails")]
public partial class Orderdetail
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("vendorid")]
    public int? Vendorid { get; set; }

    [Column("requestid")]
    public int? Requestid { get; set; }

    [Column("faxnumber")]
    [StringLength(50)]
    public string? Faxnumber { get; set; }

    [Column("email")]
    [StringLength(50)]
    public string? Email { get; set; }

    [Column("businesscontact")]
    [StringLength(100)]
    public string? Businesscontact { get; set; }

    [Column("prescription")]
    public string? Prescription { get; set; }

    [Column("noofrefill")]
    public int? Noofrefill { get; set; }

    [Column("createdby")]
    public int? Createdby { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime Createddate { get; set; }

    [ForeignKey("Createdby")]
    [InverseProperty("Orderdetails")]
    public virtual Aspnetuser? CreatedbyNavigation { get; set; }

    [ForeignKey("Requestid")]
    [InverseProperty("Orderdetails")]
    public virtual Request? Request { get; set; }

    [ForeignKey("Vendorid")]
    [InverseProperty("Orderdetails")]
    public virtual Healthprofessional? Vendor { get; set; }
}
