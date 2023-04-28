using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Web;

namespace InstantTranslationWithUrl.Utilities;

public class BlazorSchoolRequestCultureProvider : RequestCultureProvider
{
    private readonly string _defaultLanguage;
    private string? _selectedLanguage;

    public BlazorSchoolRequestCultureProvider(string defaultLanguage)
    {
        _defaultLanguage = defaultLanguage;
    }

    public override Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        if (httpContext.Request.Headers["Sec-Fetch-Dest"] == "document")
        {
            string url = httpContext.Request.GetDisplayUrl();
            _selectedLanguage = GetLanguageFromUrl(url);

            if (string.IsNullOrEmpty(_selectedLanguage))
            {
                _selectedLanguage = _defaultLanguage;
            }

            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(_selectedLanguage);
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(_selectedLanguage);
            var result = new ProviderCultureResult(_selectedLanguage);

            return Task.FromResult<ProviderCultureResult?>(result);
        }
        else
        {
            return Task.FromResult<ProviderCultureResult?>(new ProviderCultureResult(_selectedLanguage));
        }
    }

    private string? GetLanguageFromUrl(string url)
    {
        var uri = new Uri(url);
        var urlParameters = HttpUtility.ParseQueryString(uri.Query);

        return urlParameters["language"];
    }
}