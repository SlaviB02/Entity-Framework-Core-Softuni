﻿namespace ProductShop.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User
    {
        public User()
        {
            this.ProductsSold = new List<Product>();
            this.ProductsBought = new List<Product>();
        }

        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public int? Age { get; set; }
        [InverseProperty("Seller")]
        public ICollection<Product> ProductsSold { get; set; } = null!;
        [InverseProperty("Buyer")]
        public ICollection<Product> ProductsBought { get; set; } = null!;
    }
}