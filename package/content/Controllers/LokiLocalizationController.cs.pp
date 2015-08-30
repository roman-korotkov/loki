using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loki.Gettext;
using Loki.Resources;
using $rootnamespace$.Models;

namespace $rootnamespace$.Controllers
{
	public class LokiLocalizationController : Controller
	{
		public const string CookieName = "CurrentUICulture";

		[HttpPost]
		public ActionResult SetCulture(int id, string returnToUrl)
		{
			var cultureInfo = LokiCultureService.FindCulture(id);

			HttpContext.Response.Cookies.Set(new HttpCookie(CookieName, cultureInfo.LCID.ToString(CultureInfo.InvariantCulture))
				{
					Expires = DateTime.Now.AddYears(1)
				});

			return Redirect(returnToUrl ?? "/");
		}

		public ActionResult Index()
		{
			var cultures = LokiCultureService.Cultures.
				Select(x => new SelectListItem
				{
					Value = x.LCID.ToString(),
					Text = x.DisplayName
				});

			return View(cultures);
		}

		public ActionResult Download(int id)
		{
			var culture = LokiCultureService.FindCulture(id);
			
			var exporter = new POEporter
				{
					Name = "$assemblyname$",
					Culture = culture
				};

			exporter.AddNamespace("$rootnamespace$.Resources");

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
				var content = Loki.Gettext.PortableObject.File.ReadFrom(reader);

				var importer = new POImporter
					{
						Culture = LokiCultureService.FindCulture(content.Language)
					};
					
				content.Entries.RemoveAll(x => string.IsNullOrEmpty(x.TranslatedText));

				importer.Import(content, ResourceProviders.Editor);
			}

			return RedirectToAction("Index");
		}
	}
}