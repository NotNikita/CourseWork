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
    public class WhiskyController : Controller
    {
        private readonly ComicsDbContext _context;
        private UserManager<User> _userManager;
        private ComicsDbContext _db;
        private IWebHostEnvironment _appEnviroment;
        private IWhiskyRepository _whiskyRepository;
        private ICommentsRepository _commentRepository;
        private IUserRepository _userRepository;
        private ImageManagment _imageManagment;
        private IHubContext<UpdateHub> _updateHub;

        public WhiskyController(ComicsDbContext context, UserManager<User> userManager, ComicsDbContext db, IWebHostEnvironment appEnviroment, IWhiskyRepository whiskyRepository, ICommentsRepository commentRepository, IUserRepository userRepository, ImageManagment imageManagment, IHubContext<UpdateHub> updateHub)
        {
            _context = context;
            _userManager = userManager;
            _db = db;
            _appEnviroment = appEnviroment;
            _whiskyRepository = whiskyRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _imageManagment = imageManagment;
            _updateHub = updateHub;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View(await _context.Whiskies.ToListAsync());
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }

            var item = _whiskyRepository.GetWhiskyById(id);
            item.User = _userRepository.GetUserDb(item.UserId);

            if (item == null)
            {
                return NotFound();
            }

            return View("Details", item);
        }

        // GET: Whisky/Create
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

            return View(new Whisky() { CollectionId = collectionId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Tags,Price,Brand,Vintage,Bottled,Strength,Size")] Whisky whisky, IFormFile uploadedImage, int coll_Id)
        {
            if (ModelState.IsValid)
            {
                if (uploadedImage != null && uploadedImage.Length > 0)
                {
                    string coll_url = await _imageManagment.UploadImageAsync(whisky.Name, uploadedImage.OpenReadStream());
                    whisky.Img = coll_url;
                }

                User user = await _userManager.GetUserAsync(HttpContext.User);
                whisky.UserId = user.Id;
                whisky.User = user;
                whisky.Created = DateTime.Now;
                whisky.CollectionId = coll_Id;

                await _context.AddAsync(whisky);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(whisky);
        }

        [Route("Whisky/{pbId?}/{comment}")]
        public async Task<IActionResult> CreateComment(int pbId, string comment)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            Comment comm = new Comment
            {
                ItemId = pbId,
                Author = user,
                Text = comment,
                Item = _whiskyRepository.GetWhiskyById(pbId),
                CreationDate = DateTime.Now,
                ItemTheme = "Whisky"
            };

            if (ModelState.IsValid)
            {
                _commentRepository.AddComm(comm);
            }
            return RedirectToAction("CommentsList", new { id = pbId });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Whisky/Comment/Delete/{id?}")]
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
            Whisky lot = _whiskyRepository.GetWhiskyById(id);
            lot.Comments = _whiskyRepository.GetCommentsByWhiskyId(id);
            return PartialView(lot.Comments);
        }

        // GET: Whisky/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }

            var whisky = await _context.Whiskies.FindAsync(id);
            if (whisky == null)
            {
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }
            return View(whisky);
        }

        // POST: Whisky/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Tags,Price,Brand,Vintage,Bottled,Strength,Size,Img")] Whisky whisky, IFormFile uploadedImage)
        {
            if (id != whisky.Id)
            {
                return NotFound();
            }

            if (uploadedImage != null && uploadedImage.Length > 0)
            {
                string coll_url = await _imageManagment.UploadImageAsync(whisky.Name, uploadedImage.OpenReadStream());
                whisky.Img = coll_url;
            }

            if (ModelState.IsValid)
            {

                try
                {
                    var whiskyFromDb = _whiskyRepository.GetWhiskyById(whisky.Id);
                    whiskyFromDb.Img = whisky.Img;
                    whiskyFromDb.Name = whisky.Name;
                    whiskyFromDb.Brand = whisky.Brand;
                    whiskyFromDb.Vintage = whisky.Vintage;
                    whiskyFromDb.Bottled = whisky.Bottled;
                    whiskyFromDb.Strength = whisky.Strength;
                    whiskyFromDb.Size = whisky.Size;
                    whiskyFromDb.Tags = whisky.Tags;
                    whiskyFromDb.User = await _userManager.FindByIdAsync(whiskyFromDb.UserId);
                    _whiskyRepository.Update(whiskyFromDb);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WhiskyExists(whisky.Id))
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
            return View(whisky);
        }

        // GET: Whisky/AddToWishList/1
        public async Task<IActionResult> AddToWishList(int id)
        {
            try
            {
                var curr_user = await _userManager.GetUserAsync(HttpContext.User);
                var like_db = _context.Likes.FirstOrDefault(x =>
                x.ItemId == id && x.UserId == curr_user.Id);
                if (like_db == null)
                {
                    var whisky = await _db.Whiskies.FirstOrDefaultAsync(com => com.Id == id);
                    var like = new Like()
                    {
                        ItemId = whisky.Id,
                        Item = whisky,
                        UserId = curr_user.Id,
                        User = curr_user
                    };
                    whisky.Likes.Add(like);
                    _context.Add(like);
                    _whiskyRepository.Update(whisky);
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

        // GET: Whisky/Delete/5
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

            var collection = _whiskyRepository.GetWhiskyById(id);
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
            var whisky = _whiskyRepository.GetWhiskyById(id);
            _whiskyRepository.DeleteWhisky(whisky);
            _db.Likes.RemoveRange(whisky.Likes);
            return RedirectToAction(nameof(Index));
        }

        private bool WhiskyExists(int id)
        {
            return _context.Whiskies.Any(e => e.Id == id);
        }
    }
}
