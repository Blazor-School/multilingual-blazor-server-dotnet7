using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Reflection;

namespace InstantTranslationWithUrl.Utilities;

public class BlazorSchoolLanguageNotifier
{
    public CultureInfo CurrentCulture
    {
        get => _currentCulture;

        set
        {
            if (string.IsNullOrWhiteSpace(value.Name))
            {
                _currentCulture = CultureInfo.CurrentCulture;
                NotifyLanguageChange();
            }
            else
            {
                var allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

                if (allCultures.Contains(value))
                {
                    _currentCulture = value;
                NotifyLanguageChange();
                }
            }
        }
    }

    public BlazorSchoolLanguageNotifier(IOptions<RequestLocalizationOptions> options)
    {
        _currentCulture = options.Value.DefaultRequestCulture.Culture;
    }

    private readonly List<ComponentBase> _subscribedComponents = new();
    private CultureInfo _currentCulture;

    public void SubscribeLanguageChange(ComponentBase component) => _subscribedComponents.Add(component);

    public void UnsubscribeLanguageChange(ComponentBase component) => _subscribedComponents.Remove(component);

    private void NotifyLanguageChange()
    {
        foreach (var component in _subscribedComponents)
        {
            if (component is not null)
            {
                var stateHasChangedMethod = component.GetType()?.GetMethod("StateHasChanged", BindingFlags.Instance | BindingFlags.NonPublic);
                _ = (stateHasChangedMethod?.Invoke(component, null));
            }
        }
    }
}