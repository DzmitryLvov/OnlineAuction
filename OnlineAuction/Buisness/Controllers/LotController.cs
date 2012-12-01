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
        
        public ActionResult Index(int? id = null)
        {
            var model = id != null ? DataAccess.GetViewModelById(id) : null;
            if (model != null)
            {
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Index(ViewLotModel model )
        {
            if (ModelState.IsValid)
            {
                Auction.MakeBet(model, HttpContext.User.Identity.Name);
                return RedirectToAction("Index", "Lot",new {id = model.ID});
            }
            var restoreModel = DataAccess.GetViewModelById(model.ID);

            return View(restoreModel);
        }


        public ActionResult CreateLot()
        {
            return View();
        }

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
            return CreateLot(model);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteLot(string leadername)
        {
            /*if (Auction.DeleteLot(id, leadername))
            {
                RedirectToAction("Index", "Home");
            }*/
                return Index();
            
        }
    }
}
