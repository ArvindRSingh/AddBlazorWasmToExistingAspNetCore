using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/clientapp"), app1 =>
{
    app1.UseBlazorFrameworkFiles("/clientapp");
    app1.UseRouting();
    app1.UseEndpoints(endpoints =>
    {
        //endpoints.MapControllers();
        endpoints.MapFallbackToFile("/clientapp/{*path:nonfile}", "/clientapp/index.html");
    });
    //app1.UsePathBase("/clientapp");
    app1.UseStaticFiles();
    app1.UseStaticFiles("/clientapp");
});
app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
