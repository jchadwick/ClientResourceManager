# Client Resource Manager:  A manager for your scripts and stylesheets!

## Usage

### Registering Resources

Register resources for your page anywhere you have access to `HttpContext` or `HttpContextBase`:

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

### Rendering Resources

There are two ways to render the resources in your page: The Awesome Way, or The Performant Way.
We recommend The Awesome Way, unless you are super-performance-minded.

#### The Awesome Way
The Awesome Way to include the resources in your page is to let the Client Resource Manager framework figure out where to inject everything.

In order to use The Awesome Way, simply register the `ClientResourceManager.Module` module in your web.config:

```xml
<modules>
	<add name="ClientResources" type="ClientResourceManager.Module"/>
</modules>
```

The module will then inspect all of your outgoing requests and inject the required client resource markup where appropriate.


#### The Performant Way

If you don't like the idea of all those CPU cycles wasted trying to figure out where to put the client resources markup and want to take matters into your own hands, then The Performant Way is for you.

To go this Way, make sure the model declaration above is removed, then call the `.RenderHead()` and `.Render()` methods to render the resources to the page:

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

