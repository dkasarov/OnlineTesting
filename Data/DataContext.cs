using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineTesting.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTesting.Data
{
    public class DataContext : IdentityDbContext<User, Role, int,
        IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Test> Tests { get; set; }

        public DbSet<TestQuestion> TestQuestions { get; set; }

        public DbSet<TestQuestionType> TestQuestionTypes { get; set; }

        public DbSet<TestQuestionAnswer> TestQuestionAnswers { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Exam> Exams { get; set; }

        public DbSet<ExamAnswer> ExamAnswers { get; set; }

        public DbSet<StudentToTest> StudentToTests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<Exam>()
                .HasKey(k => new { k.StudentId, k.TestQuestionId, k.ExamAnswerId });

            builder.Entity<StudentToTest>(studentToTests =>
            {
                studentToTests.HasKey(k => k.Id);

                studentToTests
                    .HasOne(u => u.User)
                    .WithMany(s => s.StudentToTests)
                    .HasForeignKey(us => us.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                studentToTests
                    .HasOne(u => u.Test)
                    .WithMany(s => s.StudentToTests)
                    .HasForeignKey(us => us.TestId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Exam>()
                .HasOne(u => u.ExamAnswer)
                .WithMany(u => u.Exams)
                .HasForeignKey(u => u.ExamAnswerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Exam>()
                .HasOne(u => u.Student)
                .WithMany(u => u.Exams)
                .HasForeignKey(u => u.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Exam>()
                .HasOne(u => u.TestQuestion)
                .WithMany(m => m.Exams)
                .HasForeignKey(u => u.TestQuestionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
