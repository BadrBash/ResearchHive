using Microsoft.EntityFrameworkCore;
using Persistence.Extensions;
using Model.Entities;

namespace Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectDocument> ProjectDocuments { get; set; }
        public DbSet<StudentSupervisor> StudentSupervisors { get; set; }
        public DbSet<LecturerDepartment> LecturerDepartments { get; set; }
        public DbSet<ProjectSubmissionWindow> ProjectSubmissionWindows { get; set; }
        public  DbSet<Role> Roles { get; set; } 
        public  DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Research> Researches { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Student> Students { get; set; }    
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Department> Departments { get; set; } 
        public DbSet<User> Users { get; set; } 
        
        protected override  void OnModelCreating(ModelBuilder modelBuilder)
        {
            // In your DbContext's OnModelCreating
            modelBuilder.Entity<User>().HasIndex(exp => exp.Email);
            modelBuilder.Entity<Student>(en =>
            {
                en.HasOne(s => s.User)
                  .WithOne(u => u.Student)
                  .HasForeignKey("User", "UserId")
                  .OnDelete(DeleteBehavior.ClientSetNull);
            });
            modelBuilder.Entity<StudentSupervisor>(en =>
            {
                en.HasOne(s => s.Student)
                  .WithMany(u => u.StudentSupervisors)
                  .HasForeignKey(s => s.StudentId)
                  .OnDelete(DeleteBehavior.ClientSetNull);
            });
            modelBuilder.Entity<StudentSupervisor>(en =>
            {
                en.HasOne(s => s.Lecturer)
                  .WithMany(l => l.StudentSupervisors)
                  .HasForeignKey(l => l.LecturerId)
                  .OnDelete(DeleteBehavior.ClientSetNull);
            });
            modelBuilder.ApplyAllConfigurations<ApplicationDbContext>();
            modelBuilder.ConfigureDeletableEntities();
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateSoftDeleteStatuses();
            this.AddAuditInfo();
            return base.SaveChangesAsync(cancellationToken);
        }

        private const string IsDeletedProperty = "IsDeleted";

        
       
            private void UpdateSoftDeleteStatuses()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues[IsDeletedProperty] = false;
                        break;
                    case EntityState.Modified:
                        entry.CurrentValues[IsDeletedProperty] = false;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues[IsDeletedProperty] = true;
                        break;
                }
            }
        }
    }
}
