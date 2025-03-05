using Microsoft.EntityFrameworkCore;

namespace University_backend;

public class UniversityContext : DbContext
{
    public UniversityContext(DbContextOptions<UniversityContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Progress> Progress { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Progress Relationships (User-Lesson Many-to-Many)
        modelBuilder.Entity<Progress>()
            .HasOne(p => p.User)
            .WithMany(u => u.ProgressRecords)
            .HasForeignKey(p => p.UserId);

        modelBuilder.Entity<Progress>()
            .HasOne(p => p.Lesson)
            .WithMany(l => l.ProgressRecords)
            .HasForeignKey(p => p.LessonId);

        // Enrollment Relationships (User-Course Many-to-Many)
        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.User)
            .WithMany(u => u.Enrollments)
            .HasForeignKey(e => e.UserId);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId);
    }
}