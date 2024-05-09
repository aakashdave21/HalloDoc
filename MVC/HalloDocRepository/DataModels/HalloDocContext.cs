using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HalloDocRepository.DataModels;

public partial class HalloDocContext : DbContext
{
    public HalloDocContext()
    {
    }

    public HalloDocContext(DbContextOptions<HalloDocContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Adminregion> Adminregions { get; set; }

    public virtual DbSet<Aspnetrole> Aspnetroles { get; set; }

    public virtual DbSet<Aspnetuser> Aspnetusers { get; set; }

    public virtual DbSet<Aspnetuserrole> Aspnetuserroles { get; set; }

    public virtual DbSet<Blockrequest> Blockrequests { get; set; }

    public virtual DbSet<Casetag> Casetags { get; set; }

    public virtual DbSet<Concierge> Concierges { get; set; }

    public virtual DbSet<Emaillog> Emaillogs { get; set; }

    public virtual DbSet<Encounterform> Encounterforms { get; set; }

    public virtual DbSet<Healthprofessional> Healthprofessionals { get; set; }

    public virtual DbSet<Healthprofessionaltype> Healthprofessionaltypes { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Orderdetail> Orderdetails { get; set; }

    public virtual DbSet<Payrate> Payrates { get; set; }

    public virtual DbSet<Physician> Physicians { get; set; }

    public virtual DbSet<Physicianfile> Physicianfiles { get; set; }

    public virtual DbSet<Physicianlocation> Physicianlocations { get; set; }

    public virtual DbSet<Physicianregion> Physicianregions { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Requestclient> Requestclients { get; set; }

    public virtual DbSet<Requestconcierge> Requestconcierges { get; set; }

    public virtual DbSet<Requestnote> Requestnotes { get; set; }

    public virtual DbSet<Requeststatuslog> Requeststatuslogs { get; set; }

    public virtual DbSet<Requesttype> Requesttypes { get; set; }

    public virtual DbSet<Requestwisefile> Requestwisefiles { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Rolemenu> Rolemenus { get; set; }

    public virtual DbSet<Shift> Shifts { get; set; }

    public virtual DbSet<Shiftdetail> Shiftdetails { get; set; }

    public virtual DbSet<Shiftdetailregion> Shiftdetailregions { get; set; }

    public virtual DbSet<Smslog> Smslogs { get; set; }

    public virtual DbSet<Timesheet> Timesheets { get; set; }

    public virtual DbSet<Timesheetdetail> Timesheetdetails { get; set; }

    public virtual DbSet<Timesheetreimbursement> Timesheetreimbursements { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("User ID=postgres;Password=Aakash21##;Host=localhost;Port=5432;Database=HalloDoc;Pooling=true;Connection Lifetime=0;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("admin_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Createddate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Aspnetuser).WithMany(p => p.AdminAspnetusers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("admin_aspnetuserid_fkey");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.AdminCreatedbyNavigations).HasConstraintName("admin_createdby_fkey");

            entity.HasOne(d => d.ModifiedbyNavigation).WithMany(p => p.AdminModifiedbyNavigations).HasConstraintName("admin_modifiedby_fkey");

            entity.HasOne(d => d.Region).WithMany(p => p.Admins).HasConstraintName("admin_regionid_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Admins).HasConstraintName("admin_roleid_fkey");
        });

        modelBuilder.Entity<Adminregion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("adminregion_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Admin).WithMany(p => p.Adminregions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("adminregion_adminid_fkey");

            entity.HasOne(d => d.Region).WithMany(p => p.Adminregions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("adminregion_regionid_fkey");
        });

        modelBuilder.Entity<Aspnetrole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("aspnetroles_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Aspnetuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("aspnetusers_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Aspnetuserrole>(entity =>
        {
            entity.HasKey(e => new { e.Userid, e.Roleid }).HasName("aspnetuserroles_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Role).WithMany(p => p.Aspnetuserroles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("aspnetuserroles_roleid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Aspnetuserroles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("aspnetuserroles_userid_fkey");
        });

        modelBuilder.Entity<Blockrequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("blockrequests_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Request).WithMany(p => p.Blockrequests)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("blockrequests_requestid_fkey");
        });

        modelBuilder.Entity<Casetag>(entity =>
        {
            entity.HasKey(e => e.Casetagid).HasName("casetag_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Concierge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("concierge_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Createddate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Region).WithMany(p => p.Concierges).HasConstraintName("concierge_regionid_fkey");
        });

        modelBuilder.Entity<Emaillog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("emaillog_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Admin).WithMany(p => p.Emaillogs).HasConstraintName("emaillog_adminid_fkey");

            entity.HasOne(d => d.Physician).WithMany(p => p.Emaillogs).HasConstraintName("emaillog_physicianid_fkey");

            entity.HasOne(d => d.Request).WithMany(p => p.Emaillogs)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("emaillog_requestid_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Emaillogs).HasConstraintName("emaillog_roleid_fkey");
        });

        modelBuilder.Entity<Encounterform>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("encounterform_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Isfinalized).HasDefaultValueSql("false");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Request).WithOne(p => p.Encounterform)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("encounterform_request_id_fkey");
        });

        modelBuilder.Entity<Healthprofessional>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("healthprofessionals_pkey");

            entity.Property(e => e.Createddate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Modifieddate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.ProfessionNavigation).WithMany(p => p.Healthprofessionals).HasConstraintName("healthprofessionals_profession_fkey");

            entity.HasOne(d => d.Region).WithMany(p => p.Healthprofessionals).HasConstraintName("healthprofessionals_regionid_fkey");
        });

        modelBuilder.Entity<Healthprofessionaltype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("healthprofessionaltype_pkey");

            entity.Property(e => e.Createddate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Isactive).HasDefaultValueSql("true");
            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("menu_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Orderdetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orderdetails_pkey");

            entity.Property(e => e.Createddate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.Orderdetails).HasConstraintName("orderdetails_createdby_fkey");

            entity.HasOne(d => d.Request).WithMany(p => p.Orderdetails)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("orderdetails_requestid_fkey");

            entity.HasOne(d => d.Vendor).WithMany(p => p.Orderdetails).HasConstraintName("orderdetails_vendorid_fkey");
        });

        modelBuilder.Entity<Payrate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("payrate_pkey");

            entity.Property(e => e.Batchtesting).HasDefaultValueSql("0");
            entity.Property(e => e.Housecall).HasDefaultValueSql("0");
            entity.Property(e => e.Housecallnightweekend).HasDefaultValueSql("0");
            entity.Property(e => e.Nightshiftweekend).HasDefaultValueSql("0");
            entity.Property(e => e.Phoneconsult).HasDefaultValueSql("0");
            entity.Property(e => e.Phoneconsultnightweekend).HasDefaultValueSql("0");
            entity.Property(e => e.Shift).HasDefaultValueSql("0");

            entity.HasOne(d => d.Physician).WithMany(p => p.Payrates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payrate_physicianid_fkey");
        });

        modelBuilder.Entity<Physician>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("physician_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Createddate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsNotificationStop).HasDefaultValueSql("false");
            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Aspnetuser).WithMany(p => p.PhysicianAspnetusers).HasConstraintName("physician_aspnetuserid_fkey");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.PhysicianCreatedbyNavigations).HasConstraintName("physician_createdby_fkey");

            entity.HasOne(d => d.ModifiedbyNavigation).WithMany(p => p.PhysicianModifiedbyNavigations).HasConstraintName("physician_modifiedby_fkey");

            entity.HasOne(d => d.Region).WithMany(p => p.Physicians).HasConstraintName("physician_regionid_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Physicians).HasConstraintName("physician_roleid_fkey");
        });

        modelBuilder.Entity<Physicianfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("physicianfile_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Physician).WithOne(p => p.Physicianfile)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("physicianfile_physicianid_fkey");
        });

        modelBuilder.Entity<Physicianlocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("physicianlocation_pkey");

            entity.Property(e => e.Createddate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Physician).WithOne(p => p.Physicianlocation).HasConstraintName("physicianlocation_physicianid_fkey");
        });

        modelBuilder.Entity<Physicianregion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("physicianregion_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Physician).WithMany(p => p.Physicianregions).HasConstraintName("physicianregion_physicianid_fkey");

            entity.HasOne(d => d.Region).WithMany(p => p.Physicianregions).HasConstraintName("physicianregion_regionid_fkey");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("region_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("request_pkey");

            entity.Property(e => e.Completedbyphysician).HasDefaultValueSql("false");
            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsBlocked).HasDefaultValueSql("false");
            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Createduser).WithMany(p => p.Requests).HasConstraintName("request_createduserid_fkey");

            entity.HasOne(d => d.Physician).WithMany(p => p.Requests).HasConstraintName("request_physicianid_fkey");

            entity.HasOne(d => d.Requesttype).WithMany(p => p.Requests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("request_requesttypeid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Requests).HasConstraintName("request_userid_fkey");
        });

        modelBuilder.Entity<Requestclient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("requestclient_pkey");

            entity.HasOne(d => d.Region).WithMany(p => p.Requestclients).HasConstraintName("requestclient_regionid_fkey");

            entity.HasOne(d => d.Request).WithMany(p => p.Requestclients).HasConstraintName("requestclient_requestid_fkey");
        });

        modelBuilder.Entity<Requestconcierge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("requestconcierge_pkey");

            entity.HasOne(d => d.Concierge).WithMany(p => p.Requestconcierges)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("requestconcierge_conciergeid_fkey");

            entity.HasOne(d => d.Request).WithMany(p => p.Requestconcierges).HasConstraintName("requestconcierge_requestid_fkey");
        });

        modelBuilder.Entity<Requestnote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("requestnotes_pkey");

            entity.Property(e => e.Createddate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Modifieddate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.RequestnoteCreatedbyNavigations).HasConstraintName("requestnotes_createdby_fkey");

            entity.HasOne(d => d.ModifiedbyNavigation).WithMany(p => p.RequestnoteModifiedbyNavigations).HasConstraintName("requestnotes_modifiedby_fkey");

            entity.HasOne(d => d.Request).WithOne(p => p.Requestnote).HasConstraintName("requestnotes_requestid_fkey");
        });

        modelBuilder.Entity<Requeststatuslog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("requeststatuslog_pkey");

            entity.Property(e => e.Createddate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Transtoadmin).HasDefaultValueSql("false");
            entity.Property(e => e.Updateddate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Admin).WithMany(p => p.Requeststatuslogs).HasConstraintName("requeststatuslog_adminid_fkey");

            entity.HasOne(d => d.Physician).WithMany(p => p.RequeststatuslogPhysicians).HasConstraintName("requeststatuslog_physicianid_fkey");

            entity.HasOne(d => d.Request).WithMany(p => p.Requeststatuslogs).HasConstraintName("requeststatuslog_requestid_fkey");

            entity.HasOne(d => d.Transtophysician).WithMany(p => p.RequeststatuslogTranstophysicians).HasConstraintName("requeststatuslog_transtophysicianid_fkey");
        });

        modelBuilder.Entity<Requesttype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("requesttype_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Requestwisefile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("requestwisefile_pkey");

            entity.HasOne(d => d.Admin).WithMany(p => p.Requestwisefiles).HasConstraintName("requestwisefile_adminid_fkey");

            entity.HasOne(d => d.Physician).WithMany(p => p.Requestwisefiles).HasConstraintName("requestwisefile_physicianid_fkey");

            entity.HasOne(d => d.Request).WithMany(p => p.Requestwisefiles).HasConstraintName("requestwisefile_requestid_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Isdeleted).HasDefaultValueSql("false");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Rolemenu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("rolemenu_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Menu).WithMany(p => p.Rolemenus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rolemenu_menuid_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Rolemenus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rolemenu_roleid_fkey");
        });

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("shift_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Weekdays).IsFixedLength();

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.Shifts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("shift_createdby_fkey");

            entity.HasOne(d => d.Physician).WithMany(p => p.Shifts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("shift_physicianid_fkey");
        });

        modelBuilder.Entity<Shiftdetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("shiftdetail_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.ShiftdetailCreatedbyNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("shiftdetail_createdby_fkey");

            entity.HasOne(d => d.ModifiedbyNavigation).WithMany(p => p.ShiftdetailModifiedbyNavigations).HasConstraintName("shiftdetail_modifiedby_fkey");

            entity.HasOne(d => d.Region).WithMany(p => p.Shiftdetails).HasConstraintName("shiftdetail_regionid_fkey");

            entity.HasOne(d => d.Shift).WithMany(p => p.Shiftdetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("shiftdetail_shiftid_fkey");
        });

        modelBuilder.Entity<Shiftdetailregion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("shiftdetailregion_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Region).WithMany(p => p.Shiftdetailregions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("shiftdetailregion_regionid_fkey");

            entity.HasOne(d => d.Shiftdetail).WithMany(p => p.Shiftdetailregions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("shiftdetailregion_shiftdetailid_fkey");
        });

        modelBuilder.Entity<Smslog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("smslog_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Admin).WithMany(p => p.Smslogs).HasConstraintName("smslog_adminid_fkey");

            entity.HasOne(d => d.Physician).WithMany(p => p.Smslogs).HasConstraintName("smslog_physicianid_fkey");

            entity.HasOne(d => d.Request).WithMany(p => p.Smslogs)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("smslog_requestid_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Smslogs).HasConstraintName("smslog_roleid_fkey");
        });

        modelBuilder.Entity<Timesheet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("timesheet_pkey");

            entity.Property(e => e.Bonus).HasDefaultValueSql("0");
            entity.Property(e => e.Isfinalized).HasDefaultValueSql("false");

            entity.HasOne(d => d.Physician).WithMany(p => p.Timesheets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("timesheet_physicianid_fkey");
        });

        modelBuilder.Entity<Timesheetdetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("timesheetdetails_pkey");

            entity.Property(e => e.Housecall).HasDefaultValueSql("0");
            entity.Property(e => e.Isweekend).HasDefaultValueSql("false");
            entity.Property(e => e.Phoneconsult).HasDefaultValueSql("0");
            entity.Property(e => e.Shifthours).HasDefaultValueSql("0");

            entity.HasOne(d => d.Timesheet).WithMany(p => p.Timesheetdetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("timesheetdetails_timesheetid_fkey");
        });

        modelBuilder.Entity<Timesheetreimbursement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("timesheetreimbursement_pkey");

            entity.HasOne(d => d.Timesheet).WithMany(p => p.Timesheetreimbursements)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("timesheetreimbursement_timesheetid_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Updatedat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Aspnetuser).WithMany(p => p.Users).HasConstraintName("users_aspnetuserid_fkey");

            entity.HasOne(d => d.Region).WithMany(p => p.Users).HasConstraintName("users_regionid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
