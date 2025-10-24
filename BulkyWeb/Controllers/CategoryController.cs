using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using BulkyWeb.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        public CategoryController(ICategoryRepository db)
        {
			_categoryRepo = db;
        }
        public IActionResult Index()
        {
            List<Category> categories =  new List<Category>();
            categories = _categoryRepo.GetAll().ToList();
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
				_categoryRepo.Add(obj);
				_categoryRepo.Save();
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
            Category category = _categoryRepo.Get(u => u.Id ==id);
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
				_categoryRepo.Update(obj);
				_categoryRepo.Save();
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
			Category? category = _categoryRepo.Get(u => u.Id == id);
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
			Category? category = _categoryRepo.Get(u => u.Id == id);
			if (category == null) { return NotFound() ; }
			_categoryRepo.Remove(category);
			_categoryRepo.Save();
			TempData["success"] = "Category created successfully";
			return RedirectToAction("Index", "Category");
		}


	}
}
