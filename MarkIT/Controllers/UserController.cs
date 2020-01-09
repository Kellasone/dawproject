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
            HashSet<Category> categories = new HashSet<Category>();
            
        
            
            foreach(var bookmark in user.SavedBookmarks)
            {
                categories.Add(db.Category.Find(bookmark.CategoryId));
            }
            ViewBag.categories = categories;
            return View();
        }

        public ActionResult Saved(string id, int id2)
        {
            var userId = id;
            var categoryId = id2;

            ViewBag.catTitle = db.Category.Find(id2).Title;
            SavedBookmarks[] saved = db.SavedBookmarks.Where(m => m.CategoryId == categoryId && m.UserId == userId).ToArray();

            var user = db.Users.Find(userId);
            ViewBag.user = user.UserName.Substring(0, user.UserName.IndexOf('@'));

            List<Bookmark> listOfSavedBookmarks = new List<Bookmark>();
            foreach (var bookmark in saved)
            {
                listOfSavedBookmarks.Add(db.Bookmarks.Find(bookmark.BookmarkId));
            }
            ViewBag.SavedBookmarks = listOfSavedBookmarks;
            return View();
        }
    }
}