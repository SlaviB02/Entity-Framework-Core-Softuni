using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType("Creator")]
    public class CreatorExport
    {
        [XmlElement("CreatorName")]
        public string CreatorName { get; set; } = null!;
        [XmlArray("Boardgames")]
        public BoardGameExport[] Boardgames { get; set; }=null!;

        [XmlAttribute("BoardgamesCount")]

        public int BoardgamesCount {  get; set; }
    }
}
