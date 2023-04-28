using InstantTranslationWithUrl.Utilities;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddLocalization(options => options.ResourcesPath = "BlazorSchoolResources");
builder.Services.AddScoped<BlazorSchoolLanguageNotifier>();
builder.Services.AddScoped(typeof(IStringLocalizer<>), typeof(BlazorSchoolStringLocalizer<>));
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    _ = options.AddSupportedCultures(new[] { "en", "fr" });
    _ = options.AddSupportedUICultures(new[] { "en", "fr" });
    options.RequestCultureProviders = new List<IRequestCultureProvider>()
        {
            new BlazorSchoolRequestCultureProvider("en")
        };
});

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseRequestLocalization();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
