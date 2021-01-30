using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Comics.Domain.CrossRefModel;

namespace Comics.Domain
{
    public class BaseItem
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        //[Required]
        public string Tags { get; set; }

        public bool isInWishList { get; set; } = false;

        public IEnumerable<Comment> Comments { get; set; }
        public IList<Like> Likes { get; set; } = new List<Like>();

        [Column("UserId")]
        public string UserId { get; set; }

        public User User { get; set; }

        [Url]
        [Column("Image")]
        public string Img { get; set; }

        [Column("Price")]
        public int Price { get; set; }

        [Column("Created")]
        public DateTime Created { get; set; }

        [Column("CollectionId")]
        public int CollectionId { get; set; }
    }
}
