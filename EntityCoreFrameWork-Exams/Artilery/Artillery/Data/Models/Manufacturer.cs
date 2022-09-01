using Artillery.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Artillery.Data.Models
{
    public class Manufacturer
    {
        public Manufacturer()
        {
            this.Guns = new HashSet<Gun>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.MANUFACTURER_NAME_MAX_LENGTH)]
        public string ManufacturerName { get; set; }

        [Required]
        [MaxLength(GlobalConstants.MANUFACTURER_FOUNDED_MAX_LENGTH)]
        public string Founded { get; set; }

        public virtual ICollection<Gun> Guns { get; set; }
    }
}
