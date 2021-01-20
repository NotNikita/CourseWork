using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Comics.DAL;
using Comics.Domain;
using Microsoft.AspNetCore.Hosting;
using Comics.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Comics.Services;
using Microsoft.AspNetCore.Http;
using Comics.Web.ViewModel;

namespace Comics.Web.Controllers
{
    public class CollectionController : Controller
    {
        private readonly ComicsDbContext _context;
        private UserManager<User> _userManager;
        private ComicsDbContext _db;
        private IWebHostEnvironment _appEnviroment;
        private ICollectionRepository _collectionRepository;
        private IUserRepository _userRepository;
        private ImageManagment _imageManagment;

        public CollectionController(ComicsDbContext context, UserManager<User> userManager, ComicsDbContext db, IWebHostEnvironment appEnviroment, ICollectionRepository collectionRepository, IUserRepository userRepository, ImageManagment imageManagment)
        {
            _context = context;
            _userManager = userManager;
            _db = db;
            _appEnviroment = appEnviroment;
            _collectionRepository = collectionRepository;
            _userRepository = userRepository;
            _imageManagment = imageManagment;
        }


        // GET: Collection
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Collections.ToListAsync());
        }

        // GET: Collection/Create
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Any(r => r == "guest"))
            {
                return RedirectToAction("Index");
            }

            return View(); //new Collection()
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Theme")] Collection collection, IFormFile uploadedImage)
        {
            if (ModelState.IsValid)
            {
                if (uploadedImage != null && uploadedImage.Length > 0)
                {
                        string coll_url = await _imageManagment.UploadImageAsync(collection.Name, uploadedImage.OpenReadStream());
                        collection.Img = coll_url;
                }

                User user = await _userManager.GetUserAsync(HttpContext.User);
                collection.UserId = user.Id;
                collection.User = user;

                _context.Add(collection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(collection);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collection = await _context.Collections.FindAsync(id);
            if (collection == null)
            {
                return NotFound();
            }
            return View(collection);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Theme, Img")] Collection collection, IFormFile uploadedImage)
        {
            if (id != collection.Id)
            {
                return NotFound();
            }

            if(uploadedImage != null && uploadedImage.Length > 0)
            {
                string coll_url = await _imageManagment.UploadImageAsync(collection.Name, uploadedImage.OpenReadStream());
                collection.Img = coll_url;
            }

            if (ModelState.IsValid)
            {

                try
                {
                    var collectionFromDb = _collectionRepository.GetCollectionById(collection.Id);
                    collection.UserId = collectionFromDb.UserId;
                    collection.User = collectionFromDb.User;
                    _collectionRepository.UpdateCollection(collection);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CollectionExists(collection.Id))
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
            return View(collection);
        }


        // GET: Collection/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collection = _collectionRepository.GetCollectionById(id);

            collection.User = _userRepository.GetUserDb(collection.UserId);
            collection.Items = _collectionRepository.GetCollectionItems(id);

            if (collection == null)
            {
                return NotFound();
            }

            return View("Details", collection);
        }


        public async Task<IActionResult> DeleteAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Any(r => r == "guest" || r == "user"))
            {
                return RedirectToAction("Index");
            }

            var collection = _collectionRepository.GetCollectionById(id);
            if (collection == null)
            {
                return NotFound();
            }

            return View(collection);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var collection = _collectionRepository.GetCollectionById(id);
            _collectionRepository.DeleteCollection(collection);
            return RedirectToAction(nameof(Index));
        }

        private bool CollectionExists(int id)
        {
            return _context.Collections.Any(e => e.Id == id);
        }
    }
}
