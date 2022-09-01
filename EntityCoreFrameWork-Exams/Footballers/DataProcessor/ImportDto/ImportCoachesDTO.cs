using Footballers.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Coach")]
    public class ImportCoachesDTO
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(GlobalConstants.COACH_NAME_MINLENGTH)]
        [MaxLength(GlobalConstants.COACH_NAME_MAXLENGTH)]
        public string Name { get; set; }

        [XmlElement("Nationality")]
        [Required]
        public string Nationality { get; set; }

        [XmlArray("Footballers")]
        public ImportCoachFootballersDTO[] Footballers { get; set; }
    }
}
