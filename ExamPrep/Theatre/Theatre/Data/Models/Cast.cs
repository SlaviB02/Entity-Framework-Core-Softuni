using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theatre.Data.Models
{
    public class Cast
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string FullName { get; set; } = null!;

        [Required]
        public bool IsMainCharacter {  get; set; }

        [Required]
        [StringLength(30)]  
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public int PlayId {  get; set; }

        [ForeignKey(nameof(PlayId))]
        public Play Play { get; set; } = null!;
    }
}
