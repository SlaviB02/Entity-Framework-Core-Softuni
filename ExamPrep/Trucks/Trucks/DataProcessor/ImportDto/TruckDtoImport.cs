using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Trucks.Data.Models.Enums;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Truck")]
    public class TruckDtoImport
    {
        [XmlElement("RegistrationNumber")]
        [StringLength(8)]
        [RegularExpression("[A-Z]{2}\\d{4}[A-Z]{2}")]
        [Required]
        public string RegistrationNumber { get; set; } = null!;

        [XmlElement("VinNumber")]
        [StringLength(17)]
        [Required]
        public string VinNumber { get; set; } = null!;

        [XmlElement("TankCapacity")]
        [Range(950, 1420)]
        public int TankCapacity { get; set; }

        [XmlElement("CargoCapacity")]
        [Range(5000, 29000)]

        public int CargoCapacity {  get; set; }
        [XmlElement("CategoryType")]
        [EnumDataType(typeof(CategoryType))]
        [Required]
        public string CategoryType { get; set; } = null!;
        [XmlElement("MakeType")]
        [EnumDataType(typeof(MakeType))]
        [Required]
        public string MakeType {  get; set; } = null!;
    }
}
