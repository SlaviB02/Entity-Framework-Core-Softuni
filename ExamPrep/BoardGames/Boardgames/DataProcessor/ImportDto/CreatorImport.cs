using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Creator")]
    public class CreatorImport
    {
        [XmlElement("FirstName")]
        [MinLength(2)]
        [MaxLength(7)]
        [Required]
        public string FirstName { get; set; } = null!;
        [XmlElement("LastName")]
        [MinLength(2)]
        [MaxLength(7)]
        [Required]
        public string LastName { get; set; }=null!;
        [XmlArray("Boardgames")]
        public BoardgameImport[] Boardgames { get; set; } = null!;
    }
}
