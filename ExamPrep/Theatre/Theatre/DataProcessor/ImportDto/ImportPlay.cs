using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Theatre.Data.Models.Enums;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Play")]
    public class ImportPlay
    {
        [Required]
        [XmlElement("Title")]
        [MinLength(4)]
        [MaxLength(50)]
        public string Title { get; set; } = null!;

        [Required]
        [XmlElement("Duration")]
        public string Duration { get; set; } = null!;

        [Required]
        [XmlElement("Raiting")]
        [Range(0.00,10.00)]
        public decimal Rating { get; set; }
        [Required]
        [XmlElement("Genre")]
        [EnumDataType(typeof(Genre))]
        public string Genre { get; set; } = null!;

        [Required]
        [XmlElement("Description")]
        [MaxLength(700)]
        public string Description { get; set; } = null!;


        [Required]
        [XmlElement("Screenwriter")]
        [MinLength(4)]
        [MaxLength(30)]
        public string Screenwriter {  get; set; } = null!;

    }
}
