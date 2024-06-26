using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        public int SquadNumber { get; set; }
        
        public bool IsInjured {  get; set; }

        public int PositionId { get; set; }

        [ForeignKey(nameof(PositionId))]
        public Position Position { get; set; } = null!;

        public int TeamId { get; set; }

        [ForeignKey(nameof(TeamId))]
        public Team Team { get; set; }=null!;

        public int TownId {  get; set; }
        [ForeignKey(nameof(TownId))]
        public Town Town { get; set; } = null!;

        public List<PlayerStatistic> PlayersStatistics { get; set; } = new List<PlayerStatistic>();

    }
}
