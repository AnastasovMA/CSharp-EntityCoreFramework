﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Trucks.DataProcessor.ImportDto
{
    public class ImportClientDTO
    {
        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Nationality { get; set; }

        public string Type { get; set; }

        public int[] Trucks { get; set; }
    }
}
