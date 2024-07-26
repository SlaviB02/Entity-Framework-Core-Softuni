using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Theatre.Data.Models.Enums;

namespace Theatre.Data.Models
{
    public class Play
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; } = null!;

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        public float Rating {  get; set; }

        [Required]
        public Genre Genre { get; set; }
        [Required]
        [StringLength(700)]
        public string Description { get; set; } = null!;

        [Required]
        [StringLength(30)]
        public string Screenwriter {  get; set; } = null!;

        public ICollection<Cast> Casts { get; set; } = new HashSet<Cast>();

        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
