using System.Reflection;
using Application;
using Application.Common.Mappings;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Persistence;
using WebMVC.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure Identity Server

builder.Services.AddControllersWithViews();
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
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();


var app = builder.Build();

// Initialize the database
// Resolve the ApplicationDbContext from the service provider
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    DbInitializer.Initialize(dbContext);
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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    // pattern: "{controller=Home}/{action=Index}/{id?}");
    pattern: "{controller=Auth}/{action=Login}/{id?}");
// app.MapControllers();

app.Run();