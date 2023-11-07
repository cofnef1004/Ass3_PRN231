using BusinessObjects.CustomModel;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddDbContext<MyDbContext>();

builder.Services.AddDefaultIdentity<ApplicationUsers>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<MyDbContext>();
builder.Services.AddScoped<UserManager<ApplicationUsers>>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();



builder.Services.AddSession();

builder.Services.Configure<IdentityOptions>(options =>
{
	// Password settings.
	options.Password.RequireDigit = false;
	options.Password.RequireLowercase = true;
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequireUppercase = false;
	options.Password.RequiredLength = 6;
	options.Password.RequiredUniqueChars = 0;

	// Lockout settings.
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
	options.Lockout.MaxFailedAccessAttempts = 4;
	options.Lockout.AllowedForNewUsers = true;

	// User settings.
	options.User.AllowedUserNameCharacters =
	"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
	options.User.RequireUniqueEmail = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
	// Cookie settings
	options.Cookie.HttpOnly = true;
	options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

	options.LoginPath = "/Identity/Account/Login";
	options.AccessDeniedPath = "/Identity/Account/AccessDenied";
	options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

	var roles = new[] { "Admin", "Customer" };

	foreach (var role in roles)
	{
		var roleExist = await roleManager.RoleExistsAsync(role);
		if (!roleExist)
		{
			await roleManager.CreateAsync(new IdentityRole(role));
		}
	}
}

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var userManager = services.GetRequiredService<UserManager<ApplicationUsers>>();

	string email = configuration["Credentials:Email"];
	string password = configuration["Credentials:Password"];

	if (await userManager.FindByEmailAsync(email) == null)
	{
		var user = new ApplicationUsers
		{
			UserName = email,
			Email = email,
			FirstName = "Admin",
			LastName = "Admin"
		};
		var result = await userManager.CreateAsync(user, password);
		if (result.Succeeded)
		{
			await userManager.AddToRoleAsync(user, "Admin");
		}
	}
}

app.Run();
