using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using OnlineAuction.Buisness.Data;
using OnlineAuction.Buisness.Models.Item;

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

        public ActionResult Index([Bind(Include = "Description")] int id)
        { var model = DataAccess.GetViewModelById(id);
            if (model != null)
            {
                return View( model);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Index(LotModel model )
        {
            if (ModelState.IsValid)
            {
                Auction.MakeBet(model, HttpContext.User.Identity.Name);
                return RedirectToAction("Index", "Lot",new {id = model.ID});
            }
            var restoreModel = DataAccess.GetViewModelById(model.ID);

            return View(restoreModel);
        }

         [Authorize]
        public ActionResult CreateLot()
        {
            return View();
        }
         [Authorize]
        [HttpPost]
        public ActionResult CreateLot(CreateLotModel model)
        {
            if (ModelState.IsValid)
            {
                if (Auction.CreateLot(model,HttpContext.User.Identity.Name))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }
        [Authorize]
        public ActionResult DeleteLot( LotModel Model)
        {
            if (Auction.DeleteLot(Model))
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("","Fail while model deleting.");
            return Index(Model);
        }
    }
}
