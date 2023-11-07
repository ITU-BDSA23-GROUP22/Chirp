using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Chirp.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;


var builder = WebApplication.CreateBuilder(args);

// Add authentication

// Add services to the container.
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

// builder.Services.AddRazorPages();
// builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();
// builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
//     .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureADB2C"));
// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("Admin", policy =>
//     {
//         policy.RequireClaim("jobTitle", "Admin");
//     });
// });
builder.Services.AddSingleton<ICheepService, CheepService>();
builder.Services.AddSingleton<ICheepRepository, CheepRepository>();
builder.Services.AddDbContext<ChirpContext>();


var app = builder.Build();

using (var context = new ChirpContext())
{
    context.Database.EnsureCreated();
    DbInitializer.SeedDatabase(context);
}

// static Task OnAuthenticationFailed(RemoteFailureContext context)
// {
//     context.Response.Redirect("/Home/Error?message=" + Uri.EscapeUriString(context.Failure.Message));
//     context.HandleResponse();

//     return Task.CompletedTask;
// }
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

// app.UseRewriter(
//     new RewriteOptions().Add(
//         context =>
//         {
//             if (context.HttpContext.Request.Path == "/MicrosoftIdentity/Account/SignedOut")
//             {
//                 context.HttpContext.Response.Redirect("/");
//             }
//         }
//     )
// );


app.MapRazorPages();
app.MapControllers();

app.Run();
