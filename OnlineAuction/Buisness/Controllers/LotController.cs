using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using OnlineAuction.Buisness.Data;
using OnlineAuction.Buisness.Models.Lot;

namespace OnlineAuction.Buisness.Controllers
{
    public class LotController : Controller
    {
        DataAccess dataAccess = new DataAccess();
        

        [HttpGet]
        public ActionResult Index( int id, string errormsg = null)
        {
            var cookie = new HttpCookie("name")
            {
                Name = "LotId",
                Value = id.ToString(),
                Expires = DateTime.Now.AddMinutes(100),
            };
            Response.SetCookie(cookie);

            new DataAccess().IncrementLotView(id);

            var model = dataAccess.GetViewModelById(id);

            if (errormsg != null)
            {
                ModelState.AddModelError("",errormsg);
            }
            if (model != null)
            {
                return View( model);
            }
            return RedirectToAction("Index", "Home");
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
            object image = Request.Files[0];
            if (ModelState.IsValid)
            {
                var errormsg = Auction.CreateLot(model, HttpContext.User.Identity.Name, image);
                if (String.IsNullOrWhiteSpace(errormsg))
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", errormsg);
            }
            return View(model);
        }

        public JsonResult GetSubCategories()
        {
            return Json(new DataAccess().GetSubCategoriesList(), JsonRequestBehavior.AllowGet);
        }
       
        [Authorize]
        public ActionResult DeleteLot( LotViewModel model)
        {
            var lotid = Convert.ToInt32(Request.Cookies["LotId"].Value);
            if (Auction.DeleteLot(lotid))
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("","Fail while model deleting.");
            return RedirectToAction("Index", new { id = lotid });
        }


        [Authorize]
        public ActionResult MakeBet(LotViewModel model)
        {
            var lotid = Convert.ToInt32(Request.Cookies["LotId"].Value); //TODO:убрать этот костыль
            var message = "";
            if (ModelState.IsValid)
            {
                message = new DataAccess().MakeBet(HttpContext.User.Identity.Name,lotid, model.BetValue);
                if (String.IsNullOrWhiteSpace(message))
                {
                    return RedirectToAction("Index", new{id=lotid});
                }
                model = new DataAccess().GetViewModelById(lotid);
                ModelState.AddModelError("", message);
                
            }

            return RedirectToAction("Index", new { id = lotid, @errormsg = message});
        }

        

        public ActionResult GetBets([DataSourceRequest] DataSourceRequest dsRequest)
        {
            var id = Convert.ToInt32(Request.Cookies["LotId"].Value);
            return Json(new DataAccess().GetBetCollection(id).Reverse().ToDataSourceResult(dsRequest));
        }

        [Authorize]
        public ActionResult BanComment(CommentInfo model)
        {
            new DataAccess().BanComment(model.ID);
            return RedirectToAction("Index", "Lot", new { id = model.LotID });
        }


        public ActionResult GetPreViewCollection([DataSourceRequest] DataSourceRequest dsRequest)
        {
            return Json(new DataAccess().GetConvertedActualLotCollection().ToDataSourceResult(dsRequest));
        }


        public ActionResult LeaveComment(CommentInfo model)
        {
            var id = Convert.ToInt32(Request.Cookies["LotId"].Value);
            var message = "";
            if (ModelState.IsValid)
            {
                message = new DataAccess().LeaveComment(id, HttpContext.User.Identity.Name, model.CommentText);
            }

            return RedirectToAction("Index", new { id = id, @errormsg = message });
        }

        [Authorize]
        public ActionResult RestoreLot(LotInfo lotInfo)
        {
            var lotid = Convert.ToInt32(Request.Cookies["LotId"].Value);
            var message = new DataAccess().RestoreLot(lotid);
            return RedirectToAction("Index", new { id = lotid, @errormsg=message });
        }

        [Authorize]
        public ActionResult AddToBookmarks(LotInfo lotInfo)
        {
            var lotid = Convert.ToInt32(Request.Cookies["LotId"].Value);
            var message = new DataAccess().AddToBookmarks(lotid, HttpContext.User.Identity.Name);
            return RedirectToAction("Index", new { id = lotid, @errormsg = message });
        }

        [Authorize]
        public ActionResult RemoveFromBookmarks(LotInfo lotInfo)
        {
            var lotid = Convert.ToInt32(Request.Cookies["LotId"].Value);
            var message = new DataAccess().RemoveFromBookmarks(lotid, HttpContext.User.Identity.Name);
            return RedirectToAction("Index", new { id = lotid, @errormsg = message });
        }
    }
}
