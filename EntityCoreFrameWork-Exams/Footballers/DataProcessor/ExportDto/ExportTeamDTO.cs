using System;
using System.Collections.Generic;
using System.Text;

namespace Footballers.DataProcessor.ExportDto
{
    public class ExportTeamDTO
    {
        public string Name { get; set; }

        public ICollection<ExportFootballerDTO> Footballers { get; set; }
    }
}
