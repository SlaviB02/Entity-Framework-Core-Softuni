﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Coach")]
    public class CoachDtoImport
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; } = null!;
        [XmlElement("Nationality")]
        [Required]
        public string Nationality { get; set; } = null!;

        [XmlArray("Footballers")]
        public FootballerDtoImport[]Footballers { get; set; }=null!;
    }
}
