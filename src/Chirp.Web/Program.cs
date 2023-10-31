using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Chirp.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add authentication



List<string> initialScopes = new List<string>
    {
        "openid",  // required for authentication
        "profile", // include additional user profile information
        // Add other scopes as needed
    };

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        options.Authority = $"{builder.Configuration["AzureADB2C:Instance"]}{builder.Configuration["AzureADB2C:Domain"]}/{builder.Configuration["AzureADB2C:SignUpSignInPolicyId"]}/v2.0";
        options.ClientId = builder.Configuration["AzureADB2C:ClientId"];
        options.ResponseType = OpenIdConnectResponseType.IdToken;
        options.UsePkce = true;
        options.CallbackPath = builder.Configuration["AzureADB2C:CallbackPath"];
        options.SignedOutCallbackPath = builder.Configuration["AzureADB2C:SignedOutCallbackPath"];
        options.TokenValidationParameters.NameClaimType = "name";
        options.Events = new OpenIdConnectEvents
        {
            OnRemoteFailure = OnAuthenticationFailed
        };
    });
// builder.Services.AddAuthorization(options =>
// {
//     // By default, all incoming requests will be authorized according to 
//     // the default policy
//     options.FallbackPolicy = options.DefaultPolicy;
// });
// builder.Services.AddRazorPages(options =>
// {
//     options.Conventions.AllowAnonymousToPage("/Index");
// })
// .AddMvcOptions(options => { })
// .AddMicrosoftIdentityUI();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<ICheepService, CheepService>();
builder.Services.AddSingleton<ICheepRepository, CheepRepository>();
builder.Services.AddDbContext<ChirpContext>();


var app = builder.Build();

using (var context = new ChirpContext())
{
    context.Database.EnsureCreated();
    DbInitializer.SeedDatabase(context);
}

static Task OnAuthenticationFailed(RemoteFailureContext context)
{
    context.Response.Redirect("/Home/Error?message=" + Uri.EscapeUriString(context.Failure.Message));
    context.HandleResponse();

    return Task.CompletedTask;
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
// app.MapControllers();

app.Run();
