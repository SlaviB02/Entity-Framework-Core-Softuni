using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Country
    {
        [Key] 
        public int CountryId { get; set; }
        [Required]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        public List<Town> Towns { get; set; }= new List<Town>();

    }
}
