using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Game
    {
        [Key]
        public int GameId { get; set; }

        public int HomeTeamId {  get; set; }
        [ForeignKey(nameof(HomeTeamId))]
        public Team HomeTeam { get; set; } = null!;
     
        public int AwayTeamId {  get; set; }
        [ForeignKey(nameof(AwayTeamId))]
        public Team AwayTeam { get; set; }=null!;
        [Required]
        public int HomeTeamGoals {  get; set; }
        [Required]
        public int AwayTeamGoals { get;set; }
        [Required]
        [Column(TypeName = "DECIMAL (18, 2)")]
        public decimal HomeTeamBetRate {  get; set; }
        [Required]
        [Column(TypeName = "DECIMAL (18, 2)")]
        public decimal AwayTeamBetRate { get;set; }
        [Required]
        [Column(TypeName = "DECIMAL (18, 2)")]
        public decimal DrawBetRate { get; set; }
        [Required]
        public DateTime DateTime {  get; set; }
        [Required]
        [MaxLength(10)]
        public string Result { get; set; } = null!;

        public List<PlayerStatistic> PlayersStatistics { get; set; }=new List<PlayerStatistic>();

        public List<Bet>Bets { get; set; }=new List<Bet>();

    }
}
