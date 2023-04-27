using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace InstantTranslationWithCookies.Utilities;

public class BlazorSchoolRequestCultureProvider : RequestCultureProvider
{
    public string DefaultCulture { get; set; }

    public BlazorSchoolRequestCultureProvider(string defaultCulture)
    {
        DefaultCulture = defaultCulture;
    }

    public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        string inputCulture = httpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName] ?? "";
        var result = CookieRequestCultureProvider.ParseCookieValue(inputCulture);

        if (result is null)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(DefaultCulture);
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(DefaultCulture);
            result = new(DefaultCulture);
        }
        else
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(result.Cultures.First().Value);
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(result.UICultures.First().Value);
        }

        return Task.FromResult<ProviderCultureResult?>(result);
    }
}