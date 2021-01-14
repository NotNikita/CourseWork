using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Comics.Web.Controllers
{
    public class ComicController : Controller
    {
        // GET: ComicController
        public ActionResult Index()
        {
            return View();
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
