using System;
using System.Collections.Generic;
using System.Text;

namespace Comics.Domain.CrossRefModel
{
    public class Like
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public BaseItem Item { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
