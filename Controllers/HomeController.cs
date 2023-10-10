using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTest.Data;
using MyTest.Models;
using MyTest.Models.ViewModel;
using System.Diagnostics;

namespace MyTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;

        public HomeController(ApplicationDbContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Categories = db.Category,
                Products = db.Product.Include(u => u.ApplicationType).Include(u => u.Category)
            };

            return View(homeVM);
        }

        public IActionResult Details(int id) 
        {
            DetailsVM detailVM = new DetailsVM()
            {
                Product = db.Product.Include(u=>u.ApplicationType).Include(i=>i.Category).Where(u=>u.Id==id).FirstOrDefault()
                , ExistsInCart = false
            };

            return View(detailVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}