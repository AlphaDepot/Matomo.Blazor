using Matomo.Blazor.Dimensions;
using Matomo.Blazor.Variables;
using Microsoft.JSInterop;

namespace Matomo.Blazor;

/// <summary>
/// Provides methods to interact with the Matomo JavaScript API.
/// </summary>
public static class MatomoProvider
{
    /// <summary>
    ///  Initializes the Matomo tracking by calling the initialize method in the JavaScript API.
    /// </summary>
    /// <param name="jsRuntime">The JavaScript runtime.</param>
    /// <param name="apiUrl">The URL of the Matomo API.</param>
    /// <param name="siteId">The site ID.</param>
    /// <param name="initialPage">The initial page onload.</param>
    /// <param name="userId">The id of the current user.</param>
    /// <param name="variables">An array of optional custom variables.</param>
    /// <param name="dimensions">An array of optional custom dimensions.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    public  static ValueTask Initialize(IJSRuntime jsRuntime, string apiUrl, int siteId, string initialPage, string userId, CustomVariable[]? variables = null, CustomDimension[]? dimensions = null)
    {
        return   jsRuntime.InvokeVoidAsync("MatomoProvider.initialize", apiUrl, siteId, initialPage, userId, variables, dimensions);
    }

    /// <summary>
    ///  Reports updated page info to the Matomo API whenever a page changes, making sure the JavaScript API sends it.
    /// </summary>
    /// <param name="jsRuntime">The JavaScript runtime.</param>
    /// <param name="referrerUrl">The URL of the previous page.</param>
    /// <param name="currentUrl">The URL of the current page.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    public static ValueTask TriggerPageChange(IJSRuntime jsRuntime, string referrerUrl, string currentUrl)
    {
        return jsRuntime.InvokeVoidAsync("MatomoProvider.triggerPageChange", referrerUrl, currentUrl);
    }

    /// <summary>
    ///  Resets the user ID in the Matomo API.
    /// </summary>
    /// <remarks>
    ///  Currently, this method is not used in the Matomo.Blazor library.
    /// </remarks>
    /// <param name="jsRuntime">The JavaScript runtime.</param> 
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    public static ValueTask ResetUserId(IJSRuntime jsRuntime)
    {
        return jsRuntime.InvokeVoidAsync("MatomoProvider.resetUserId");
    }
    
}