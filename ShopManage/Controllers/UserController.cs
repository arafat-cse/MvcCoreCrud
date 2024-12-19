using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using ShopManage.Models;
using ShopManage.Models.Vm;

namespace ShopManage.Controllers
{
    public class UserController : Controller
    {
        private readonly UserDbContext _db;
        private readonly IWebHostEnvironment _env;

        public UserController(UserDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public IActionResult Index()
        {
            IQueryable<User> user =_db.Users.Include(d => d.Details).ThenInclude(p => p.Product);
            return View(user);
        }
        public IActionResult Create()
        {
            //ViewBag.Products = _db.Products.ToList();
            ViewBag.Products = new SelectList(_db.Products, "ProductId", "ProductName").ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vmodel vmodel, int[] ProductId)
        {
            if (ModelState.IsValid)
            {
                User user = new User()
                {
                    UserName = vmodel.UserName,
                    Age = vmodel.Age,
                    IsActive = vmodel.IsActive,
                    Date = vmodel.Date,
                };


                if (vmodel.ImagePath != null)
                {
                    var webroot = _env.WebRootPath;
                    var folder = "Images";
                    var imgFileName = Path.GetFileName(vmodel.ImagePath.FileName);
                    var fileToSave = Path.Combine(webroot, folder, imgFileName);
                    using (var stream = System.IO.File.Create(fileToSave))
                    {
                        await vmodel.ImagePath.CopyToAsync(stream);
                    }
                    user.ImagePath = "/" + folder + "/" + imgFileName;
                }


                foreach (var item in ProductId)
                {
                    var product = _db.Products.FirstOrDefault(f => f.ProductId == item);
                    Detail detail = new Detail()
                    {
                        User = user,
                        UserId = user.UserId,
                        ProductName = product.ProductName,
                        ProductId = product.ProductId,
                    };
                    _db.Details.Add(detail);
                }


                var p = await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }


            // ViewBag.Products = _db.Products.Select(p => new SelectListItem { Value = p.ProductId.ToString(), Text = p.ProductName }).ToList();

            return View(vmodel);
        }

        public IActionResult AddProductDropdown(int? id)
        {
            ViewBag.Products = new SelectList(_db.Products, "ProductId", "ProductName", id ?? 0).ToList();

            /*ViewBag.Products = products;*/
            return PartialView("_ProductDropdown");
        }
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                var user = _db.Users.Find(id); 
                var dp = _db.Details.Where(u => u.UserId == user.UserId).ToList();
                _db.Details.RemoveRange(dp);
                _db.Users.Remove(user);
                _db.SaveChanges();
                /* return RedirectToAction(nameof("Index"));*/
                return RedirectToAction("Index");

            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ViewBag.Products = new SelectList(_db.Products.ToList(), "ProductId", "ProductName");
            if (id != null)
            {
                var user = _db.Users.Find(id);
                var dp = _db.Details.Where(u => u.UserId == user.UserId).ToList();
                if (user != null)
                {
                    var obj = new Vmodel()
                    {
                        UserId = user.UserId,
                        UserName = user.UserName,
                        Age = user.Age,
                        Date = user.Date,
                        IsActive = user.IsActive,
                        ImageFile = user.ImagePath,
                        ProductList = dp,
                    };
                    //obj.ProductList = dp;
                    return View(obj);
                }
            }
            return View();
        }

        [HttpPost] 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Vmodel vmodel, int[] ProductId)
        {
            if (ModelState.IsValid)
            {
                var user = _db.Users.Find(vmodel.UserId);
                user.UserName = vmodel.UserName;
                user.Age = vmodel.Age;
                user.Date = vmodel.Date;
                user.IsActive = vmodel.IsActive;

                if (vmodel.ImagePath != null)
                {
                    var webroot = _env.WebRootPath;
                    var folder = "Images";
                    var imgFileName = Path.GetFileName(vmodel.ImagePath.FileName);
                    var fileToSave = Path.Combine(webroot, folder, imgFileName);
                    using (var stream = System.IO.File.Create(fileToSave))
                    {
                        await vmodel.ImagePath.CopyToAsync(stream);
                    }
                    user.ImagePath = "/" + folder + "/" + imgFileName;
                }
                else
                {
                    user.ImagePath = user.ImagePath;
                }
                var prod = _db.Details.Where(e => e.UserId == user.UserId).ToList();
                if (prod != null)
                {
                    _db.Details.RemoveRange(prod);
                }

                foreach (var p in ProductId)
                {
                    var product = _db.Products.FirstOrDefault(f => f.ProductId == p);
                    Detail detail = new Detail()
                    {
                        User = user,
                        UserId = user.UserId,
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                    };
                    _db.Details.Add(detail);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vmodel);
        }

    }
}
