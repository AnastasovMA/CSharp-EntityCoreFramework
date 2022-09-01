using Footballers.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Footballer")]
    public class ImportCoachFootballersDTO
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(GlobalConstants.FOOTBALLER_NAME_MINLENGTH)]
        [MaxLength(GlobalConstants.FOOTBALLER_NAME_MAXLENGTH)]
        public string Name { get; set; }

        [XmlElement("ContractStartDate")]
        [Required]
        public string ContractStartDate { get; set; }

        [XmlElement("ContractEndDate")]
        [Required]
        public string ContractEndDate { get; set; }

        [XmlElement("BestSkillType")]
        [Required]
        [Range(GlobalConstants.BEST_SKILL_TYPE_MINLENGTH,
            GlobalConstants.BEST_SKILL_TYPE_MAXLENGTH)]
        public int BestSkillType { get; set; }

        [XmlElement("PositionType")]
        [Required]
        [Range(GlobalConstants.POSITION_TYPE_MINLENGTH,
            GlobalConstants.POSITION_TYPE_MAXLENGTH)]
        public int PositionType { get; set; }
    }
}
