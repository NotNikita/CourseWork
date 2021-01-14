using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Comics.Domain
{
    public class Whisky : BaseItem
    {
        [Column("Brand")]
        public string? Brand { get; set; }

        [Column("Vintage")]
        public DateTime? Vintage { get; set; }

        [Column("Bottled")]
        public DateTime? Bottled { get; set; }

        [Column("Strength")]
        public int? Strength { get; set; } //"% Vol."

        [Column("Size")]
        public int? Size { get; set; } // 700 ml
    }
}
