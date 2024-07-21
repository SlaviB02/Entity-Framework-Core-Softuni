using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Despatcher")]
    public class DespatcherDtoImport
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; } = null!;
        [XmlElement("Position")]
        public string? Position { get; set; }

        [XmlArray("Trucks")]
        public TruckDtoImport[] Trucks { get; set; } = null!;
    }
}
