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


	}
}