using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyTest.Data;
using MyTest.Models;
using MyTest.Models.ViewModel;
using System.Collections.Generic;
//using System.Data.Entity;

namespace MyTest.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ProductController(ApplicationDbContext _db, IWebHostEnvironment _webHostEnvironment)
        {
            db = _db;
            webHostEnvironment = _webHostEnvironment;
        }
        
        public IActionResult Index()
        {
            IEnumerable<Product> objList = db.Product.Include(i=>i.Category).Include(i=>i.ApplicationType);

            return View(objList);
        }

        //GET - UPSERT
        public IActionResult Upsert(int? id)
        {

            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),

                ApplicationTypeSelectList = db.ApplicationType.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                //this is for create
                return View(productVM);
            }
            else 
            {
                //this is for edit
                productVM.Product = db.Product.Find(id);
                if (productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            var files = HttpContext.Request.Form.Files;
            string webRootPath = webHostEnvironment.WebRootPath;

            if (productVM.Product.Id == 0)
            {
                //Creating
                string upload = webRootPath + WC.ImagePath;
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(files[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                productVM.Product.Image = fileName + extension;

                db.Product.Add(productVM.Product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else 
            {
            //Updating
                var objFromDb = db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productVM.Product.Id);
                if (files.Count > 0)
                {
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    var oldFile = Path.Combine(upload, objFromDb.Image);

                    if (System.IO.File.Exists(oldFile))
                    {
                        System.IO.File.Delete(oldFile);
                    }

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.Image = fileName + extension;
                }
                else 
                {
                    productVM.Product.Image = objFromDb.Image;
                }

                db.Product.Update(productVM.Product);
            }

            productVM.CategorySelectList = db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            productVM.ApplicationTypeSelectList = db.ApplicationType.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            return View(productVM);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = db.Product.Include(u=>u.Category).Include(i=>i.ApplicationType).FirstOrDefault(u => u.Id == id); //Жадная загрузка


            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //POST
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = db.Product.Find(id);

            if (obj == null) return NotFound();

            var files = obj.Image;

            if (ModelState.IsValid)
            {
                if (!files.IsNullOrEmpty())
                {
                    string upload = webHostEnvironment.WebRootPath + WC.ImagePath;

                    var file = Path.Combine(upload, obj.Image);

                    if (System.IO.File.Exists(file))
                    {
                        System.IO.File.Delete(file);
                    }
                }

                db.Product.Remove(obj);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}
