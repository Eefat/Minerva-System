using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MinervaSystem.Base.Models;

namespace MinervaSystem.Base.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            HomeViewModel viewModel = new HomeViewModel();
            //take top 5 announcements
            //viewModel.Announcements = ContextPerRequest.CurrentContext.Announcements
            //    .Where(a => !a.Expires.HasValue || DbFunctions.DiffDays(DateTime.Today, a.Expires.Value) > 0)
            //    .OrderByDescending(a => a.Created).Take(5).ToList();
            return View(viewModel);
        }
		
		public ActionResult MoreAnnouncements()
        {
            ViewBag.Announcements = ContextPerRequest.CurrentContext.Announcements
                .Where(a => !a.Expires.HasValue || DbFunctions.DiffDays(DateTime.Today, a.Expires.Value) > 0)
                .OrderByDescending(a => a.Created).ToList();
            return View();
        }
    }
}