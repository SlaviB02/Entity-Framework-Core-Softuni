using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType("Property")]
    public class PropertyDto
    {
        [XmlElement("PropertyIdentifier")]
        [MinLength(16)]
        [MaxLength(20)]
        public string PropertyIdentifier { get; set; } = null!;
        [XmlElement("Area")]
        [Range(0, int.MaxValue)]
        public int Area { get; set; }
        [XmlElement("Details")]
        [MinLength(5)]
        [MaxLength(500)]
        public string Details { get; set; } = null!;
        [XmlElement("Address")]
        [MinLength(5)]
        [MaxLength(200)]
        public string Address { get; set; } = null!;
        [XmlElement("DateOfAcquisition")]
        public string DateOfAcquisition { get; set; }=null!;
    }
}
