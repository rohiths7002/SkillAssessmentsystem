using Microsoft.EntityFrameworkCore;
using SkillAssessmentSystem.Models;

namespace SkillAssessmentSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Result> Results { get; set; }
    public DbSet<Assessment> Assessments { get; set; }
public DbSet<Certificate> Certificates { get; set; }
public DbSet<Question> Questions { get; set; }
}
}