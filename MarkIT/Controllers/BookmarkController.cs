using MarkIT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace MarkIT.Controllers
{
	
    public class BookmarkController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int _perPage = 5;

        // GET: Bookmark
        public ActionResult Index()
        {
			var bookmarks = db.Bookmarks.Include("User").OrderBy(a => a.Id);

			if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            var totalItems = bookmarks.Count();
            var currentPage = Convert.ToInt32(Request.Params.Get("Page"));
            var offset = 0;

            if(!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * this._perPage;
            }
            var paginatedBookmarks = bookmarks.Skip(offset).Take(this._perPage);

            ViewBag.perPage = this._perPage;
            ViewBag.total = totalItems;
            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)this._perPage);
            ViewBag.Bookmarks = paginatedBookmarks;
            
            return View();
        }

        public ActionResult Show(int id)
        {
            Bookmark bookmark = db.Bookmarks.Find(id);

            if (User.Identity.GetUserId() == bookmark.UserId || User.IsInRole("Administrator"))
                ViewBag.afisareButoane = true;
            else
                ViewBag.afisareButoane = false;

            return View(bookmark);
        }

        [Authorize(Roles = "User,Administrator")]
        public ActionResult New()
        {
            Bookmark bookmark = new Bookmark();

            bookmark.UserId = User.Identity.GetUserId();

            return View(bookmark);

        }

        [HttpPost]
        public ActionResult New(Bookmark bookmark)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Bookmarks.Add(bookmark);
                    db.SaveChanges();
                    TempData["message"] = "Bookmark created!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(bookmark);
                }
            }
            catch (Exception)
            {
                return View(bookmark);
            }
        }
        [Authorize(Roles = "User,Administrator")]
        public ActionResult Edit(int id)
        {
			
			  Bookmark bookmark = db.Bookmarks.Find(id);
			  ViewBag.Article = bookmark; 
			  return View(bookmark);
			 
	
        }
        
        [HttpPut]
        public ActionResult Edit(int id, Bookmark requestBookmark)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Bookmark bookmark = db.Bookmarks.Find(id);
                    if (TryUpdateModel(bookmark))
                    {
                        bookmark.Title = requestBookmark.Title;
                        bookmark.Description = requestBookmark.Description;
                        bookmark.ImageLink = requestBookmark.ImageLink;
                        bookmark.Link = requestBookmark.Link;
                        bookmark.Tags = requestBookmark.Tags;
                        bookmark.UserId = requestBookmark.UserId;
                        db.SaveChanges();
                        TempData["message"] = "Bookmark edited!";
                    }
                    return RedirectToAction("Index");
                }
                else
                {
					Console.WriteLine("error");
					return View(requestBookmark);
                }

            }
            catch (Exception)
            {
                return View(requestBookmark);
            }
        }

        [Authorize(Roles = "User, Administrator")]
        public ActionResult Delete(int id)
        {
            Bookmark bookmark = db.Bookmarks.Find(id);
            db.Bookmarks.Remove(bookmark);
            db.SaveChanges();
            TempData["message"] = "Bookmark deleted!";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "User, Administrator")]
        public ActionResult PersonalBookmarks()
		{
			string user = User.Identity.GetUserId();
			Bookmark[] listOfPersonalBookmarks = db.Bookmarks.Where(m => m.UserId == user).ToArray();
			ViewBag.bookmarks = listOfPersonalBookmarks;
			return View();

		}
    }
}