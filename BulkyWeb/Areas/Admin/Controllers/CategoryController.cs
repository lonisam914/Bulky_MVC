using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork db)
        {
			_unitOfWork = db;
        }
        public IActionResult Index()
        {
            List<Category> categories =  new List<Category>();
            categories = _unitOfWork.Category.GetAll().ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The DisplayOrder cannot exactly match the Name");  //custom validation
            }
            if (ModelState.IsValid)   //check model property validations
            {
				_unitOfWork.Category.Add(obj);
				_unitOfWork.save();
				TempData["success"] = "Category created successfully";
				return RedirectToAction("Index", "Category");
			}	 
            return View();		
        }

		public IActionResult Edit(int? id)
		{
            if (id == null || id == 0)
            {
              return NotFound();
            }
            Category category = _unitOfWork.Category.Get(u => u.Id ==id);
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
		public IActionResult Edit(Category obj)
		{

            if (ModelState.IsValid)   //check model property validations
			{
				_unitOfWork.Category.Update(obj);
				_unitOfWork.save();
				TempData["success"] = "Category updated successfully";
				return RedirectToAction("Index", "Category");
			}
			return View();
		}

		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			Category? category = _unitOfWork.Category.Get(u => u.Id == id);
			//other ways to impliment
			//Category? category1 = _db.Categories.FirstOrDefault(c => c.Id == id);
			//Category? category2 = _db.Categories.Where(c => c.Id == id).FirstOrDefault();

			if (category == null)
			{
				return NotFound();
			}
			return View(category);
		}
		[HttpPost , ActionName("Delete")]
		public IActionResult DeletePost(int? id)
		{
			Category? category = _unitOfWork.Category.Get(u => u.Id == id);
			if (category == null) { return NotFound() ; }
			_unitOfWork.Category.Remove(category);
			_unitOfWork.save();
			TempData["success"] = "Category created successfully";
			return RedirectToAction("Index", "Category");
		}


	}
}
