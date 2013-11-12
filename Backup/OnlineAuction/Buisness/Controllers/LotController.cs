using System;
using System.Web.Mvc;
using OnlineAuction.Buisness.Data;
using OnlineAuction.Buisness.Models.Home;
using OnlineAuction.Buisness.Models.Lot;

namespace OnlineAuction.Buisness.Controllers
{
    public class LotController : Controller
    {
        DataAccess dataAccess = new DataAccess();
        public ActionResult Catalog()
        {
            return View(new CatalogModel());
        }


        public ActionResult Index( int id)
        { var model = dataAccess.GetViewModelById(id);
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
            var restoreModel = dataAccess.GetViewModelById(model.ID);

            return View(restoreModel);
        }

        [Authorize]
        public ActionResult CreateLot()
        {
            return View( new CreateLotModel());
        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateLot(CreateLotModel model)
        {
            object image = Request.Files[0];
            if (ModelState.IsValid)
            {
                if (Auction.CreateLot(model,HttpContext.User.Identity.Name, image))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("","");
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

        public ActionResult Hot()
        {
            return View(new HotCatalogModel());
        }
    }
}
