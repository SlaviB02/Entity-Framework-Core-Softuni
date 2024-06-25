using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(80)")]
        public string Name { get; set; } = null!;
        [Column(TypeName = "nvarchar")]
        public string?  Description { get; set; }
        [Required]
        public  DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        [Column(TypeName= "DECIMAL (18, 2)")]
        public decimal Price {  get; set; }

       public List<StudentCourse> StudentsCourses { get; set; }=new List<StudentCourse>();

       public List<Resource> Resources { get; set; }=new List<Resource>();

       public List<Homework> Homeworks { get; set; }=new List<Homework>();
    }
}
