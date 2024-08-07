﻿using Medicines.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType("Medicine")]
    public class MedicineImportDto
    {
        [XmlAttribute("category")]
        [EnumDataType(typeof(Category))]
        public string Category { get; set; } = null!;
        [XmlElement("Name")]
        [Required]
        [MinLength(3)]
        [MaxLength(150)]
        public string Name { get; set; } = null!;
        [XmlElement("Price")]
        [Range(0.01,1000.00)]
        public decimal Price { get; set; }
        [XmlElement("ProductionDate")]
        [Required]
        public string ProductionDate { get; set; } = null!;
        [XmlElement("ExpiryDate")]
        [Required]
        public string ExpiryDate {  get; set; }=null!;
        [XmlElement("Producer")]
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Producer {  get; set; } = null!;
    }
}
