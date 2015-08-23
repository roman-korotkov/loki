using System;
using System.Web.Mvc;
using Loki.Mvc;
using Loki.Samples.Resources;

namespace Loki.Samples.Controllers
{
	public class HomeController : LocalizableController<Home>
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = Resources.AboutMessage;

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = Resources.ContactMessage;

			return View();
		}

		public ActionResult Error()
		{
			throw new ApplicationException(Resources.Errors.SomethingBadHappaned);
		}
	}
}