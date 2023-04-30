using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace InstantTranslationWithLocalStorage.Utilities;

public class BlazorSchoolStringLocalizer<TComponent> : IStringLocalizer<TComponent>
{
    private readonly IOptions<LocalizationOptions> _localizationOptions;
    private readonly BlazorSchoolLanguageNotifier _blazorSchoolLanguageNotifier;

    public LocalizedString this[string name] => FindLocalziedString(name);
    public LocalizedString this[string name, params object[] arguments] => FindLocalziedString(name, arguments);

    public BlazorSchoolStringLocalizer(IOptions<LocalizationOptions> localizationOptions, BlazorSchoolLanguageNotifier blazorSchoolLanguageNotifier)
    {
        _localizationOptions = localizationOptions;
        _blazorSchoolLanguageNotifier = blazorSchoolLanguageNotifier;
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        var resourceManager = CreateResourceManager();
        var result = new List<LocalizedString>();

        try
        {
            var resourceSet = resourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);
            result = resourceSet.Cast<DictionaryEntry>()
                .Select(item => new LocalizedString((string)item.Key, (string)item.Value, false, GetResourceLocaltion()))
                .ToList();
        }
        catch
        {
            result.Add(new("", "", true, GetResourceLocaltion()));
        }

        return result;
    }

    private LocalizedString FindLocalziedString(string key, object[]? arguments = default)
    {
        var resourceManager = CreateResourceManager();
        LocalizedString result;

        try
        {
            string value = resourceManager.GetString(key, _blazorSchoolLanguageNotifier.CurrentCulture);

            if (arguments is not null)
            {
                value = string.Format(value, arguments);
            }

            result = new(key, value, false, GetResourceLocaltion());
        }
        catch
        {
            result = new(key, "", true, GetResourceLocaltion());
        }

        return result;
    }

    private ResourceManager CreateResourceManager()
    {
        string resourceLocaltion = GetResourceLocaltion();
        var resourceManager = new ResourceManager(resourceLocaltion, Assembly.GetExecutingAssembly());

        return resourceManager;
    }

    private string GetResourceLocaltion()
    {
        var componentType = typeof(TComponent);
        var nameParts = componentType.FullName.Split('.').ToList();
        nameParts.Insert(1, _localizationOptions.Value.ResourcesPath);
        string resourceLocaltion = string.Join(".", nameParts);

        return resourceLocaltion;
    }
}