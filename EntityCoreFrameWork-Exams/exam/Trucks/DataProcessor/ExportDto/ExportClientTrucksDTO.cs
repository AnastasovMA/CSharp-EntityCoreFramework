﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Trucks.DataProcessor.ExportDto
{
    public class ExportClientTrucksDTO
    {
        public string TruckRegistrationNumber { get; set; }

        public string VinNumber { get; set; }

        public int TankCapacity { get; set; }

        public int CargoCapacity { get; set; }

        public string CategoryType { get; set; }

        public string MakeType { get; set; }
    }
}
