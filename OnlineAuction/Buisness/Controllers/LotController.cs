﻿using System;
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
        


        public ActionResult Index( int id)
        {
            var cookie = new HttpCookie("name")
            {
                Name = "LotId",
                Value = id.ToString(),
                Expires = DateTime.Now.AddMinutes(100),
            };
            Response.SetCookie(cookie);


            var model = dataAccess.GetViewModelById(id);
            if (model != null)
            {
                return View( model);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public ActionResult Index(LotViewModel model )
        {
            if (ModelState.IsValid)
            {
                 var lotid = Convert.ToInt32(Request.Cookies["LotId"].Value);

                new DataAccess().LeaveComment(lotid, HttpContext.User.Identity.Name, model.CommentText);

                return RedirectToAction("Index", "Lot",new {id = lotid});
            }

            var restoreModel = dataAccess.GetViewModelById(model.Model.ID);

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
            //object image = Request.Files[0];
            if (ModelState.IsValid)
            {
                if (Auction.CreateLot(model,HttpContext.User.Identity.Name))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("","");
            return View(model);
        }

        public JsonResult GetSubCategories()
        {
            return Json(new DataAccess().GetSubCategoriesList(), JsonRequestBehavior.AllowGet);
        }
       
        [Authorize]
        public ActionResult DeleteLot( LotViewModel Model)
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

        [Authorize]
        public ActionResult MakeBet(BetInfo Model)
        {
            var lotid = Convert.ToInt32(Request.Cookies["LotId"].Value); //TODO:убрать этот костыль
            new DataAccess().MakeBet(HttpContext.User.Identity.Name,lotid, Model.BetValue);
            return Json(ModelState.ToDataSourceResult());
        }

        public ActionResult GetBets([DataSourceRequest] DataSourceRequest dsRequest)
        {
            var id = Convert.ToInt32(Request.Cookies["LotId"].Value);
            return Json(new DataAccess().GetBetCollection(id).Reverse().ToDataSourceResult(dsRequest));
        }

        [Authorize]
        public ActionResult BanComment(BetInfo model)
        {
            throw new NotImplementedException();
        }    
    }
}
