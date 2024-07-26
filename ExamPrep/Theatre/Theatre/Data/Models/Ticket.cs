using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theatre.Data.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal Price { get; set; }
        [Required]
        public sbyte RowNumber {  get; set; }


        [Required]
        public int PlayId { get; set; }

        [ForeignKey(nameof(PlayId))]
        public Play Play { get; set; } = null!;

        [Required]
        public int TheatreId {  get; set; }

        [ForeignKey(nameof(TheatreId))]

        public Theatre Theatre { get; set; } = null!;

    }
}
