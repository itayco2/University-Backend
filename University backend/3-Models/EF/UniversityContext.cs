using Microsoft.EntityFrameworkCore;

namespace University_backend
{
    public class UniversityContext : DbContext
    {
        public UniversityContext(DbContextOptions<UniversityContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<Lessons> Lessons { get; set; }
        public DbSet<Enrollments> Enrollments { get; set; }
        public DbSet<Progress> ProgressRecords { get; set; }
        public DbSet<Roles> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data for Roles
            modelBuilder.Entity<Roles>().HasData(
                new Roles
                {
                    RoleId = 1,
                    RoleName = "Admin"
                },
                new Roles
                {
                    RoleId = 2,
                    RoleName = "Student"
                },
                new Roles
                {
                    RoleId = 3,
                    RoleName = "Instructor"
                }
            );

            modelBuilder.Entity<Users>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<Enrollments>()
                .HasOne(e => e.User)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Enrollments>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);

            modelBuilder.Entity<Progress>()
                .HasOne(p => p.User)
                .WithMany(u => u.ProgressRecords)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Progress>()
                .HasOne(p => p.Lesson)
                .WithMany()
                .HasForeignKey(p => p.LessonId);

            modelBuilder.Entity<Courses>()
                .HasMany(c => c.Lessons)
                .WithOne(l => l.Course)
                .HasForeignKey(l => l.CourseId);

            modelBuilder.Entity<Courses>()
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Enrollments>()
                .Property(e => e.EnrolledAt)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
