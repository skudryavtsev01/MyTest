using Microsoft.AspNetCore.Mvc;
using MyTest.Data;
using MyTest.Models;
using System.Collections.Generic;

namespace MyTest.Controllers
{
    public class ApplicationTypeController : Controller
    {
        private readonly ApplicationDbContext db;
        public ApplicationTypeController(ApplicationDbContext _db)
        {
            db = _db;
        }
        
        public IActionResult Index()
        {
            IEnumerable<ApplicationType> objList = db.ApplicationType;
            return View(objList);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationType obj)
        {
            if(ModelState.IsValid) 
            {
                db.ApplicationType.Add(obj);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) 
            {
                return NotFound();
            }

            var obj = db.ApplicationType.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType obj)
        {
            if (ModelState.IsValid)
            {
                db.ApplicationType.Update(obj);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = db.ApplicationType.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = db.ApplicationType.Find(id);
            if (ModelState.IsValid)
            {
                db.ApplicationType.Remove(obj);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}
