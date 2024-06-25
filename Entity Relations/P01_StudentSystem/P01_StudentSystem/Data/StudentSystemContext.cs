using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext:DbContext
    {
        public StudentSystemContext()
        {
            
        }
        public StudentSystemContext(DbContextOptions options)
            : base(options) 
        {
        
            
        }

       public DbSet<Student> Students { get; set; } = null!;

       public DbSet<Course> Courses { get; set; } = null!;
       public DbSet<Resource> Resources { get; set; } = null!;

       public DbSet<Homework> Homeworks { get; set; } = null!;

        public DbSet<StudentCourse> StudentsCourses { get; set; }=null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>()
                .HasKey(e => new { e.StudentId, e.CourseId });







            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(optionsBuilder.IsConfigured==false)
            {
              
                optionsBuilder.UseSqlServer("Server=.;Database=StudentSystem;Integrated Security=True;");
            }
        }

        
    }
}
