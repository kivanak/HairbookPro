using System.Linq;
using System.Web.Mvc;
using Hairbookpro.Models;

namespace Hairbookpro.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDeletedCommentsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var comments = db.DeletedCommentLogs
                .OrderByDescending(c => c.DeletedAt)
                .ToList();

            return View(comments);
        }
    }
}