using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Export
{
    [XmlType("sale")]
    public class SalesExportDto
    {
        [XmlElement("car")]
        public CarExportModel Car { get; set; }
        [XmlElement("discount")]
        public string Discount { get; set; } = null!;

        [XmlElement("customer-name")]

        public string CustomerName { get; set; } = null!;
        [XmlElement("price")]
        public string Price { get; set; }=null!;

        [XmlElement("price-with-discount")]
        public string PriceWithDiscount { get; set; } = null!;
    }
}
