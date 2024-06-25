using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Models
{
    public class Homework
    {
        [Key]
        public int HomeworkId { get; set; }
        [Required]
        [Column(TypeName = "varchar")]
        public string Content { get; set; } = null!;

       public Data.Enumerations.ContentType ContentType { get; set; }
        [Required]
        public DateTime SubmissionTime { get; set; }

        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }=null!;
        
        public int CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }=null !;

    }
}
