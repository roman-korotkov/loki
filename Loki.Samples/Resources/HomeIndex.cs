namespace Loki.Samples.Resources
{
	public class HomeIndex : Home
	{
		public string Caption = "ASP.NET";
		public string GettingStarted = "Getting started";
		public string GettingStartedText =
			@"ASP.NET MVC gives you a powerful, patterns-based way to build dynamic websites that
            enables a clean separation of concerns and gives you full control over markup
            for enjoyable, agile development.";
		public string GetMoreLibraries = "Get more libraries";
		public string GetMoreLibrariesText = "NuGet is a free Visual Studio extension that makes it easy to add, remove, and update libraries and tools in Visual Studio projects.";
		public string WebHosting = "Web Hosting";
		public string WebHostingText = "You can easily find a web hosting company that offers the right mix of features and price for your applications.";

		public HomeButtons Buttons = new HomeButtons();

		public HomeIndex()
		{
			Title = "Home Page";
		}

		public string Title { get; set; }
	}
}