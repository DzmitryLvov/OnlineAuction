using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using OnlineAuction.Buisness.Data;

namespace OnlineAuction.Buisness.Controllers
{
    public class AdministrationController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetUnapprovedComments([DataSourceRequest] DataSourceRequest dsRequest)
        {
            return Json(new DataAccess().GetUnApprovedComments().ToDataSourceResult(dsRequest), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetusersByRoles([DataSourceRequest] DataSourceRequest dsRequest)
        {
            return Json(new DataAccess().GetUsersRolesInfo().ToDataSourceResult(dsRequest), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetLotsLocations([DataSourceRequest] DataSourceRequest dsRequest)
        {
            return Json(new DataAccess().GetLotsLocations().ToDataSourceResult(dsRequest), JsonRequestBehavior.AllowGet);
        }
        public ActionResult LotsByUsers([DataSourceRequest] DataSourceRequest dsRequest)
        {
            return Json(new DataAccess().GetLotsByUsers().ToDataSourceResult(dsRequest), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUsersLotCount([DataSourceRequest] DataSourceRequest dsRequest)
        {
            return Json(new DataAccess().GetUsersLotCount().ToDataSourceResult(dsRequest), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetActiveUsers([DataSourceRequest] DataSourceRequest dsRequest)
        {
            return Json(new DataAccess().GetActiveUsers().ToDataSourceResult(dsRequest), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetLotsCategories([DataSourceRequest] DataSourceRequest dsRequest)
        {
            return Json(new DataAccess().GetLotsCategories().ToDataSourceResult(dsRequest), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ClearComments()
        {
            new DataAccess().ClearComments();
            return View("Index");
        }
    }
}