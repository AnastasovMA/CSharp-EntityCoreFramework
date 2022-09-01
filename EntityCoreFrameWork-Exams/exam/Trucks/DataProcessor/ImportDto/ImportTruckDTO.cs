using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Truck")]
    public class ImportTruckDTO
    {
        [XmlElement("RegistrationNumber")]
        [MinLength(8)]
        [MaxLength(8)]
        [RegularExpression(@"^[A-Z]{2}[0-9]{4}[A-Z]{2}")]
        public string RegistrationNumber { get; set; }

        [XmlElement("VinNumber")]
        [Required]
        [MinLength(17)]
        [MaxLength(17)]
        public string VinNumber { get; set; }

        [Required]
        [Range(950,1420)]
        [XmlElement("TankCapacity")]
        public int TankCapacity { get; set; }

        [Required]
        [Range(5000,29000)]
        [XmlElement("CargoCapacity")]
        public int CargoCapacity { get; set; }

        [Range(0,3)]
        [Required]
        [XmlElement("CategoryType")]
        public int CategoryType { get; set; }

        [Required]
        [Range(0,4)]
        [XmlElement("MakeType")]
        public int MakeType { get; set; }
    }
}
