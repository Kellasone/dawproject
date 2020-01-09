using MarkIT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarkIT.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Show(string id)
        {
            var user = db.Users.Find(id);
            ViewBag.user = user.UserName.Substring(0, user.UserName.IndexOf('@'));
            List<Bookmark> savedBookmarks = new List<Bookmark>();
            
            foreach (var bookmark in user.SavedBookmarks)
            {
                savedBookmarks.Add(db.Bookmarks.Find(bookmark.BookmarkId));
                    }
            ViewBag.SavedBookmarks = savedBookmarks;
            return View();
        }
    }
}