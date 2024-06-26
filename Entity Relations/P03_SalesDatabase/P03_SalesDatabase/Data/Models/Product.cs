using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P03_SalesDatabase.Data.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; } = null!;
        [Required]
        [Column(TypeName = "DECIMAL (18, 2)")]
        public decimal Quantity { get; set; }
        [Required]
        [Column(TypeName = "DECIMAL (18, 2)")]
        public decimal Price { get; set; }
        public List<Sale> Sales { get; set; } = new List<Sale>();
    }
}
