using System.Web.Mvc;
using OnlineAuction.Buisness.Data;
using OnlineAuction.Buisness.Models.Home;

namespace OnlineAuction.Buisness.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            var model = new IndexModel();
            return View(model);
        }

        
    }
}
