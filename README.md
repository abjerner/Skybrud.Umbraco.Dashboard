# Skybrud.Umbraco.Dashboard 

This package can be divided and described in two seperate parts:

1. The base of this package is to provide a way to manipulate tabs and panels in dashboards in Umbraco 7 - all by using C#. Currently in Umbraco, dashboards needs to be configured in `~/config/Dashboard.config`, but this package lets you add news tabs and panels as well as minipulating existing tabs and panels in Umbraco. Since this was thought as something other packages could use as well, I have created [an issue on the Umbraco tracker](http://issues.umbraco.org/issue/U4-6557) since I believe this is something that should be in the Umbraco core.

2. Secondly, this package adds a few panels can you can add to a dashboard - eg. a panel for showing Analytics statistics for sites in your Umbraco installation. This package won't add any panels by default, so you need to do this by writing a bit of code yourself - you can find a bit of inspiration in the examples below.

## Links

- <a href="#installation">Installation</a>
- <a href="#quick-example">Quick example</a>
- <a href="#so-why-does-this-work">So why does this work?</a>
- <a href="#the-future">The Future</a>

## Installation

The code in this repository has been released as `v1.0.0-beta1`. The beta label is primarily because I hadn't testet the code thoroughly at the time of the release, but I haven't experienced any issues since the beta release.

1. [**NuGet Package**][NuGetPackage]  
Install this NuGet package in your Visual Studio project. Makes updating easy.

2. [**ZIP file**][GitHubRelease]  
Grab a ZIP file of the latest release; unzip and the files to the root directory of your website.

## Quick example

By default, the "Content" section in Umbraco 7 will have two tabs - "Get Started" and "Change Password". Let us for this demo play around with those tabs.

Given this project, we can implement the `IDashboardPlugin` interface, and use the `GetDashboard` method to do our thing.

The plugin will change the tab name from "Get Started" to "Umbraco FTW" (because why not?), and add a new tab with our custom panel (or property as called internally in Umbraco). Properties of the `DashboardDataProperty` class will be available to play around with in AngularJS ;)

```C#
public class DemoDashboardPlugin : IDashboardPlugin {
    
    public void GetDashboard(string section, List<DashboardTab> tabs) {

        // Skip if not the dashboard for "content"
        if (section != "content") return;

        // Find the "Get Started" tab
        DashboardTab getStarted = tabs.FirstOrDefault(x => x.Alias == "GetStarted");
        if (getStarted == null) return;

        // Adjust the label a bit
        getStarted.Label = "Umbraco FTW";

        // Add a new tab
        tabs.Add(new DashboardTab {
            Label = "A lot of bacon",
            Alias = "bacon",
            Properties = new List<DashboardProperty> {
                new DashboardDataProperty("Demo") {
                    Data = new {
                        a = 123,
                        b = 456,
                        c = 789
                    }
                }
            }
        });

    }
    
}
```

The plugin doesn't get picked up by it self, so we need to add it our selves. This can be done as:

```C#
public class Startup : ApplicationEventHandler {

    protected override void ApplicationStarted(UmbracoApplicationBase app, ApplicationContext ctx) {
        DashboardContext.Current.Plugins.Add(new DemoDashboardPlugin());
    }

}
```

## So why does this work?

In a normal Umbraco installation, you can only add tabs and panels by editing the `/config/Dashboard.config` file (or by writing some code, that modifies that file). When Umbraco needs to render the dashboard for the "Content" section, it will make a request to `/umbraco/backoffice/UmbracoApi/Dashboard/GetDashboard?section=content`, and the response will be an array of the tabs for that section.

However with [a little hack](https://github.com/abjerner/Skybrud.Umbraco.Dashboard/blob/master/src/Skybrud.Umbraco.Dashboard/DashboardStartup.cs#L28), we can change which URL that Umbraco will request, so Umbraco instead will make a request to our custom Dashboard WebApi controller at `/umbraco/backoffice/Skybrud/Dashboard/GetDashboard?section=content`.

## The Future

In the future, this project will also feature panels/properties that you can add to your dashboard. Have a look at the screenshot below.

While the Mailchimp and NuGet boxes are currently just mockups, the big box with the statistics pulls information directly from Google Analytics.

So when your editors log in, they could be presented with a view like this:

![An example](https://github.com/abjerner/Skybrud.Umbraco.Dashboard/blob/master/images/readme-dashboard-preview.png)


[NuGetPackage]: https://www.nuget.org/packages/Skybrud.Umbraco.Dashboard
[GitHubRelease]: https://github.com/abjerner/Skybrud.Umbraco.Dashboard/releases/latest
