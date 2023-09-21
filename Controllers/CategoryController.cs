using Microsoft.AspNetCore.Mvc;
using MyTest.Data;
using MyTest.Models;
using System.Collections.Generic;

namespace MyTest.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext db;
        public CategoryController(ApplicationDbContext _db)
        {
            db = _db;
        }
        
        public IActionResult Index()
        {
            IEnumerable<Category> objList = db.Category;
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
        public IActionResult Create(Category obj)
        {
            db.Category.Add(obj);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
