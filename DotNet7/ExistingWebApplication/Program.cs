using BlazorWasm;
using ExistingWebApplication.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/clientapp"), app1 =>
{
    
    app1.UseBlazorFrameworkFiles("/clientapp");
    app1.UseRouting();
    app1.UseEndpoints(endpoints =>
    {
        //endpoints.MapControllers();
        endpoints.MapFallbackToFile("/{*path:nonfile}", "/index.html");
    });
    app1.UsePathBase("/clientapp");
    app1.UseStaticFiles("");
});
app.UseWebAssemblyDebugging();
app.MapFallbackToFile("index.html");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
