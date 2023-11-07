using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Chirp.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add authentication
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureADB2C"));
builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to 
    // the default policy
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AllowAnonymousToPage("/Public");
})
.AddMvcOptions(options => { })
.AddMicrosoftIdentityUI();

builder.Services.AddSingleton<ICheepRepository, CheepRepository>();
builder.Services.AddTransient<ChirpContext>();


var app = builder.Build();

using (var context = new ChirpContext())
{
    context.Database.EnsureCreated();
    DbInitializer.SeedDatabase(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
