using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Color
    {
        [Key]
        public int ColorId { get; set; }
        [Required]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        [InverseProperty("PrimaryKitColor")]
        public List<Team> PrimaryKitTeams {  get; set; }=new List<Team>();
        [InverseProperty("SecondaryKitColor")]
        public List<Team> SecondaryKitTeams { get; set; } = new List<Team>();   
    }
}
