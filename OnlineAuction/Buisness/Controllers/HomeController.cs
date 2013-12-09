using System;
using System.Linq;
using System.Web.Mvc;
using OnlineAuction.Buisness.Data;
using OnlineAuction.Buisness.Models.Home;

namespace OnlineAuction.Buisness.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index(IndexModel model = null) //разобраться с поиском
        {
            var dataAccess = new DataAccess();
            if (!String.IsNullOrWhiteSpace(model.SearchQuery))
                return View(new IndexModel()
                {
                    Lots = dataAccess.SearchLotByName(model.SearchQuery)
                });
            if (model.Lots == null )
            {
                return View(new IndexModel()
                {
                    Lots = dataAccess.GetConvertedActualLotCollection().OrderBy(t =>t.ActualDate)
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

        
        
        
    }
}
