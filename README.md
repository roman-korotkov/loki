# loki 

An easy to use ASP.NET MVC localization library that brings powerful OOP concepts into boring task of string resources creation. 
It allows using inheritance and association in order to organize your resources into logical, seamlessly maintainable structure. 
The structure that is smoothly embedded into the MVC infrastructure. 
Loki provides runtime exporting and importing facilities to and from Gettext PO files. 
That makes translation as simple as uploading file to Google Translator Toolkit and downloading it back translated automatically by Google processor or manually by a professional team.

## NuGet

The library is available as NuGet package. Run the following command in the Package Manager Console:

    Install-Package loki

The package installs fully functional sample, which illustrates usages of the most concepts. Access it with the /Loki route. The package adds the following items to the project:

### App_Start\LokiConfig.cs

Define resource configuration here. By default loki uses EF resource provider with static caching. 
Database uses LokiContext connection string. By default the database is create as Loki.mdf file in the AppData directory.
The database is empty by default so changing cultures have no effect until you download PO file translate it and upload it back.
Loki config also defines a /Loki route for demonstration. You can use chainig methods on your routes to initialize culture from route, cookie, domain, etc.

### Web.config

Loki adds LokiContext connection string to the resource database to the project's Web.config. 
When you specify an existing database, the database will be updated with loki migration. 
Use package Package Manager Console to roll it back in that case if required.

### View\Web.config

Loki sets system.web.webPages.razor\pages pageBaseType attribute to 'Loki.Mvc.LocalizableWebViewPage', but only in case the attribute has it's default value 'System.Web.Mvc.WebViewPage'.
LocalizableWebViewPage is the same as WebViewPage but it allows defining strongly typed resource for the page as another generic parameter.
Operability of all pages is not affected by this change. However now for each page you can define a resource type:

    @model MyModel

becomes

    @model MyResource, MyModel

then you access the string resource defined as properties or fields of the MyResource type on this page

    <h1>@Resources.HelloWorld</h1>

### Models\LokiCultureService

A simple implementation of culture handling logic in an application for demo purposes. Replace it with your business logic.

### Resources\LokiLocalization.cs

A sample resource type, fields or properties defines the localizable strings with default neutral culture value. 
You can use inheritance and association when define resource types. You can also add attributes with PO comments, for instance, to describe context to translator.
Here is an example:

    public class HomeIndex : Home
    {
        public string IngeritedMessage = "Inherited message";
    }
    
    [ExtractedComment("This is the general description, please use as few special terms as possible")]
    public class HomeIndex : Home
    {
        public string SomeMessage = "Some message";
        
        public Bottons Buttons = new Buttons();
    }
    
    public class Buttons
    {
        public string Next = "Next";	
        public string Prev = "Prev";
    }

### Controllers\LokiLocalizationController.cs

Represents a simple localization controller. It demonstrates changing of language and how to implement exporting and importing to and from PO files.
You can inherit you controllers from LocalizableController and specify resource type as generic argument. This way you can access our resources from controller:

    throw new Exception(Resources.MyExceptionMessage);

### Views\LokiLocalization\Index.cshtml

The only view the supports LokiLocalizationController functionality and demonstrates usages of strongly typed resources in the view.
 
## Google Translator Toolkit

https://translate.google.com/toolkit/
