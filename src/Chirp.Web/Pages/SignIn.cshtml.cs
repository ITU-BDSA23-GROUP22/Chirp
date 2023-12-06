using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.web.Pages
{
    public class SignInModel : PageModel
    {
        #region Mapped Razor properties 

        [FromForm(Name = "username")]
        [Required(ErrorMessage = "username is required")]
        [MaxLength(160)]
        public string UserName { get; set; } = string.Empty;

        [FromForm(Name = "useremail")]
        [Required(ErrorMessage = "ueremail is required")]
        [MaxLength(160)]
        public string UserEmail { get; set; } = string.Empty;

        #endregion

        private readonly IWebHostEnvironment hostEnvironment;

        public SignInModel(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public void OnGet()
        {
        }

        public async Task<ActionResult> OnPostSignIn()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Local development SignIn

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, UserName),
                new Claim("emails", UserEmail),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties { IsPersistent = false, };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToPage("/public");
        }

        public async Task<ActionResult> OnGetSignOut()
        {
            // Local development SignOut

            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToPage("/public");
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            // Ensure that this page ONLY responds in Local Development

            if (!hostEnvironment.IsDevelopment())
            {
                context.Result = NotFound();
            }
        }
    }
}