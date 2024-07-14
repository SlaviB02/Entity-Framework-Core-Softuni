using Cadastre.Data.Enumerations;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType("District")]
    public class DistrictDto
    {
        [XmlAttribute("Region")]
        [EnumDataType(typeof(Region))]
        public string Region { get; set; } = null!;
        [XmlElement("Name")]
        [MinLength(2)]
        [MaxLength(80)]
        public string Name { get; set; } = null!;
        [XmlElement("PostalCode")]
        [RegularExpression(@"[A-Z]{2}-[0-9]{5}")]
        public string PostalCode { get; set; }=null!;

        [XmlArray("Properties")]

        public PropertyDto[] Properties { get; set; }
    }
}
