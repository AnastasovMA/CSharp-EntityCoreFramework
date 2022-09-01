using Footballers.Common;
using Footballers.Data;
using Footballers.Data.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Footballers.Data.Models
{
    public class Footballer
    {
        public Footballer()
        {
            this.TeamsFootballers = new HashSet<TeamFootballer>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        //TODO MINLENGTH
        [MaxLength(GlobalConstants.FOOTBALLER_NAME_MAXLENGTH)]
        public string Name { get; set; }

        public DateTime ContractStartDate { get; set; } //REQUIRED BY DEFAULT

        public DateTime ContractEndDate { get; set; }
 
        public PositionType PositionType { get; set; }

        public BestSkillType BestSkillType { get; set; }

        //[Required]
        [ForeignKey(nameof(Coach))]
        public int CoachId { get; set; }

        public virtual Coach Coach { get; set; }

        public ICollection<TeamFootballer> TeamsFootballers { get; set; }
    }
}
