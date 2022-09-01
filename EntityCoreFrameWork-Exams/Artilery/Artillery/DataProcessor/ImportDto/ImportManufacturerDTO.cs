using Artillery.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Manufacturer")]
    public class ImportManufacturerDTO
    {
        [XmlElement("ManufacturerName")]
        [Required]
        [MinLength(GlobalConstants.MANUFACURER_NAME_MINLENGTH)]
        [MaxLength(GlobalConstants.MANUFACTURER_NAME_MAX_LENGTH)]
        public string ManufacturerName { get; set; }

        [XmlElement("Founded")]
        [Required]
        [MinLength(GlobalConstants.MANUFACTURER_FOUNDED_MIN_LENGTH)]
        [MaxLength(GlobalConstants.MANUFACTURER_FOUNDED_MAX_LENGTH)]
        public string Founded { get; set; }
            
    }
}
