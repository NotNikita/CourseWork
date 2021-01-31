using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comics.DAL;
using Comics.Domain;
using Comics.Domain.CrossRefModel;
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
            //comic.Comments = _comicRepository.GetCommentsByComicId(comic.Id);

            if (comic == null)
            {
                return NotFound();
            }

            return View("Details", comic);
        }

        // GET: Comic/Create
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create(int collectionId)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Any(r => r == "guest"))
            {
                return RedirectToAction("Index");
            }

            return View(new Comic() { CollectionId = collectionId }) ;
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Publisher,Price,Format,Tags,Released,PageCount,Description")] Comic comic, IFormFile uploadedImage, int coll_Id)
        {
            if (ModelState.IsValid)
            {
                if (uploadedImage != null && uploadedImage.Length > 0)
                {
                    string coll_url = await _imageManagment.UploadImageAsync(comic.Name, uploadedImage.OpenReadStream());
                    comic.Img = coll_url;
                }

                User user = await _userManager.GetUserAsync(HttpContext.User);
                comic.UserId = user.Id;
                comic.User = user;
                comic.Created = DateTime.Now;
                comic.CollectionId = coll_Id;

                //_comicRepository.AddComic(comic);
                await _context.AddAsync(comic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(comic);
        }

        [Route("Comic/{pbId?}/{comment}")]
        public async Task<IActionResult> CreateComment(int pbId, string comment)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            Comment comm = new Comment
            {
                ItemId = pbId,
                Author = user,
                Text = comment,
                Item = _comicRepository.GetComicById(pbId),
                CreationDate = DateTime.Now,
                ItemTheme = "Comic"
            };

            if (ModelState.IsValid)
            {
                _commentRepository.AddComm(comm);
            }
            return RedirectToAction("CommentsList", new { id = pbId });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Comic/Comment/Delete/{id?}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var comm = await _db.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (comm == null)
            {
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var lotId = comm.ItemId;
            if (comm.Author != user && !roles.Any(r => r == "admin" || r == "moderator"))
            {
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }
            _commentRepository.RemoveComm(comm);
            await _db.SaveChangesAsync();
            return RedirectToAction("CommentsList", new { id = lotId });
        }

        public PartialViewResult CommentsList(int id)
        {
            Comic lot = _comicRepository.GetComicById(id);
            lot.Comments = _comicRepository.GetCommentsByComicId(id);
            return PartialView(lot.Comments);
        }

        // GET: Comic/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }

            var comic = await _context.Comics.FindAsync(id);
            if (comic == null)
            {
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }
            return View(comic);
        }

        // POST: Comic/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Tags,Publisher,Img")] Comic comic, IFormFile uploadedImage)
        {
            if (id != comic.Id)
            {
                return NotFound();
            }

            if (uploadedImage != null && uploadedImage.Length > 0)
            {
                string coll_url = await _imageManagment.UploadImageAsync(comic.Name, uploadedImage.OpenReadStream());
                comic.Img = coll_url;
            }

            if (ModelState.IsValid)
            {

                try
                {
                    var comicFromDb = _comicRepository.GetComicById(comic.Id);
                    comicFromDb.Img = comic.Img;
                    comicFromDb.Name = comic.Name;
                    comicFromDb.Publisher = comic.Publisher;
                    comicFromDb.Description = comic.Description;
                    comicFromDb.Tags = comic.Tags;
                    comicFromDb.User = await _userManager.FindByIdAsync(comicFromDb.UserId);
                    _comicRepository.Update(comicFromDb);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComicExists(comic.Id))
                    {
                        return RedirectPermanent("~/Error/Index?statusCode=404");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(comic);
        }

        // GET: Comic/AddToWishList/1
        public async Task<IActionResult> AddToWishList(int id)
        {
            try
            {
                var curr_user = await _userManager.GetUserAsync(HttpContext.User);
                var like_db = _context.Likes.FirstOrDefault(x =>
                x.ItemId == id && x.UserId == curr_user.Id);
                if (like_db == null)
                {
                    var comic = await _db.Comics.FirstOrDefaultAsync(com => com.Id == id);
                    var like = new Like()
                    {
                        ItemId = comic.Id,
                        Item = comic,
                        UserId = curr_user.Id,
                        User = curr_user
                    };
                    comic.Likes.Add(like);
                    _context.Add(like);
                    _comicRepository.Update(comic);
                    _db.Likes.Update(like);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _db.Likes.Remove(like_db);
                    await _context.SaveChangesAsync();
                }
            }
            catch (NullReferenceException)
            {
                //logger.LogError("Doesn't exist item. Controller:Comic. Action:AddToWishList");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Comic/Delete/5
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

            var collection = _comicRepository.GetComicById(id);
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
            var comic = _comicRepository.GetComicById(id);
            _comicRepository.DeleteComic(comic);
            _db.Likes.RemoveRange(comic.Likes);
            return RedirectToAction(nameof(Index));
        }

        private bool ComicExists(int id)
        {
            return _context.Comics.Any(e => e.Id == id);
        }
    }
}
