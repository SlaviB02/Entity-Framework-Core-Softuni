using Footballers.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Footballer")]
    public class FootballerDtoImport
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; } = null!;
        [XmlElement("ContractStartDate")]
        [Required]
        public string ContractStartDate { get; set; } = null!;
        [XmlElement("ContractEndDate")]
        [Required]
        public string ContractEndDate { get; set; } = null!;
        [XmlElement("BestSkillType")]
        [Required]
        [EnumDataType(typeof(BestSkillType))]
        public string BestSkillType { get; set; } = null!;
        [XmlElement("PositionType")]
        [Required]
        [EnumDataType(typeof(PositionType))]
        public string PositionType { get; set; } = null!;
    }
}
