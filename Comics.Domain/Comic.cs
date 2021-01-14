using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Comics.Domain
{
    public class Comic : BaseItem
    {
        [Column("Publisher")]
        public string? Publisher { get; set; } //ushort

        [Column("Description")]
        public string? Description { get; set; }

        [Column("Pages")]
        public int? PageCount { get; set; }

        [Column("Format")]
        public string? Format { get; set; }

        [Column("Released")]
        public DateTime? Released { get; set; }
    }
}
