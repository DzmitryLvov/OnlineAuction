using System.Runtime.Remoting.Messaging;
using System.Web.Mvc;

namespace OnlineAuction.Buisness.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Error404()
        {
            return View();
        }
    }
}