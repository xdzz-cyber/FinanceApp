using System.Globalization;
using System.Reflection;
using Application;
using Application.Common.Mappings;
using Application.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using Persistence;
using Serilog;
using Serilog.Events;
using StackExchange.Redis;
using WebMVC.BackgroundJobs;
using WebMVC.Hubs;
using WebMVC.Middlewares;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
// Configure Identity Server
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources"); ;
builder.Services.AddControllersWithViews().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();
//.AddViewLocalization();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture("en");
    options.AddSupportedCultures("en", "uk");
    options.AddSupportedUICultures("en", "uk");
    options.FallBackToParentCultures = true;
    options.FallBackToParentUICultures = true;
    // options.RequestCultureProviders.Clear();
    options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
});
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    config.AddProfile(new AssemblyMappingProfile(typeof(IApplicationDbContext).Assembly));
});

builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    // options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
}).AddCookie().AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Google:ClientId"];
    options.ClientSecret = builder.Configuration["Google:ClientSecret"];
    // options.CallbackPath = "/Auth/GoogleResponse";
});
builder.Services.AddAuthorization();
builder.Services.AddSignalR();
builder.Services.AddHttpClient();
builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"));
});
builder.Services.AddHangfireServer();
// Register Redis ConnectionMultiplexer as a singleton
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = ConfigurationOptions.Parse(builder.Configuration["Redis:ConnectionString"]);
    config.ClientName = builder.Configuration["Redis:InstanceName"];
    return ConnectionMultiplexer.Connect(config);
});

// Add Serilog as the logging provider
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddLogging(loggingBuilder =>
{
    // Include Serilog as the logging provider
    loggingBuilder.AddSerilog();
});

var app = builder.Build();

// Initialize the database
// Resolve the ApplicationDbContext from the service provider
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    DbInitializer.Initialize(dbContext).GetAwaiter().GetResult();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCustomExceptionHandlerMiddleware();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseStaticFiles();
app.UseHangfireDashboard();

// Schedule a recurring job to execute every 1 minutes
RecurringJob.AddOrUpdate<HangfireRemoteApiCallJob>(x => x.MakeRemoteApiCall(), Cron.MinuteInterval(5));

app.UseRequestLocalization(app.Services.GetService<IOptions<RequestLocalizationOptions>>().Value);
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapHub<CoinsHub>("/coinsHub");
    endpoints.MapHub<BankingHub>("/bankingHub");
});

app.Run();