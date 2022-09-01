using Artillery.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Country")]
    public class ImportCountryDTO
    {
        [XmlElement("CountryName")]
        [Required]
        [MinLength(GlobalConstants.COUNTRY_MIN_LENGTH)]
        [MaxLength(GlobalConstants.COUNTRY_MAX_LENGTH)]
        public string CountryName { get; set; }

        [XmlElement("ArmySize")]
        [Range(GlobalConstants.COUNTRY_ARMYSIZE_MINLENGTH,
            GlobalConstants.COUNTRY_ARMYSIZE_MAXLENGTH)]
        public int ArmySize { get; set; }
    }
}
