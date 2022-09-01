using System;
using System.Collections.Generic;
using System.Text;

namespace Trucks.DataProcessor.ExportDto
{
    public class ExportClientDTO
    {
        public string Name { get; set; }

        public virtual ExportClientTrucksDTO[] Trucks { get; set; }
    }
}
