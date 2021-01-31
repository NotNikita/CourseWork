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
    public class BikeController : Controller
    {
        private readonly ComicsDbContext _context;
        private UserManager<User> _userManager;
        private ComicsDbContext _db;
        private IWebHostEnvironment _appEnviroment;
        private IBikeRepository _bikeRepository;
        private ICommentsRepository _commentRepository;
        private IUserRepository _userRepository;
        private ImageManagment _imageManagment;
        private IHubContext<UpdateHub> _updateHub;

        public BikeController(ComicsDbContext context, UserManager<User> userManager, ComicsDbContext db, IWebHostEnvironment appEnviroment, IBikeRepository bikeRepository, ICommentsRepository commentRepository, IUserRepository userRepository, ImageManagment imageManagment, IHubContext<UpdateHub> updateHub)
        {
            _context = context;
            _userManager = userManager;
            _db = db;
            _appEnviroment = appEnviroment;
            _bikeRepository = bikeRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _imageManagment = imageManagment;
            _updateHub = updateHub;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View(await _context.Bikes.ToListAsync());
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }

            var bike = _bikeRepository.GetBikeById(id);
            bike.User = _userRepository.GetUserDb(bike.UserId);

            if (bike == null)
            {
                return NotFound();
            }

            return View("Details", bike);
        }

        // GET: Bike/Create
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

            return View(new Bike() { CollectionId = collectionId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Producer,Color,Year,Tags,Price,BikeType,WheelDiameter,Description")] Bike bike, IFormFile uploadedImage, int coll_Id)
        {
            if (ModelState.IsValid)
            {
                if (uploadedImage != null && uploadedImage.Length > 0)
                {
                    string coll_url = await _imageManagment.UploadImageAsync(bike.Name, uploadedImage.OpenReadStream());
                    bike.Img = coll_url;
                }

                User user = await _userManager.GetUserAsync(HttpContext.User);
                bike.UserId = user.Id;
                bike.User = user;
                bike.Created = DateTime.Now;
                bike.CollectionId = coll_Id;

                await _context.AddAsync(bike);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bike);
        }

        [Route("Bike/{pbId?}/{comment}")]
        public async Task<IActionResult> CreateComment(int pbId, string comment)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            Comment comm = new Comment
            {
                ItemId = pbId,
                Author = user,
                Text = comment,
                Item = _bikeRepository.GetBikeById(pbId),
                CreationDate = DateTime.Now,
                ItemTheme = "Bike"
            };

            if (ModelState.IsValid)
            {
                _commentRepository.AddComm(comm);
            }
            return RedirectToAction("CommentsList", new { id = pbId });

        }

        public PartialViewResult CommentsList(int id)
        {
            Bike lot = _bikeRepository.GetBikeById(id);
            lot.Comments = _bikeRepository.GetCommentsByBikeId(id);
            return PartialView(lot.Comments);
        }

        // GET: Bike/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }

            var bike = await _context.Bikes.FindAsync(id);
            if (bike == null)
            {
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }
            return View(bike);
        }

        // POST: Bike/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Tags,Producer,Color,BikeType,WheelDiameter,Img")] Bike bike, IFormFile uploadedImage)
        {
            if (id != bike.Id)
            {
                return NotFound();
            }

            if (uploadedImage != null && uploadedImage.Length > 0)
            {
                string coll_url = await _imageManagment.UploadImageAsync(bike.Name, uploadedImage.OpenReadStream());
                bike.Img = coll_url;
            }

            if (ModelState.IsValid)
            {

                try
                {
                    var bikeFromDb = _bikeRepository.GetBikeById(bike.Id);
                    bikeFromDb.Img = bike.Img;
                    bikeFromDb.Name = bike.Name;
                    bikeFromDb.Producer = bike.Producer;
                    bikeFromDb.Color = bike.Color;
                    bikeFromDb.BikeType = bike.BikeType;
                    bikeFromDb.WheelDiameter = bike.WheelDiameter;
                    bikeFromDb.Description = bike.Description;
                    bikeFromDb.Tags = bike.Tags;
                    bikeFromDb.User = await _userManager.FindByIdAsync(bikeFromDb.UserId);
                    _bikeRepository.Update(bikeFromDb);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BikeExists(bike.Id))
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
            return View(bike);
        }

        // GET: Bike/AddToWishList/1
        public async Task<IActionResult> AddToWishList(int id)
        {
            try
            {
                var curr_user = await _userManager.GetUserAsync(HttpContext.User);
                var like_db = _context.Likes.FirstOrDefault(x =>
                x.ItemId == id && x.UserId == curr_user.Id);
                if (like_db == null)
                {
                    var bike = await _db.Bikes.FirstOrDefaultAsync(com => com.Id == id);
                    var like = new Like()
                    {
                        ItemId = bike.Id,
                        Item = bike,
                        UserId = curr_user.Id,
                        User = curr_user
                    };
                    bike.Likes.Add(like);
                    _context.Add(like);
                    _bikeRepository.Update(bike);
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

        // GET: Bike/Delete/5
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

            var collection = _bikeRepository.GetBikeById(id);
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
            var bike = _bikeRepository.GetBikeById(id);
            _bikeRepository.DeleteBike(bike);
            _db.Likes.RemoveRange(bike.Likes);
            return RedirectToAction(nameof(Index));
        }

        private bool BikeExists(int id)
        {
            return _context.Bikes.Any(e => e.Id == id);
        }
    }
}
