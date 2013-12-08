using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using OnlineAuction.Buisness.Data;
using OnlineAuction.Buisness.Models.Home;
using OnlineAuction.Buisness.Models.Lot;

namespace OnlineAuction.Buisness.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index(IndexModel model = null)
        {
            var dataAccess = new DataAccess();
            if (model.Lots == null)
            {
                return View(new IndexModel()
                {
                    Lots = dataAccess.GetConvertedActualLotCollection()
                });
            }
            else
            {
                return View(model);
            }
            
        }

        
        
         public ActionResult SearchByCategory(int id)
         {
             var data = new DataAccess();
             var model = new IndexModel()
             {
                 Lots = data.SearchLotBuCategoryId(id)
             };
             return View("Index", model);
         }

        public ActionResult SearchBySubCategory(int id)
        {
            var data = new DataAccess();
            var model = new IndexModel()
            {
                Lots = data.SearchLotBySubCategoryId(id)
            };
            return View("Index", model);
        }

        public ActionResult SearchByName(string query)
        {
            throw new NotImplementedException();
        }
    }
}
