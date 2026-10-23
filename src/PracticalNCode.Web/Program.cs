using Microsoft.AspNetCore.Authentication.Cookies;
using PracticalNCode.Application.Abstractions.Repositories;
using PracticalNCode.Application.Abstractions.Services;
using PracticalNCode.Application.Services;
using PracticalNCode.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add MVC Controllers and Views
builder.Services.AddControllersWithViews();

// Register Database Connection Factory
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
builder.Services.AddScoped<IDbConnectionFactory>(_ => new SqlConnectionFactory(connectionString));

// Register Repositories (Dapper)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();


// Register Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAdminService, AdminService>();


// Configure Session (strictly 5 minutes timeout)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure Cookie Authentication (strictly 5 minutes expiration)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

var app = builder.Build();

// Ensure initial admin account if database is available
try
{
    using var scope = app.Services.CreateScope();
    var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
    await authService.EnsureInitialAdminExistsAsync();
}
catch
{
    // Ignore database connectivity errors during initial startup
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
