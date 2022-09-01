using Artillery.Common;
using Artillery.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Artillery.DataProcessor.ImportDto
{
    public class ImportGunsDTO
    {
        public ImportGunsDTO()
        {
            this.Countries = new HashSet<ImportCountryIdDTO>();
        }

        public int ManufacturerId { get; set; }

        [Range(GlobalConstants.GUN_GUNWEIGHT_MINLENGTH,
            GlobalConstants.GUN_GUNWEIGHT_MAXLENGTH)]
        public int GunWeight { get; set; }

        [Range(GlobalConstants.GUN_BARRELLENGTH_MINLENGTH,
            GlobalConstants.GUN_BARRELLENGTH_MAXLENGTH)]
        public double BarrelLength { get; set; }

        public int? NumberBuild { get; set; }

        [Range(GlobalConstants.GUN_RANGE_MINLENGTH,
            GlobalConstants.GUN_RANGE_MAXLENGTH)]
        public int Range { get; set; }

        [Required]
        public string GunType { get; set; }

        public int ShellId { get; set; }

        public virtual ICollection<ImportCountryIdDTO> Countries { get; set; }
    }
}
