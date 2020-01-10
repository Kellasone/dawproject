using MarkIT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarkIT.Controllers
{
	public class CategoryController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();
		[Authorize(Roles = "Administrator")]
		public ActionResult Show()
		{
			var categories = db.Category.ToList();
			ViewBag.categories = categories;
			return View();
		}



		[Authorize(Roles = "User,Administrator")]
		public ActionResult New()
		{
			Category category = new Category();


			return View(category);

		}

		[HttpPost]
		public ActionResult New(Category category)
		{
			try
			{
				if (ModelState.IsValid)
				{
					db.Category.Add(category);
					db.SaveChanges();
					TempData["message"] = "Category created!";
					return RedirectToAction("Index", "Bookmark");

				}
				else
				{
					return View(category);
				}
			}
			catch (Exception)
			{
				return View(category);
			}
		}
		[Authorize(Roles = "Administrator")]
		public ActionResult Delete(int id)
		{
			var category = db.Category.Find(id);
			db.Category.Remove(category);

			var bookmarksToBeDeleted = db.SavedBookmarks.Where(m => m.CategoryId == id);

			foreach (var bookmark in bookmarksToBeDeleted)
			{
				db.SavedBookmarks.Remove(bookmark);
			}

			db.SaveChanges();
			return RedirectToAction("Show", "Category");
		}

		[Authorize(Roles = "Administrator")]
		public ActionResult Edit(int id)
		{
			Category category = db.Category.Find(id);
			ViewBag.Category = category;
			return View(category);
		}

		[HttpPost]
		public ActionResult Edit(int id, Bookmark requestCategory)
		{
			Category category = db.Category.Find(id);

			category.Title = requestCategory.Title;
			db.SaveChanges();
			return RedirectToAction("Show/", "category");

		}
	}
}