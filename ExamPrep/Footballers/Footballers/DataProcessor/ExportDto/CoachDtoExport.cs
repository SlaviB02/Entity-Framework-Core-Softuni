using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType("Coach")]
    public class CoachDtoExport
    {
        [XmlElement("CoachName")]
        public string CoachName { get; set; } = null!;
        [XmlAttribute("FootballersCount")]
        public int FootballersCount {  get; set; }
        [XmlArray("Footballers")]
        public FootballerDtoExport[] Footballers { get; set; } = null!;
    }
}
