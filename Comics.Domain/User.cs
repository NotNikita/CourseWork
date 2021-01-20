using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Comics.Domain
{
    public class User : IdentityUser
    {
        [Column("Registration")]
        public DateTime Registration { get; set; }
        public List<Collection> Collections { get; set; }
        public List<BaseItem> WishList { get; set; }

        [Url]
        [Column("Image")]
        public string? Img { get; set; }
    }
}
