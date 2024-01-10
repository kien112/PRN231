***REMOVED***using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ScoreManagementApi.Core.Entities;

namespace ScoreManagementApi.Core.DbContext
***REMOVED***
    public class ApplicationDbContext : IdentityDbContext<User>
    ***REMOVED***

        public DbSet<Subject> Subjects ***REMOVED*** get; set; ***REMOVED***
        public DbSet<ClassRoom> ClassRooms ***REMOVED*** get; set; ***REMOVED***
        public DbSet<ComponentScore> ComponentScores ***REMOVED*** get; set; ***REMOVED***
        public DbSet<Score> Scores ***REMOVED*** get; set; ***REMOVED***
        public DbSet<ClassStudent> ClassStudents ***REMOVED*** get; set; ***REMOVED***

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        ***REMOVED*** 
            
***REMOVED***

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        ***REMOVED***
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ClassRoom>()
                .HasOne(c => c.Teacher)
                .WithMany()
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClassRoom>()
                .HasOne(c => c.Creator)
                .WithMany()
                .HasForeignKey(c => c.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);
***REMOVED***

***REMOVED***
***REMOVED***
