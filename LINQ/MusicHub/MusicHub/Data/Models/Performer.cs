using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHub.Data.Models
{
    public class Performer
    {

        public Performer()
        {
            PerformerSongs=new HashSet<SongPerformer>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }
        [Required]
        public int Age {  get; set; }

        [Required]
        [Column(TypeName ="Decimal(18,2)")]
        public decimal NetWorth {  get; set; }

        public ICollection<SongPerformer> PerformerSongs { get; set; }  
    }
}
