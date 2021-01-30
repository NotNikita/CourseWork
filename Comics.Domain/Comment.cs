using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Comics.Domain
{
    public class Comment
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        
        [Column("BaseItemId")]
        public BaseItem Item { get; set; }
        public DateTime CreationDate { get; set; }
        public string Text { get; set; }
        public User Author { get; set; }
    }
}
