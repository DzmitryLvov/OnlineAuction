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
            return View(new IndexModel()
            {
                Lots = dataAccess.GetConvertedActualLotCollection(), 
                Categories = dataAccess.GetCategoriesCollection()
            });
        }

        

        public ActionResult Index(IEnumerable<LotPreviewModel>  model)
        {
            try
            {
                return  Index(new IndexModel
                {
                    Categories = new DataAccess().GetCategoriesCollection(),
                    Lots = model
                });
            }
            catch (Exception e)
            {
                return View();
            }
        }
         public ActionResult SearchByCategory(int id)
         {
              var dataAccess = new DataAccess();
             return RedirectToAction( "Index","Home",dataAccess.SearchLotBuCategoryId(id));

         }

        public ActionResult SearchBySubCategory(int id)
        {
            return null;
        }
    }
}
