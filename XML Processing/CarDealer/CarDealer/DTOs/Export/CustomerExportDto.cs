using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export
{
    [XmlType("customer")]
    public class CustomerExportDto
    {
        [XmlAttribute("full-name")]
        
        public string Name { get; set; } = null!;
        [XmlAttribute("bought-cars")]
        
        public string BoughtCars { get; set; }=null!;
        [XmlAttribute("spent-money")]
        
        public decimal SpentMoney {  get; set; }

    }
}
