using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P03_SalesDatabase.Data.Models
{
    public class Store
    {
        [Key]
        public int StoreId { get; set; }


        [Required]
        [Column(TypeName = "nvarchar(80)")]
        public string Name { get; set; } = null!;

        public List<Sale> Sales { get; set; }=new List<Sale>();
    }
}
