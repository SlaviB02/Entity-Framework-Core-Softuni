﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ExportDto
{
    [XmlType("Actor")]
    public class ActorExport
    {
        [XmlAttribute("FullName")]
        public string FullName { get; set; } = null!;

        [XmlAttribute("MainCharacter")]
        public string MainCharacter { get; set; }=null!;

        
    }
}
