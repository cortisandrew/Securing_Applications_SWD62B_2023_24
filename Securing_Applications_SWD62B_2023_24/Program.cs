using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Securing_Applications_SWD62B_2023_24.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

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
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

    if (roleManager == null || userManager == null)
    {
        // you have a problem with setting up the services
        // Log this information
        // Shutdown everything
        return;
    }

    const string ADMIN_ROLE = "Admin";
    const string DEFAULT_ADMIN_EMAIL = "defaultadmin@mysite.com";
    const string DEFAULT_PASS = "SecretPass123!"; // Don't use passwords you find as examples!

    bool adminExists = roleManager.RoleExistsAsync(ADMIN_ROLE).Result;

    if (!adminExists)
    {
        roleManager.CreateAsync(new IdentityRole(ADMIN_ROLE)).Wait();
    }

    var adminUser = userManager.FindByEmailAsync(DEFAULT_ADMIN_EMAIL).Result;

    if (adminUser == null) { // admin user does not exist
        var createdAdminUserResult = userManager.CreateAsync(
            new ApplicationUser() { UserName = DEFAULT_ADMIN_EMAIL, Email = DEFAULT_ADMIN_EMAIL },
            DEFAULT_PASS).Result;
    
        if (!createdAdminUserResult.Succeeded)
        {
            // issue setting up admin
            // Log this information
            // Shutdown everything
            return;
        }

        adminUser = userManager.FindByEmailAsync(DEFAULT_ADMIN_EMAIL).Result;
    }

    if (!userManager.IsInRoleAsync(adminUser, ADMIN_ROLE).Result)
    {
        var addIdentityRoleResult = userManager.AddToRoleAsync(adminUser, ADMIN_ROLE).Result;
    }

    
}

app.Run();
