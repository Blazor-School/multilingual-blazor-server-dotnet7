using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Web;

namespace InstantTranslationWithUrl.Utilities;

public class BlazorSchoolRequestCultureProvider : RequestCultureProvider
{
    private readonly RequestLocalizationOptions _options;

    public BlazorSchoolRequestCultureProvider(RequestLocalizationOptions options)
    {
        _options = options;
    }

    public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        string url = httpContext.Request.GetDisplayUrl();
        var defaultCulture = _options.DefaultRequestCulture.Culture;
        var selectedCulture = GetCultureFromUrl(url);
        selectedCulture ??= defaultCulture;
        CultureInfo.CurrentCulture = selectedCulture;
        CultureInfo.CurrentUICulture = selectedCulture;
        var result = new ProviderCultureResult(selectedCulture.Name);

        return Task.FromResult<ProviderCultureResult?>(result);
    }

    private CultureInfo? GetCultureFromUrl(string url)
    {
        var uri = new Uri(url);
        var urlParameters = HttpUtility.ParseQueryString(uri.Query);
        var culture = CultureInfo.GetCultureInfo(urlParameters["language"] ?? "");

        return culture;
    }
}