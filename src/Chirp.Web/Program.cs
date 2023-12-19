
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Services;
using Chirp.Core.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Chirp.Web.Filters;

namespace Chirp.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add logging to console if env = development
            if (builder.Environment.IsDevelopment())
            {
                builder.Logging.AddConsole();
            }

            using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
                .SetMinimumLevel(LogLevel.Trace)
                .AddConsole());
            var loggerDuringStartUp = loggerFactory.CreateLogger<Program>();

            if (builder.Environment.IsDevelopment())
            {
                // Configure local development authentication
                builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie();
            }
            else
            {
                // Configure azure authentication
                builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                        .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureADB2C"));
            }

            builder.Services.AddAuthorization(options =>
            {
                // By default, all incoming requests will be authorized according to the default policy
                options.FallbackPolicy = options.DefaultPolicy;
            });

            // Configure razorpages
            builder.Services.AddRazorPages(options =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    options.Conventions.AllowAnonymousToPage("/SignIn");
                }
                options.Conventions.AllowAnonymousToPage("/Public");
                options.Conventions.AllowAnonymousToPage("/UserTimeline");

            })
            .AddMvcOptions(options =>
            {
                options.Filters.Add<EnsureAuthorCreatedFilter>();
            })
            .AddMicrosoftIdentityUI();

            // Add dbContext with options
            builder.Services.AddDbContext<ChirpDBContext>(options =>
                {
                    DbContextOptionsHelper.Configure(
                        options,
                        builder.Configuration,
                        loggerDuringStartUp);
                });

            builder.Services.AddScoped<IDbContext>(x => x.GetService<ChirpDBContext>()
                ?? throw new Exception("Failed to get service ChirpDBContext for IDbContext"));

            // Adding services
            builder.Services.AddTransient<ICheepRepository, CheepRepository>();
            builder.Services.AddTransient<IAuthorRepository, AuthorRepository>();
            builder.Services.AddTransient<IChirpService, ChirpService>();
            builder.Services.AddTransient<IPresentationService, PresentationService>();

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var app = builder.Build();

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
        }
    }
}
