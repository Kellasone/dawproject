using MarkIT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarkIT.Controllers
{
    public class BookmarkController : Controller
    {
        private BookmarkDBContext db = new BookmarkDBContext();

        // GET: Bookmark
        public ActionResult Index()
        {
            var bookmarks = db.Bookmarks;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            ViewBag.Bookmarks = bookmarks;
            
            return View();
        }

        public ActionResult Show(int id)
        {
            Bookmark bookmark = db.Bookmarks.Find(id);
            return View(bookmark);
        }

        public ActionResult New()
        {
            Bookmark bookmark = new Bookmark();

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
            catch (Exception e)
            {
                return View(bookmark);
            }
        }

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
            catch (Exception e)
            {
                return View(requestBookmark);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Bookmark bookmark = db.Bookmarks.Find(id);
            db.Bookmarks.Remove(bookmark);
            db.SaveChanges();
            TempData["message"] = "Bookmark deleted!";
            return RedirectToAction("Index");
        }

    }
}