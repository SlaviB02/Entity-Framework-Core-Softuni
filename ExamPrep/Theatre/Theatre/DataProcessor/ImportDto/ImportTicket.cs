using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theatre.DataProcessor.ImportDto
{
    public class ImportTicket
    {
        [Required]
        [Range(1.00,100.00)]
        public decimal Price { get; set; }
        [Required]
        [Range(1,10)]
        public sbyte RowNumber {  get; set; }
        [Required]
        public int PlayId {  get; set; }
    }
}
