using Matomo.Blazor.Dimensions;
using Matomo.Blazor.Variables;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;


namespace Matomo.Blazor;

public partial class Matomo : IDisposable
{
    [CascadingParameter]
    private Task<AuthenticationState>? AuthenticationStateTask { get; set; }

    [Inject]
    private IJSRuntime JsRuntime { get; set; } = null!;

    [Inject]
    private ILogger<Matomo> Logger { get; set; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;



    [Parameter, EditorRequired]
    public string ApiUrl { get; set; } = null!;

    [Parameter, EditorRequired]
    public int SiteId { get; set; }

    [Parameter] public CustomVariable[]? CustomVariables { get; set; } 
    
    [Parameter]
    public CustomDimension[]? CustomDimensions { get; set; }

    [Parameter]
    public Func<Task<string>>? UserIdFunc { get; set; }

    private string _referrerUrl = string.Empty;

    

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        NavigationManager.LocationChanged -= OnLocationChanged;
        NavigationManager.LocationChanged += OnLocationChanged;
    }
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var userId = await GetUserId();
            var url = NavigationManager.Uri;
            
            
            try
            {
                await MatomoProvider.Initialize(JsRuntime, ApiUrl, SiteId, url, userId, CustomVariables,
                    CustomDimensions);
            }
            catch (JSException jsException)
            {
                var message = jsException.Message.Contains("undefined", StringComparison.OrdinalIgnoreCase)
                    ? "MatomoProvider JS object is undefined. Check your ApiUrl and ensure your Matomo instance is reachable."
                    : "Matomo JSInterop error";
                
                Logger.LogError(jsException, message);
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "An error occurred while initializing the Matomo provider");
            }
        }
        
        await base.OnAfterRenderAsync(firstRender);
    }
    
    /// <summary>
    ///  Triggers a page change event when the location changes.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="args">The location changed event arguments.</param>
    private async void OnLocationChanged(object? sender, LocationChangedEventArgs args)
    {
        try
        {
            await MatomoProvider.TriggerPageChange( JsRuntime,_referrerUrl, args.Location);
        }
        catch (Exception exception)
        {
            Logger.LogWarning(exception, "An error occurred while triggering a page change");
        }
        finally
        {
            _referrerUrl = args.Location;
        }
    }

    /// <summary>
    ///  Gets the user id from the provided function.
    /// </summary>
    /// <returns>The user id.</returns>
    private async Task<string> GetUserId()
    {
        var userId = string.Empty;

        if (UserIdFunc is not null)
        {
            userId = await UserIdFunc();
        }
        
        return userId;
    }
    
    
    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
        GC.SuppressFinalize(this);
    }
}