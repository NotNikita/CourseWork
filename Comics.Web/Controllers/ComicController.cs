using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comics.DAL;
using Comics.Domain;
using Comics.Services;
using Comics.Services.Abstract;
using Comics.Web.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private IComicRepository _comicRepository;
        private ICommentsRepository _commentRepository;
        private IUserRepository _userRepository;
        private ImageManagment _imageManagment;
        private IHubContext<UpdateHub> _updateHub;

        public ComicController(ComicsDbContext context, UserManager<User> userManager, ComicsDbContext db, IWebHostEnvironment appEnviroment, IComicRepository comicRepository, ICommentsRepository commentRepository, IUserRepository userRepository, ImageManagment imageManagment, IHubContext<UpdateHub> updateHub)
        {
            _context = context;
            _userManager = userManager;
            _db = db;
            _appEnviroment = appEnviroment;
            _comicRepository = comicRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _imageManagment = imageManagment;
            _updateHub = updateHub;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View(await _context.Comics.ToListAsync());
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }

            var comic = _comicRepository.GetComicById(id);

            comic.User = _userRepository.GetUserDb(comic.UserId);
            comic.Comments = _comicRepository.GetCommentsByComicId(comic.Id);

            if (comic == null)
            {
                return NotFound();
            }

            return View("Details", comic);
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

        [Route("Lot/{pbId?}/{comment}")]
        public async Task<IActionResult> CreateComment(int pbId, string comment)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            Comment comm = new Comment
            {
                ItemId = pbId,
                Author = user,
                Text = comment,
                Item = _comicRepository.GetComicById(pbId)
            };

            if (ModelState.IsValid)
            {
                _commentRepository.AddComm(comm);
            }
            return RedirectToAction("CommentsList", new { id = pbId });

        }

        public PartialViewResult CommentsList(int id)
        {
            Comic lot = _comicRepository.GetComicById(id);
            lot.Comments = _comicRepository.GetCommentsByComicId(id);
            return PartialView(lot.Comments);
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
