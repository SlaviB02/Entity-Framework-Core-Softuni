using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Import
{
    [XmlType("Car")]
    public class CarPartDto
    {
        [XmlElement("make")]
        public string Make { get; set; } = null!;

        [XmlElement("model")]
        public string Model { get; set; }= null!;

        [XmlElement("traveledDistance")]
        public int TraveledDistance {  get; set; }
        [XmlArray("parts")]
        public PartsIdDto[] Parts { get; set; } 
    }
}
