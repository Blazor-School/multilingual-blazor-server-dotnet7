using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace InstantTranslationWithUrl.Utilities;

public class BlazorSchoolLanguageNotifier
{
    private readonly List<ComponentBase> _subscribedComponents = new();

    public void SubscribeLanguageChange(ComponentBase component) => _subscribedComponents.Add(component);

    public void UnsubscribeLanguageChange(ComponentBase component) => _subscribedComponents.Remove(component);

    public void NotifyLanguageChange()
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