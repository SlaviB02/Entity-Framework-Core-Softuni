using P02_FootballBetting.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Bet
    {
        [Key]
        public int BetId { get; set; }
        [Required]
        [Column(TypeName = "DECIMAL (18, 2)")]
        public decimal Amount { get; set; }

        public Prediction Prediction { get; set;} 
        [Required]
        public DateTime DateTime { get; set; }

        public int UserId {  get; set; }

        [Required]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        public int GameId {  get; set; }
        [ForeignKey(nameof(GameId))]
        public Game Game { get; set; } = null!;

    }
}
