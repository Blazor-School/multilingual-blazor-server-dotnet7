using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace InstantTranslationWithCookies.Utilities;

public class BlazorSchoolRequestCultureProvider : RequestCultureProvider
{
    private readonly RequestLocalizationOptions _options;

    public BlazorSchoolRequestCultureProvider(RequestLocalizationOptions options)
    {
        _options = options;
    }

    public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        string inputCulture = httpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName] ?? "";
        var defaultCulture = _options.DefaultRequestCulture.Culture;
        var result = CookieRequestCultureProvider.ParseCookieValue(inputCulture);

        if (result is null)
        {
            CultureInfo.CurrentCulture = defaultCulture;
            CultureInfo.CurrentUICulture = defaultCulture;
            result = new(defaultCulture.Name);
        }
        else
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(result.Cultures.First().Value);
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(result.UICultures.First().Value);
        }

        return Task.FromResult<ProviderCultureResult?>(result);
    }
}