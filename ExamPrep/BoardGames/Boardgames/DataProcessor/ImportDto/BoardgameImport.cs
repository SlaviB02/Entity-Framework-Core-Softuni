using Boardgames.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Boardgame")]
    public class BoardgameImport
    {
        [XmlElement("Name")]
        [MinLength(10)]
        [MaxLength(20)]
        [Required]
        public string Name { get; set; } = null!;
        [XmlElement("Rating")]
        [Range(1,10.00)]
        [Required]
        public double Rating {  get; set; }
        [XmlElement("YearPublished")]
        [Range(2018,2023)]
        [Required]
        public int YearPublished { get; set; }
        [XmlElement("CategoryType")]
        [EnumDataType(typeof(CategoryType))]
        [Required]
        public string CategoryType { get; set; } = null!;
        [XmlElement("Mechanics")]
        [Required]
        public string Mechanics { get; set; } = null!;
    }
}
