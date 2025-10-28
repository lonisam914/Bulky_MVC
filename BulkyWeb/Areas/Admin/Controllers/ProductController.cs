using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork db, IWebHostEnvironment webHostEnvironment )
        {
			_unitOfWork = db;
			_webHostEnvironment= webHostEnvironment;
		}
        public IActionResult Index()
        {
            List<Product> products =  new List<Product>();
			products = _unitOfWork.Product.GetAll().ToList();
            return View(products);
        }

		public IActionResult Upsert(int? id)   //using for tnoth udpate and create in one view page
			{
			ProductViewModel productViewModel = new()
			{
				CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString(),
				}),
				Product = new Product()
			};
			if (id == null || id == 0)   //create
			{
				return View(productViewModel);
			}
			else  //update
			{
				productViewModel.Product= _unitOfWork.Product.Get(u => u.Id == id);
				return View(productViewModel);
			}
		}
        [HttpPost]
        public IActionResult Upsert(ProductViewModel productViewModel,IFormFile? file)
        {
			if (ModelState.IsValid)   //check model property validations
			{
				string wwwRootPath = _webHostEnvironment.WebRootPath;
				if (file != null)
				{
					string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); //random Guide means random name for our file and extension
					string ProductPath = Path.Combine(wwwRootPath, @"images\Product");
					if (!string.IsNullOrEmpty(productViewModel.Product.ImageUrl))
					{
						//delete the old image
						var oldImagePath = Path.Combine(wwwRootPath,productViewModel.Product.ImageUrl.TrimStart('\\'));
						if (System.IO.File.Exists(oldImagePath))
						{
							System.IO.File.Delete(oldImagePath);
						}
					}
					using (var fileStream = new FileStream(Path.Combine(ProductPath, fileName), FileMode.Create))
					{
						file.CopyTo(fileStream);
					}
					productViewModel.Product.ImageUrl = @"\images\Product\" + fileName;
				}
				if (productViewModel.Product.Id == 0)
				{
					_unitOfWork.Product.Add(productViewModel.Product);
				}
				else
				{
					_unitOfWork.Product.Update(productViewModel.Product);
				}
				_unitOfWork.save();
				TempData["success"] = "Product created successfully";
				return RedirectToAction("Index", "Product");
			}
			else
			{
				productViewModel.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString(),
				});
				return View(productViewModel);
			}	
        }
		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			Product? product = _unitOfWork.Product.Get(u => u.Id == id);
			//other ways to impliment
			//Category? category1 = _db.Categories.FirstOrDefault(c => c.Id == id);
			//Category? category2 = _db.Categories.Where(c => c.Id == id).FirstOrDefault();

			if (product == null)
			{
				return NotFound();
			}
			return View(product);
		}
		[HttpPost , ActionName("Delete")]
		public IActionResult DeletePost(int? id)
		{
			Product? product = _unitOfWork.Product.Get(u => u.Id == id);
			if (product == null) { return NotFound() ; }
			_unitOfWork.Product.Remove(product);
			_unitOfWork.save();
			TempData["success"] = "Category created successfully";
			return RedirectToAction("Index", "Product");
		}
	}
}
