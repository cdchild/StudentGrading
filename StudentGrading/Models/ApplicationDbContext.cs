using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace StudentGrading.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Section> Sections { get; set; }

        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<SectionAssignment> SectionAssignments { get; set; }

        public DbSet<Rubric> Rubrics { get; set; }

        public DbSet<RubricAspect> RubricAspects { get; set; }

        public DbSet<RatingAspect> RatingAspects { get; set; }

        public DbSet<Registration> Registrations { get; set; }

        public DbSet<Submission> Submissions { get; set; }

        public DbSet<SubmissionRatingItem> SubmissionRatingItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Rubric>()
                    .HasRequired(r => r.owningUser)
                    .WithMany()
                    .WillCascadeOnDelete(false);
            modelBuilder.Entity<Submission>()
                    .HasRequired(s => s.registration)
                    .WithMany()
                    .WillCascadeOnDelete(false);
            modelBuilder.Entity<Submission>()
                    .HasRequired(s => s.sectionAssignment)
                    .WithMany()
                    .WillCascadeOnDelete(false);
            modelBuilder.Entity<SectionAssignment>()
                    .HasRequired(s => s.section)
                    .WithMany()
                    .WillCascadeOnDelete(false);
            //modelBuilder.Entity<
        }

    }
}