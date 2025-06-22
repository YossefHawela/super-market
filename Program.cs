using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuperMarket.Data;
using SuperMarket.Filters;
using SuperMarket.Middleware;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddTransient<DataConnector>(); // Register DataConnector as a singleton service

builder.Services.AddAuthentication("ToDoAuthenCookie")
    .AddCookie("ToDoAuthenCookie", options =>
    {
        options.LoginPath = "/Account/Login"; // Set the login path
        options.AccessDeniedPath = "/Account/AccessDenied"; // Set the access denied path
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set the cookie expiration time
        options.SlidingExpiration = true; // Enable sliding expiration
        options.LogoutPath = "/Account/Logout"; // Set the logout path
        options.ReturnUrlParameter = "returnUrl"; // Set the return URL parameter name
    });


builder.Services.AddScoped<LogActionFilter>();

builder.Services.AddAuthorization();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<PreventSqlInjectionMiddleware>();
builder.Services.AddScoped<PreventXssMiddleware>();

builder.Services.AddRazorPages();


var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    builder.Services.FeedDb(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();

app.UseMiddleware<PreventSqlInjectionMiddleware>();
app.UseMiddleware<PreventXssMiddleware>();


app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

