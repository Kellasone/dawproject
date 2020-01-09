using MarkIT.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;

namespace MarkIT.Controllers
{
	
    public class BookmarkController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int _perPage = 6;

        // GET: Bookmark
        public ActionResult Index()
        {
			var bookmarks = db.Bookmarks.Include("User").OrderByDescending(a => a.Id);

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
            ViewBag.Username = bookmark.User.UserName.Substring(0, bookmark.User.UserName.IndexOf('@'));

            if (User.Identity.GetUserId() == bookmark.UserId || User.IsInRole("Administrator"))
                ViewBag.afisareButoane = true;
            else
                ViewBag.afisareButoane = false;


			bool voted = false;
            bool saved = false;
            foreach (var vote in bookmark.Votes)
                if (vote.UserId == User.Identity.GetUserId())
                    voted = true;
            string user = User.Identity.GetUserId();
            SavedBookmarks[] listOfSavedBookmarks = db.SavedBookmarks.Where(m => m.UserId == user).ToArray();
            foreach(var itemSavedBookmark in listOfSavedBookmarks)
            {
                if (itemSavedBookmark.BookmarkId == id)
                   saved = true;
            }

            ViewBag.Voted = voted;
            ViewBag.Saved = saved;
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
			  ViewBag.Bookmark = bookmark; 
			  return View(bookmark);
			 
        }

		[HttpPost]
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
                    return RedirectToAction("Show/" + requestBookmark.Id);
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
		public ActionResult DeleteComment(int id)
		{
			Comment comment = db.Comments.Find(id);
			db.Comments.Remove(comment);
			db.SaveChanges();
			return RedirectToAction("/Show/"+comment.BookmarkId);
		}

		[Authorize(Roles = "User, Administrator")]
        public ActionResult PersonalBookmarks()
		{
            try
            {
                string user = User.Identity.GetUserId();

                var bookmarks = db.Bookmarks.Where(m => m.UserId == user).OrderByDescending(a => a.Id);

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
            }
            catch(Exception)
            {
                ViewBag.total = 0;
            }
			
            
			return View();
		}


        public ActionResult Search(string search)
        {
            try
            {
                //var bookmarks = db.Bookmarks.SqlQuery("Select * from Bookmarks where Contains(title, @word)", new SqlParameter("@word", search));
                
                var bookmarks = db.Bookmarks.Where(m => m.Title.Contains(search) || m.Description.Contains(search) || m.Tags.Contains(search)).OrderByDescending(a => a.Id);

                var totalItems = bookmarks.Count();
                var currentPage = Convert.ToInt32(Request.Params.Get("Page"));
                var offset = 0;

                if (!currentPage.Equals(0))
                {
                    offset = (currentPage - 1) * this._perPage;
                }
                var paginatedBookmarks = bookmarks.Skip(offset).Take(this._perPage);

                ViewBag.perPage = this._perPage;
                ViewBag.total = totalItems;
                ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)this._perPage);
                ViewBag.Bookmarks = paginatedBookmarks;
            }
            catch(Exception)
            {
                ViewBag.total = 0;
            }

            return View(); 
        }

		public ActionResult Vote(int id)
		{
			Vote vote = new Vote();
			vote.BookmarkId = id;
			vote.UserId = User.Identity.GetUserId();
			db.Votes.Add(vote);
			db.SaveChanges();
			return RedirectToAction("Show/" + id);
		}

		public ActionResult Unvote(int id)
		{
			
			Bookmark bookmark = db.Bookmarks.Find(id);
			foreach (Vote vote in bookmark.Votes)
				if (vote.UserId == User.Identity.GetUserId())
				{ db.Votes.Remove(vote);
					db.SaveChanges();
					break;
				}
			
			return RedirectToAction("Show/" + id);
		}

		public ActionResult AddComment()
		{
			Comment comment = new Comment();
			return View(comment);
		}

		[HttpPost]
		public ActionResult AddComment(Comment comment)
		{
			
			comment.UserName = User.Identity.GetUserName();
			db.Comments.Add(comment);
			db.SaveChanges();
			return RedirectToAction("/show/"+comment.BookmarkId);
		}

        public ActionResult SaveBookmark (int id)
        {
			// SavedBookmarks savedBookmark = new SavedBookmarks();
			//savedBookmark.BookmarkId = id;
			//savedBookmark.UserId = User.Identity.GetUserId();
			// db.SavedBookmarks.Add(savedBookmark);
			// db.SaveChanges();
			// return RedirectToAction("/show/" + id);
			string user = User.Identity.GetUserId();
			ViewBag.Categories = db.Category.Where(m => m.UserId == user);
			
			SavedBookmarks savedBookmark = new SavedBookmarks();
			
			ViewBag.BookmarkId = id.ToString();
			return View(savedBookmark);

        }
		[HttpPost]
		public ActionResult SaveBookmark(SavedBookmarks savedBookmarks)
		{
			
			db.SavedBookmarks.Add(savedBookmarks);
			db.SaveChanges();
			return RedirectToAction("/show/" + savedBookmarks.BookmarkId);
		}



		public ActionResult DeleteSavedBookmark(int id)
        {
            
            int bookmarkId = id;
            string userId = User.Identity.GetUserId();

            SavedBookmarks[] listofUserSavedBookmarks = db.SavedBookmarks.Where(m => m.UserId == userId).ToArray();

            foreach(var savedBookmark in listofUserSavedBookmarks)
            {
                if (savedBookmark.BookmarkId == bookmarkId)
                    db.SavedBookmarks.Remove(savedBookmark);
            }
            
            db.SaveChanges();
            return RedirectToAction("/show/" + id);
        }

     

    }
}