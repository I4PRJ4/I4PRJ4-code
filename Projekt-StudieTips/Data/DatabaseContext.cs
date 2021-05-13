using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Projekt_StudieTips.Models;

namespace Projekt_StudieTips.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) { }
        public DbSet<Degree> Degrees { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Tip> Tips { get; set; }

        public DbSet<User> Users { get; set; } //SKAL FJERNES SENERE


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Degree>().HasIndex(d => d.DegreeName).IsUnique();




            modelBuilder.Entity<Course>().HasOne(c => c.Degrees)
                .WithMany(d => d.Courses)
                .HasForeignKey(c => c.DegreeId);

            // Tip
            modelBuilder.Entity<Tip>().HasOne(t => t.Course)
                .WithMany(c => c.Tips)
                .HasForeignKey(t => t.CourseId);

            modelBuilder.Entity<Tip>().HasOne(t => t.User)
                .WithMany(c => c.Tips)
                .HasForeignKey(t => t.UserId);


            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Username = "TronaldDump" },
                new User { UserId = 2, Username = "SwedishNoob" },
                new User { UserId = 3, Username = "TheLegend" },
                new User { UserId = 4, Username = "IntrovertedSnail" }
            );

            modelBuilder.Entity<Degree>().HasData(
                new Degree { DegreeId = 1, DegreeName = "Softwareteknologi" }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course { CourseId = 1, CourseName = "Generelt", DegreeId = 1},
                new Course { CourseId = 2, CourseName = "Softwaretest", DegreeId = 1 },
                new Course { CourseId = 3, CourseName = "Database", DegreeId = 1 },
                new Course { CourseId = 4, CourseName = "GUI", DegreeId = 1 }
            );

            modelBuilder.Entity<Tip>().HasData(
                new Tip { TipId = 1, UserId = 1, CourseId = 1, Date = new DateTime(2021, 04, 22, 12, 50, 00), Headline = "Gode idéer", Text = "Nu skal i bare høre!" },
                new Tip { TipId = 2, UserId = 3, CourseId = 1, Date = new DateTime(2021, 04, 22, 15, 12, 00), Headline = "Billigste bøger?", Text = "Læs overskrift?" },
                new Tip { TipId = 3, UserId = 2, CourseId = 1, Date = new DateTime(2021, 04, 26, 05, 08, 00), Headline = "Mcdonalds-snak", Text = "Fish-O-Filet er det bedste" },
                new Tip { TipId = 4, UserId = 4, CourseId = 1, Date = new DateTime(2021, 04, 18, 19, 36, 00), Headline = "Tips til Zombs", Text = "jeg er dårlig, søger et team, træning hver onsdag kl. 20" },
                new Tip { TipId = 5, UserId = 2, CourseId = 2, Date = new DateTime(2021, 04, 23, 08, 03, 00), Headline = "Ladeskabsopgaven", Text = "Forstår den ikk" },
                new Tip { TipId = 6, UserId = 1, CourseId = 3, Date = new DateTime(2021, 04, 24, 20, 28, 00), Headline = "Hjælp til DAB", Text = "Jeg har virkelig brug for hjælp til DAB Assignment 2" },
                new Tip { TipId = 7, UserId = 3, CourseId = 3, Date = new DateTime(2021, 04, 27, 10, 38, 00), Headline = "Assignment 3", Text = "hvad sker der" }
            );
        }
    }
}
