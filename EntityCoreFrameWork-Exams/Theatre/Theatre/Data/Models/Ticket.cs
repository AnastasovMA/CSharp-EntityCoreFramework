using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Theatre.Data.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public sbyte RowNumber { get; set; }

        [ForeignKey(nameof(Play))]
        public int PlayId { get; set; }

        [ForeignKey(nameof(Theatre))]

        public int TheatreId { get; set; }

        #region Navigation Property

        public virtual Play Play { get; set; }

        public virtual Theatre Theatre { get; set; }

        #endregion
    }
}
