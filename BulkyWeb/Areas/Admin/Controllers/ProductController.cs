using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork db)
        {
			_unitOfWork = db;
        }
        public IActionResult Index()
        {
            List<Product> products =  new List<Product>();
			products = _unitOfWork.Product.GetAll().ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product obj)
        {
            //if (obj.Name == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("Name", "The DisplayOrder cannot exactly match the Name");  //custom validation
            //}
            if (ModelState.IsValid)   //check model property validations
            {
				_unitOfWork.Product.Add(obj);
				_unitOfWork.save();
				TempData["success"] = "Product created successfully";
				return RedirectToAction("Index", "Product");
			}	 
            return View();		
        }

		public IActionResult Edit(int? id)
		{
            if (id == null || id == 0)
            {
              return NotFound();
            }
            Product category = _unitOfWork.Product.Get(u => u.Id ==id);
            //other ways to impliment
            //Category? category1 = _db.Categories.FirstOrDefault(c => c.Id == id);
			//Category? category2 = _db.Categories.Where(c => c.Id == id).FirstOrDefault();

			if (category == null)
            {
                return NotFound(); 
            }
            return View(category);
		}
		[HttpPost]
		public IActionResult Edit(Product obj)
		{

            if (ModelState.IsValid)   //check model property validations
			{
				_unitOfWork.Product.Update(obj);
				_unitOfWork.save();
				TempData["success"] = "Category updated successfully";
				return RedirectToAction("Index", "Product");
			}
			return View();
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
