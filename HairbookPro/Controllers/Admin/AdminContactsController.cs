using System.Linq;
using System.Web.Mvc;
using Hairbookpro.Models;

namespace Hairbookpro.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminContactsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var messages = db.ContactMessages.OrderByDescending(m => m.CreatedAt).ToList();
            return View(messages);
        }
    }
}
