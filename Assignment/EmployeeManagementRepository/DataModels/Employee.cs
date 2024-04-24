using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementRepository.DataModels;

[Table("employee")]
public partial class Employee
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("firstname")]
    [StringLength(50)]
    public string? Firstname { get; set; }

    [Column("lastname")]
    [StringLength(50)]
    public string? Lastname { get; set; }

    [Column("dept_id")]
    public int? DeptId { get; set; }

    [Column("age")]
    public int? Age { get; set; }

    [Column("gender")]
    public int? Gender { get; set; }

    [Column("email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Column("education")]
    public int? Education { get; set; }

    [Column("company")]
    [StringLength(50)]
    public string? Company { get; set; }

    [Column("experience")]
    public int? Experience { get; set; }

    [Column("package")]
    public decimal? Package { get; set; }

    [ForeignKey("DeptId")]
    [InverseProperty("Employees")]
    public virtual Department? Dept { get; set; }
}
