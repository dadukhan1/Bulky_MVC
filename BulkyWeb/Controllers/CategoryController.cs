using Bulky.DataAccess.Data;
using Bulky.DatabAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
            List<Category> categoryObj = _categoryRepo.GetAll().ToList();
            return View(categoryObj);
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
                ModelState.AddModelError("Name", "The Display Order can't exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                _categoryRepo.Add(obj);
                _categoryRepo.Save();
                TempData["Success"] = "Category created succefully.";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryObj = _categoryRepo.Get(u => u.Id == id);
            if (categoryObj == null)
            {
                return NotFound();
            }
            return View(categoryObj);
        }


        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Update(obj);
                _categoryRepo.Save();
                TempData["Success"] = "Category updated succefully.";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryObj = _categoryRepo.Get(u => u.Id == id);
            if (categoryObj == null)
            {
                return NotFound();
            }
            return View(categoryObj);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? categoryObj = _categoryRepo.Get(u => u.Id == id);
            if (categoryObj == null)
            {
                return NotFound();
            }
            _categoryRepo.Delete(categoryObj);
            _categoryRepo.Save();
            TempData["Success"] = "Category deleted succefully.";
            return RedirectToAction("Index");
        }
    }
}
