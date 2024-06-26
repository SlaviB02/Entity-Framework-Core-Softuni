﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P03_SalesDatabase.Data.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId {  get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = null!;
        [Required]
        [Column(TypeName = "varchar(80)")]
        public string Email { get; set; } = null!;
        [Required]
        public string CreditCardNumber { get; set; }=null!;
        public List<Sale> Sales { get; set; } = new List<Sale>();
    }
}
