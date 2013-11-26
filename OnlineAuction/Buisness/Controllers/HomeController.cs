using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using OnlineAuction.Buisness.Data;
using OnlineAuction.Buisness.Models.Home;

namespace OnlineAuction.Buisness.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View(new IndexModel() {Lots = new DataAccess().GetConvertedActualLotCollection()});
        }


        
    }
}
