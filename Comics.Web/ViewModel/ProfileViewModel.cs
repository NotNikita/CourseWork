using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comics.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Comics.Web.ViewModel
{
    public class ProfileViewModel
    {
        public User user { get; set; }
        public bool isMe { get; set; }
    }
}
