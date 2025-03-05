# Matomo Blazor

Matomo Blazor is a Blazor component that registers and enables the Matomo Analytics tracking code for you.

This component builds upon the [`MatomoProvider`](https://github.com/igotinfected/MatomoProvider) by `igotinfected`, enhancing it with support for custom variables and custom dimensions.

##  Setup
Add the script to the `index.html` file in the `wwwroot` folder for WebAssembly projects, or include it in the `App.razor` file in the `Components` folder for server app project. This script initializes the Matomo tracking code.
```html
<script src="_content/Matomo.Blazor/matomo.js"></script>
```

Add the `<Matomo />` component to your entry that can be `App.razor` in WebAssembly or `Router.razor` in Server. It should be added in the highest level component possible to ensure that it is loaded on every page. 
Is recommended to use an environment check to load the correct configuration for the component. This is to avoid tracking development data.
```csharp
@if (!Env.IsDevelopment())
{
<Matomo
        ApiUrl="@_apiUrl"
        SiteId="@_siteId"
        UserIdFunc="() => UserId()"
        CustomVariables="Variables"
        CustomDimensions="Dimensions"
/>
}
```
You must set the `ApiUrl` and `SiteId`. The optional `UserIdFunc`, `CustomVariables`, and `CustomDimensions` let you set a user ID, custom variables, and custom dimensions. Store `ApiUrl` and `SiteId` in the `appsettings.json` if desired:
```json
{
  "Matomo": {
    "ApiUrl": "https://your-matomo-url.com/",
    "SiteId": 1
  }
}
```

## Custom Dimensions
Dimensions must be created in the Matomo dashboard before they can be used. The index of the custom dimension must be passed to the `CustomDimensions` property. The index is the number that appears in the Matomo dashboard when you create the custom dimension. The value of the custom dimension can be set using the `CustomDimensions` property. The `Matomo` component takes an array of `CustomDimension` objects. Each `CustomDimension` object takes the index of the custom dimension and the value to be set.

```csharp
    private CustomDimension[] Dimensions { get; set; } =
    [
        new CustomDimension(1, "Value for Dimension 1")
    ];
```

## Custom Variables
In order to use the custom variables the plugin must be enabled in the matomo settings. The `Matomo` component takes an array of `CustomVariable` objects. Each `CustomVariable` object takes the index of the custom variable, the name of the custom variable, and the value to be set as well as the scope of the variable. The scope can be set to `Visit` or `Page`. The value of the custom variable can be set using the `CustomVariables` property.

```csharp
    private CustomVariable[] Variables { get; set; } =
    [
        new CustomVariable(1, "User Name", "John Smith", VariableScope.Visit)
    ];
```
## User Id
The `UserIdFunc` property is a `Func<string>` that returns a string. This function is called when the component is initialized and the return value is set as the user id for the current user. The user id can be used to track a user across multiple sessions. The user id can be set to a unique identifier for the user.

```csharp
    private Task<string> UserId()
    {
        var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        var guid = Guid.NewGuid().ToString();
        return Task.FromResult($"{guid}-{timestamp}");
    }
```


## Additional Notes
- The `compose.yml` file provides a fast way to launch a local Matomo instance for development purposes.
- This project has not been tested on the Blazor Hybrid App with interactivity set to `per page/component`. 
- While not tested, the hybrid app should work as long as the script and component is placed in the correct location depending on the interactivity setting and your project structure.



