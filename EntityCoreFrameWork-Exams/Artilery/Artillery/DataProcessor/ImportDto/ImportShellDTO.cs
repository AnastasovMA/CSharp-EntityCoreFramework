using Artillery.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Shell")]
    public class ImportShellDTO
    {
        [XmlElement("ShellWeight")]
        [Required]
        [Range(GlobalConstants.SHELL_WEIGHT_MINLENGTH,
            GlobalConstants.SHELL_WEIGHT_MAXLENGTH)]
        public double ShellWeight { get; set; }

        [XmlElement("Caliber")]
        [Required]
        [MinLength(GlobalConstants.SHELL_CALIBER_MINLEGTH)]
        [MaxLength(GlobalConstants.SHELL_CALIBER_MAXLENGTH)]
        public string Caliber { get; set; }
    }
}
