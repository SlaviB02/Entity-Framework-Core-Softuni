using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class PlayerStatistic
    {
        public int GameId { get; set; }

        [ForeignKey(nameof(GameId))]    
        public Game Game { get; set; } = null!;

        public int PlayerId { get; set; }
        [ForeignKey(nameof(PlayerId))]
        public Player Player { get; set; } = null!;
        [Required]
        public byte ScoredGoals {  get; set; }
        [Required]
        public byte Assists {  get; set; }
        [Required]
        [Column(TypeName = "DECIMAL (18, 2)")]
        public byte MinutesPlayed {  get; set; }

    }
}
