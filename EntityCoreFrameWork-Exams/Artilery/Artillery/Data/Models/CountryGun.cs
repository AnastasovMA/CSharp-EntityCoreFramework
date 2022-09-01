using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Artillery.Data.Models
{
    public class CountryGun
    {
        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }

        [ForeignKey(nameof(Gun))]
        public int GunId { get; set; }

        #region Navigation Properties
        public virtual Gun Gun { get; set; }

        public virtual Country Country { get; set; }
        #endregion
    }
}
