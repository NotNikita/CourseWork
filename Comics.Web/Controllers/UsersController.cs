using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comics.DAL;
using Comics.Domain;
using Comics.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Comics.Web.Controllers
{
    public class UsersController : Controller
    {

        UserManager<User> _userManager;


        public UsersController(IUserRepository _rep, UserManager<User> userManager, ComicsDbContext contex)
        {
            _userManager = userManager;
        }

        [HttpGet]

        public async Task<IActionResult> Profile()
        {
            User currentUser = await _userManager.GetUserAsync(HttpContext.User);
            return RedirectToAction("Profile", "Account", new { id = currentUser.Id });

        }

        public IActionResult Index()
        {

            return View(_userManager.Users.ToList());
        }


    }
}
