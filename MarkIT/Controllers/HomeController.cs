using System.Web.Mvc;

namespace MarkIT.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index","Bookmark");
        }
    }
}