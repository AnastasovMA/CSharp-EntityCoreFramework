using Footballers.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Footballers.DataProcessor.ImportDto
{
    public class ImportTeamDTO
    {
        [Required]
        [MinLength(GlobalConstants.TEAM_NAME_MINLEGTH)]
        [MaxLength(GlobalConstants.TEAM_NAME_MAXLENGTH)]
        [RegularExpression(GlobalConstants.TEAM_NAME_REGEX)]
        public string Name { get; set; }

        [Required]
        [MinLength(GlobalConstants.TEAM_NATIONALITY_MINLEGTH)]
        [MaxLength(GlobalConstants.TEAM_NATIONALITY_MAXLENGTH)]
        public string Nationality { get; set; }

        [Required]
        public int Trophies { get; set; }

        public int[] Footballers { get; set; }
    }
}
