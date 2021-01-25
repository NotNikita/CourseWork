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
using Comics.Web.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Comics.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ComicsDbContext _comicsDbContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ICollectionRepository _collectionRepository;
        private readonly IUserRepository _userRepository;

        public UsersController(ComicsDbContext comicsDbContext, UserManager<User> userManager, SignInManager<User> signInManager, ICollectionRepository collectionRepository, IUserRepository userRepository)
        {
            _comicsDbContext = comicsDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _collectionRepository = collectionRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("admin"))
            {
                return RedirectToAction("Index", "Collection");
            }

            return View(_userManager.Users.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            User currentUser = await _userManager.GetUserAsync(HttpContext.User);
            return RedirectToAction("Profile", "Account", new { id = currentUser.Id });
        }

        [HttpGet]
        public async Task<IActionResult> UserProfile(string id)
        {
            User currentUser = await _userManager.FindByIdAsync(id);
            return RedirectToAction("Profile", "Account", new { id = currentUser.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        private bool UserExist(string id)
        {
            return _comicsDbContext.Users.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Block(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _comicsDbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.LockoutEnd > DateTime.Today)
            {
                user.LockoutEnd = null;//DateTime.Today;
                user.LockoutEnabled = false;

            }
            else
            {
                user.LockoutEnd = DateTime.Today.AddYears(1);
                user.LockoutEnabled = true;
                await _userManager.UpdateSecurityStampAsync(user);
            }

            try
            {
                _comicsDbContext.Update(user);
                await _comicsDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExist(user.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            if (user.Email == User.Identity.Name && user.LockoutEnabled == true)
            {
                await Logout();
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Block(string id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _comicsDbContext.Update(user);
                    await _comicsDbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExist(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            User currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains("admin"))
            {
                return RedirectToAction("Index", "Collection");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var colls = _collectionRepository.GetUserCollections(user);
                _comicsDbContext.Collections.RemoveRange(_comicsDbContext.Collections.Where(l => l.User.Id == id));
                _comicsDbContext.Users.Remove(user);
                await _comicsDbContext.SaveChangesAsync();
            }

            if (user.Email == User.Identity.Name && user.LockoutEnabled == true)
            {
                await Logout();
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _comicsDbContext.Users.FindAsync(id);
            _comicsDbContext.Users.Remove(user);
            await _comicsDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
