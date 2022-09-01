using System;
using System.Collections.Generic;
using System.Text;

namespace Artillery.DataProcessor.ExportDto
{
    public class ExportShellDTO
    {
        public double ShellWeight { get; set; }

        public string Caliber { get; set; }

        public virtual ExportShellGunDTO[] Guns { get; set; }
    }
}
