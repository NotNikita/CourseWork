using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Comics.Web.Models;
using Comics.DAL;
using Microsoft.EntityFrameworkCore;
using Comics.Domain;

namespace Comics.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ComicsDbContext _context;
        private ComicsDbContext _db;

        public HomeController(ILogger<HomeController> logger, ComicsDbContext context, ComicsDbContext db)
        {
            _logger = logger;
            _context = context;
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View(await _context.BaseItems.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> Search(string q)
        {
            //var results = _context.BaseItems.Where(item => EF.Functions.FreeText(item.Name, q));
            var itemquery = from x in _db.BaseItems select x;
            if (!string.IsNullOrEmpty(q))
            {
                itemquery = itemquery.Where(x => x.Name.Contains(q) || x.Description.Contains(q));
            }
            
            return View("Index", await itemquery.AsNoTracking().ToListAsync());
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true

                }
            );
            return RedirectToAction("Index", "Collection");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
