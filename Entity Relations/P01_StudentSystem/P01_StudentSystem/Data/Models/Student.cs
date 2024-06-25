using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace P01_StudentSystem.Data.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        [Required]
        [Column(TypeName ="nvarchar(100)")]
        public string Name { get; set; } = null!;
        [Column(TypeName = "char(10)")]
        public string? PhoneNumber { get; set; }
        [Required]
        public DateTime RegisteredOn { get; set; }

        public DateTime? Birthday { get; set; }

        public List<StudentCourse> StudentsCourses { get; set; }=new List<StudentCourse>();

        public List<Homework>Homeworks { get; set; }=new List<Homework>();
    }
}
