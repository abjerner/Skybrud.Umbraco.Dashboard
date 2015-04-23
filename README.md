# Skybrud.Umbraco.Dashboard 

For now, this is a project that lets you add custom tabs and panels to a dashboard in Umbraco - all by using C#.

## Links

- <a href="#quick-example">Quick example</a>
- <a href="#so-why-does-this-work">So why does this work?</a>
- <a href="#the-future">The Future</a>

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

Who knows. Come back later for more ;)







