using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comics.DAL;
using Comics.Domain;
using Comics.Services;
using Comics.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comics.Web.Controllers
{
    [Authorize]
    public class ComicController : Controller
    {
        private readonly ComicsDbContext _context;
        private UserManager<User> _userManager;
        private ComicsDbContext _db;
        private IWebHostEnvironment _appEnviroment;
        private IComicRepository _collectionRepository;
        private IUserRepository _userRepository;
        private ImageManagment _imageManagment;

        public ComicController(ComicsDbContext context, UserManager<User> userManager, ComicsDbContext db, IWebHostEnvironment appEnviroment, IComicRepository collectionRepository, IUserRepository userRepository, ImageManagment imageManagment)
        {
            _context = context;
            _userManager = userManager;
            _db = db;
            _appEnviroment = appEnviroment;
            _collectionRepository = collectionRepository;
            _userRepository = userRepository;
            _imageManagment = imageManagment;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View(await _context.Comics.ToListAsync());
        }

        // GET: ComicController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ComicController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ComicController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ComicController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ComicController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ComicController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ComicController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
