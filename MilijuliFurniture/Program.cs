using AspNetCoreHero.ToastNotification;
using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using Site.DataAccess.DBConn;
using Site.DataAccess.Interface;
using Site.DataAccess.Repository;
using System.Globalization;
using PointOfSale.Utilities.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();


//Confugure Localiztion and link up with Resource folder

builder.Services.AddLocalization(o => { o.ResourcesPath = "Resources"; });
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    const string culture = "en-US";
    List<CultureInfo> supportedCultures = new List<CultureInfo> {
        new CultureInfo("en-US"),
        new CultureInfo("np-NP"),
    };

    options.DefaultRequestCulture = new RequestCulture(culture: culture, uiCulture: culture);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});



//Cookie Authentication
builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
{
	options.Cookie.Name = "MyCookieAuth";
	options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
	options.AccessDeniedPath = "/User/AccessDenied";
    options.LoginPath = "/User/Login"; // Specify the login path

    options.Events = new CookieAuthenticationEvents
    {
        OnRedirectToLogin = context =>
        {
            context.Response.Redirect(options.LoginPath);
            return Task.CompletedTask;
        }
    };
});

//Register Policy
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("MustBelongToAdmin",
		policy => policy.RequireClaim("Role", "Admin")
		);
	options.AddPolicy("MustBelongToAdminStaff",
		policy => policy.RequireClaim("Deparment", "Admin")
		);
	options.AddPolicy("MustBelongToUser",
		policy => policy.RequireClaim("Role", "User")
		);
});


// Add ToastNotification
builder.Services.AddNotyf(config =>
{
	config.DurationInSeconds = 5;
	config.IsDismissable = true;
	config.Position = NotyfPosition.BottomRight;
});
//DbConnection
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddScoped<IUserAuth, UserAuth_Repository>();
builder.Services.AddScoped<IFurnitureItems, FurnitureItems_Repository>();
builder.Services.AddScoped<ISales, SalesService_Repository>();
builder.Services.AddScoped<IReport, ReportService_Repository>();

var context = new CustomAssemblyLoadContext();
context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "Utilities/LibraryPDF/libwkhtmltox.dll"));
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddCors();
// Configure the HTTP request pipeline.

var app = builder.Build();

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});
app.UseAuthentication();


app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();
