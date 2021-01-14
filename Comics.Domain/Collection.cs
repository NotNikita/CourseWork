using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Comics.Domain
{
    public class Collection
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Column("Description")]
        public string Description { get; set; }
        public string Theme { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<BaseItem> Items { get; set; }

        [Column("UserId")]
        public User User { get; set; }

        [Url]
        [Column("Image")]
        public string Img { get; set; }
    }
}
