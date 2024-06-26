using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = null!;
        [Required]
        [MaxLength(50)]
        public string Name {  get; set; }=null!;
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }=null!;
        [Required]
        [MaxLength(50)]
        public string Email { get; set; } = null!;
        [Required]
        [Column(TypeName = "DECIMAL (18, 2)")]
        public decimal Balance {  get; set; }

        public List<Bet> Bets { get; set; }=new List<Bet>();

    }
}
