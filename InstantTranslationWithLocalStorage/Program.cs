using InstantTranslationWithLocalStorage.Utilities;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddLocalization(options => options.ResourcesPath = "BlazorSchoolResources");
builder.Services.AddScoped<BlazorSchoolLanguageNotifier>();
builder.Services.AddScoped(typeof(IStringLocalizer<>), typeof(BlazorSchoolStringLocalizer<>));
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture("fr");
    options.AddSupportedCultures(new[] { "en", "fr" });
    options.AddSupportedUICultures(new[] { "en", "fr" });
});

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseRequestLocalization();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
