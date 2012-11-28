using System.Web.Mvc;
using OnlineAuction.Buisness.Data;
using OnlineAuction.Buisness.Models.Home;

namespace OnlineAuction.Buisness.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var model = new IndexModel
                {
                    Lots = DataAccess.LotDataBase
                };
            return View(model);
        }

        
    }
}
