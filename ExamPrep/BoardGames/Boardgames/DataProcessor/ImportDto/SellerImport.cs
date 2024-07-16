using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.DataProcessor.ImportDto
{
    public class SellerImport
    {
        [MinLength(5)]
        [MaxLength(20)]
        [Required]
        public string Name { get; set; } = null!;
        [MinLength(2)]
        [MaxLength(30)]
        [Required]
        public string Address {  get; set; } = null!;
        [Required]
        public string Country {  get; set; } = null!;
        [RegularExpression("www.[A-Za-z0-9-]+.com")]
        [Required]
        public string Website {  get; set; } = null!;

        public int[] Boardgames {  get; set; }=null!;
    }
}
