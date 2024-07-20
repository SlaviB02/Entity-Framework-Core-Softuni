using Invoices.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.DataProcessor.ImportDto
{
    public class ImportInvoicesDto
    {
        [Required]
        [Range(1000000000,1500000000)]
        public int Number { get; set; }
        [Required]
        public string IssueDate { get; set; }=null!;
        [Required]
        public string DueDate {  get; set; }=null!;
        [Required]
        public decimal Amount { get; set; }
        [Required]
        [EnumDataType(typeof(CurrencyType))]
        public string CurrencyType { get; set; } = null!;
        [Required]
        public int ClientId {  get; set; }
    }
}
