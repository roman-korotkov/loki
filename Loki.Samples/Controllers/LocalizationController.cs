using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loki.Gettext;
using Loki.Resources;
using Loki.Samples.Models;

namespace Loki.Samples.Controllers
{
	public class LocalizationController : Controller
	{
		public const string CookieName = "CurrentUICulture";

		[HttpPost]
		public ActionResult SetCulture(int id, string returnToUrl)
		{
			var cultureInfo = CultureService.FindCulture(id);

			HttpContext.Response.Cookies.Set(new HttpCookie(CookieName, cultureInfo.LCID.ToString(CultureInfo.InvariantCulture))
				{
					Expires = DateTime.Now.AddYears(1)
				});

			return Redirect(returnToUrl ?? "/");
		}

		public ActionResult Index()
		{
			var cultures = CultureService.Cultures.
				Select(x => new SelectListItem
				{
					Value = x.LCID.ToString(),
					Text = x.DisplayName
				});

			return View(cultures);
		}

		public ActionResult Download(int id)
		{
			var culture = CultureService.FindCulture(id);
			
			var exporter = new POEporter
				{
					Name = "loki-samples",
					Culture = culture
				};

			exporter.AddNamespace("Loki.Samples.Resources");

			var file = exporter.Export(ResourceProviders.Current);

			var stream = new MemoryStream();
			using (stream)
			using (var writer = new StreamWriter(stream){NewLine = "\n"})
			{
				file.WriteTo(writer);
			}

			var fileDownloadName = string.Format("{0}.{1}.po", file.Name, file.Language);

			return File(stream.ToArray(), "text/x-gettext-translation", fileDownloadName);
		}

		public ActionResult Upload(HttpPostedFileBase file)
		{
			using (var reader = new StreamReader(file.InputStream))
			{
				var content = Gettext.PortableObject.File.ReadFrom(reader);

				var importer = new POImporter
					{
						Culture = CultureService.FindCulture(content.Language)
					};

				importer.Import(content, ResourceProviders.Editor);
			}

			return RedirectToAction("Index");
		}
	}
}