using Loki.Gettext;

namespace Loki.Samples.Resources
{
	[ExtractedComment("This is the general description, please use as few special terms as possible")]
	public class Home
	{
		public string IndexMessage = "ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS and JavaScript.";
		public string AboutMessage = "Your application description page.";
		public string ContactMessage = "Your contact page.";

		public HomeErrors Errors = new HomeErrors();
	}
}