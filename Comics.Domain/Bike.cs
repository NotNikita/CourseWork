using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Comics.Domain
{
    public class Bike : BaseItem
    {
        [Column("Producer")]
        public string? Producer { get; set; }

        [Column("Color")]
        public string? Color { get; set; }

        [Column("Year")]
        public int? Year { get; set; }

        [Column("Type")]
        public string? BikeType { get; set; }

        [Column("Diameter")]
        public int? WheelDiameter { get; set; }
    }
}
