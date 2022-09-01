using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType("Coach")]
    public class ExportCoachDTO
    {
        [XmlElement("CoachName")]
        public string Name { get; set; }

        [XmlAttribute("FootballersCount")]
        public int FootballersCount { get; set; }

        [XmlArray("Footballers")]
        public ExportCoachFootballerDTO[] Footballers { get; set; }
    }
}
