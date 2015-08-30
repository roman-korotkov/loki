@using System.Threading
@using $rootnamespace$.Models
@using $rootnamespace$.Resources

@model LokiLocalization, IEnumerable<SelectListItem>

@{
	Layout = null;

	ViewBag.Title = @Resources.Title;
}

<!DOCTYPE html>

<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@ViewBag.Title</title>
    @Styles.Render("~/Content/css")
    @Scripts.Render("~/bundles/modernizr")
</head>
<body>
    <div class="navbar navbar-inverse navbar-fixed-top">
        <div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                @Html.ActionLink(@Resources.Title, "Index", "LokiLocalization", null, new { @class = "navbar-brand" })
            </div>
	        <div class="navbar-collapse collapse">
		        <ul class="nav navbar-nav navbar-right">
			        @foreach (var culture in LokiCultureService.Cultures)
			        {
				        <li>
					        @using (Html.BeginForm("SetCulture", "LokiLocalization", new { id = culture.LCID, returnToUrl = Request.RawUrl }, FormMethod.Post))
							{
								var style = Equals(Thread.CurrentThread.CurrentUICulture, culture) ? "btn btn-primary" : "btn";

								<button class="@style">@culture.Name</button>
							}
				        </li>
			        }
		        </ul>
	        </div>
        </div>
    </div>
    <div class="container body-content">

		<h2>@Resources.Title</h2>

		<table class="table table-striped">
			<tr>
				<th>
					@Resources.AvailableCultures
				</th>
			</tr>
			@foreach (var item in Model)
			{
				<tr>
					<td>
						<a title="@Resources.DownloadPOFile" href="@Url.Action("Download", new { id = item.Value })">
							@item.Text
						</a>
					</td>
				</tr>
			}
			<tr>
				<td>
					@using (Html.BeginForm("Upload", "LokiLocalization", FormMethod.Post, new { enctype = "multipart/form-data" }))
					{
						<label for="file" class="control-label">@Resources.UploadPOFile</label>
						<div class="input-group">
							<input id="file" type="file" name="file" class="form-control" />
							<input type="submit" value="@Resources.Upload" class="btn btn-primary" />
						</div>
					}
				</td>
			</tr>
		</table>

        <hr />
        
		<footer>
	        <p>
		        &copy; @DateTime.Now.Year - $assemblyname$<br/>
	        </p>
        </footer>
    </div>

    @Scripts.Render("~/bundles/jquery")
    @Scripts.Render("~/bundles/bootstrap")
</body>
</html>
