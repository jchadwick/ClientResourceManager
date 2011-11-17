# Client Resource Manager:  A manager for your scripts and stylesheets!

## Usage

Register anywhere you have access to `HttpContext` or `HttpContextBase`:

```c#
var resourceManager = HttpContext.Current.ClientResources();

// Register Scripts:
resourceManager.IncludeScript("~/scripts/jquery.js");

// Register Stylesheets:
resourceManager.IncludeStylesheet("~/content/site.css");

// Or, just add a file name and let the manager figure out what it is:
resourceManager.Include("~/content/layout.css");
resourceManager.Include("~/scripts/underscore.js");


// Control the order that the resources get rendered by specifying an optional Level:
resourceManager.Include("~/scripts/jquery.js", Level.Global);
resourceManager.Include("~/scripts/jquery.validate.js", Level.MidLevel);
```

Then, call the `.RenderHead()` and `.Render()` methods to render the resources to the page:

```aspx-cs
<html>
<head>
	<title>Demo Page</title>
	<%= Context.ClientResources().RenderHead() %>
</head>
<body>
	<h1>Test Page!</h1>

	<%= Context.ClientResources().Render() %>
</body>
</html>
```


## ASP.NET MVC Integration

The `ClientResourceManager.Mvc` library also gives you the HtmlHelper extension:

```razor-cs
@Html.ClientResources().Include("~/scripts/jquery.js");
```


And, to render in your page...

```html
<html>
<head>
	<title>Demo Page</title>
	@Html.ClientResources().RenderHead()
</head>
<body>
	<h1>Test Page!</h1>

	@Html.ClientResources().Render()
</body>
</html>
```

