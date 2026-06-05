using HRLConstructionManagerAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRLConstructionManagerAPI.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    private static readonly DateTime SeedCreatedOn = new(2026, 5, 17, 0, 0, 0, DateTimeKind.Utc);

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<User> Users => Set<User>();

    public DbSet<Site> Sites => Set<Site>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Contractor> Contractors => Set<Contractor>();

    public DbSet<SupervisorCredit> SupervisorCredits => Set<SupervisorCredit>();

    public DbSet<Expense> Expenses => Set<Expense>();

    public DbSet<Attendance> Attendances => Set<Attendance>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Roles");

            entity.HasKey(role => role.Id);

            entity.Property(role => role.RoleName)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(role => role.Enable)
                .IsRequired();

            entity.Property(role => role.CreatedOn)
                .IsRequired();

            entity.HasIndex(role => role.RoleName)
                .IsUnique();

            entity.HasData(
                new Role
                {
                    Id = 1,
                    RoleName = "admin",
                    Enable = true,
                    CreatedOn = SeedCreatedOn
                },
                new Role
                {
                    Id = 2,
                    RoleName = "supervision",
                    Enable = true,
                    CreatedOn = SeedCreatedOn
                });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");

            entity.HasKey(user => user.Id);

            entity.Property(user => user.MobileNumber)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(user => user.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(user => user.Password)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(user => user.Address)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(user => user.Enable)
                .IsRequired();

            entity.Property(user => user.Email)
                .HasMaxLength(256);

            entity.Property(user => user.CreatedBy)
                .HasMaxLength(100);

            entity.Property(user => user.ModifiedBy)
                .HasMaxLength(100);

            entity.Property(user => user.CreatedOn)
                .IsRequired();

            entity.HasIndex(user => user.MobileNumber)
                .IsUnique();

            entity.HasOne(user => user.Role)
                .WithMany(role => role.Users)
                .HasForeignKey(user => user.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasData(
                new User
                {
                    Id = 1,
                    MobileNumber = "9999999999",
                    Name = "Admin User",
                    Password = "admin",
                    RoleId = 1,
                    Address = "Head Office",
                    Email = "admin@example.com",
                    Enable = true,
                    CreatedOn = SeedCreatedOn,
                    CreatedBy = "system"
                },
                new User
                {
                    Id = 2,
                    MobileNumber = "8888888888",
                    Name = "Supervision User",
                    Password = "supervision",
                    RoleId = 2,
                    Address = "Site Office",
                    Email = "supervision@example.com",
                    Enable = true,
                    CreatedOn = SeedCreatedOn,
                    CreatedBy = "system"
                });
        });

        modelBuilder.Entity<Site>(entity =>
        {
            entity.ToTable("Sites");

            entity.HasKey(site => site.Id);

            entity.Property(site => site.SiteName)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(site => site.Address)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(site => site.City)
                .HasMaxLength(100);

            entity.Property(site => site.State)
                .HasMaxLength(100);

            entity.Property(site => site.ContactPerson)
                .HasMaxLength(100);

            entity.Property(site => site.ContactNumber)
                .HasMaxLength(20);

            entity.Property(site => site.Enable)
                .IsRequired();

            entity.Property(site => site.CreatedOn)
                .IsRequired();

            entity.Property(site => site.CreatedBy)
                .HasMaxLength(100);

            entity.Property(site => site.ModifiedBy)
                .HasMaxLength(100);

            entity.HasIndex(site => site.SiteName);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(c => c.Description)
                .HasMaxLength(500);

            entity.Property(c => c.Enable)
                .IsRequired();

            entity.Property(c => c.CreatedOn)
                .IsRequired();

            entity.Property(c => c.CreatedBy)
                .HasMaxLength(100);

            entity.Property(c => c.ModifiedBy)
                .HasMaxLength(100);

            entity.HasIndex(c => c.Name);
        });

        modelBuilder.Entity<Contractor>(entity =>
        {
            entity.ToTable("Contractors");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.ContractorName)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(c => c.Email)
                .HasMaxLength(256)
                .IsRequired();

            entity.Property(c => c.Phone)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(c => c.AssignedSites)
                .HasMaxLength(1000);

            entity.Property(c => c.Enable)
                .IsRequired();

            entity.Property(c => c.CreatedOn)
                .IsRequired();

            entity.Property(c => c.CreatedBy)
                .HasMaxLength(100);

            entity.Property(c => c.ModifiedBy)
                .HasMaxLength(100);

            entity.HasIndex(c => c.ContractorName);
            entity.HasIndex(c => c.Email);
            entity.HasIndex(c => c.Phone);
        });

        modelBuilder.Entity<SupervisorCredit>(entity =>
        {
            entity.ToTable("SupervisorCredits");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.SupervisorName)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(c => c.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(c => c.PaymentMode)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(c => c.TransactionId)
                .HasMaxLength(100);

            entity.Property(c => c.Comment)
                .HasMaxLength(500);

            entity.Property(c => c.Date)
                .HasColumnType("date")
                .IsRequired();

            entity.Property(c => c.CreatedOn)
                .IsRequired();

            entity.Property(c => c.CreatedBy)
                .HasMaxLength(100);

            entity.Property(c => c.ModifiedBy)
                .HasMaxLength(100);

            entity.HasIndex(c => c.SupervisorName);
            entity.HasIndex(c => c.PaymentMode);
            entity.HasIndex(c => c.Date);
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.ToTable("Expenses");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Title).HasMaxLength(250).IsRequired();
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.PaymentMode).HasMaxLength(50).IsRequired();
            entity.Property(e => e.TransactionId).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Date).HasColumnType("date").IsRequired();
            entity.Property(e => e.CreatedOn).IsRequired();
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.ModifiedBy).HasMaxLength(100);

            entity.HasOne(e => e.Site)
                .WithMany()
                .HasForeignKey(e => e.SiteId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => e.Date);
            entity.HasIndex(e => e.Type);
        });

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.ToTable("Attendances");

            entity.HasKey(a => a.Id);

            entity.Property(a => a.Date).HasColumnType("date").IsRequired();
            entity.Property(a => a.SkilledWorkers).IsRequired();
            entity.Property(a => a.SemiSkilledWorkers).IsRequired();
            entity.Property(a => a.UnskilledWorkers).IsRequired();
            entity.Property(a => a.StartTime).IsRequired();
            entity.Property(a => a.EndTime).IsRequired();
            
            entity.Property(a => a.CreatedOn).IsRequired();
            entity.Property(a => a.CreatedBy).HasMaxLength(100);
            entity.Property(a => a.ModifiedBy).HasMaxLength(100);

            entity.HasOne(a => a.Site)
                .WithMany()
                .HasForeignKey(a => a.SiteId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.Contractor)
                .WithMany()
                .HasForeignKey(a => a.ContractorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.Supervisor)
                .WithMany()
                .HasForeignKey(a => a.SupervisorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(a => a.Date);
        });
    }
}
