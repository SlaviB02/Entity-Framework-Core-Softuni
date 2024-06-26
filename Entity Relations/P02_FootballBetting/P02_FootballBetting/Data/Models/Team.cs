using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; } = null!;
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string LogoUrl { get; set; }= null!;
        [Required]
        [Column(TypeName = "varchar(5)")]
        public string Initials {  get; set; }=null!;
        [Required]
        [Column(TypeName = "DECIMAL (18, 2)")]
        public decimal Budget { get; set; }

        public int PrimaryKitColorId {  get; set; }
        [ForeignKey(nameof(PrimaryKitColorId))]
        public Color PrimaryKitColor { get; set; }=null!;

        public int SecondaryKitColorId { get;set; }
        [ForeignKey(nameof(SecondaryKitColorId))]
        public Color SecondaryKitColor { get; set; }=null!;

        public int TownId { get; set; }

        [ForeignKey(nameof(TownId))]
        public Town Town { get; set; } = null!;

        [InverseProperty("HomeTeam")]
        public List<Game> HomeGames {  get; set; }=new List<Game>();
        [InverseProperty("AwayTeam")]
        
        public List<Game> AwayGames { get;set;}=new List<Game>();
        
        public List<Player>Players { get; set; }=new List<Player>();

    }
}
