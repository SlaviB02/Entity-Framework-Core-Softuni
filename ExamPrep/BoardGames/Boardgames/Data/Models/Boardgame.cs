using Boardgames.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.Data.Models
{
    public class Boardgame
    {

        public Boardgame()
        {
            BoardgamesSellers = new List<BoardgameSeller>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; } = null!;

        [Required]
        [Range(1,10.00)]
        public double Rating {  get; set; }

        [Required]
        [Range(2018,2023)]
        public int YearPublished {  get; set; }

        [Required]
        public CategoryType CategoryType { get; set; }
        [Required]
        public string Mechanics {  get; set; }=null!;

        [Required]
        public int CreatorId {  get; set; }
        [ForeignKey(nameof(CreatorId))]
        public Creator Creator { get; set; } = null!;

        public ICollection<BoardgameSeller> BoardgamesSellers {  get; set; }
    }
}
