using System.Web.Mvc;

namespace Hairbookpro.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
