using System.Collections.Generic;
using System.Web.Mvc;
using OnlineAuction.Buisness.Data;

namespace OnlineAuction.Buisness.Controllers
{
    public class LotController : Controller
    {
        public ActionResult Catalog()
        {
            throw new System.NotImplementedException();
        }

        public ActionResult GetLastLotsCollection()
        {
            throw new System.NotImplementedException();
        }

        public ActionResult Index(int? id = null)
        {
            var model = id != null ? DataAccess.GetLotById(id) : null;
            if (model != null)
            {
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CreateLot()
        {
            return View();
        }
    }
}
