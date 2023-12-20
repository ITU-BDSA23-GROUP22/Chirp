using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Chirp.Infrastructure;

namespace Chirp.Web.Pages
{
    /// <summary>
    ///     Provides simple authentication for local development support
    /// </summary>
    public class SignInModel : PageModel
    {
        #region Mapped Razor properties 

        [BindProperty]
        [MaxLength(160)]
        public string? AuthorName { get; set; }

        [BindProperty]
        public Guid ExistingAuthorId { get; set; }

        #endregion

        private readonly IWebHostEnvironment hostEnvironment;
        private readonly ChirpDBContext dbContext;

        public List<SelectListItem> Options { get; private set; }

        public SignInModel(IWebHostEnvironment hostEnvironment, ChirpDBContext chirpDbContext)
        {
            this.hostEnvironment = hostEnvironment;
            this.dbContext = chirpDbContext;
            this.Options = new List<SelectListItem>();
        }

        /// <summary>
        ///     Handles Get request for SignIn page
        /// </summary>
        public void OnGet()
        {
            this.Options = this.dbContext.Authors.Select(x => new SelectListItem
            {
                Value = x.AuthorId.ToString(),
                Text = x.Name
            }).ToList();
        }

        /// <summary>
        ///     Handles Post request for SignIn with new Author (NewId, Name)
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> OnPostCreateSignIn()
        {
            if (string.IsNullOrWhiteSpace(this.AuthorName))
            {
                ModelState.AddModelError("AuthorName", "Author name is required");
            }

            if (this.AuthorName == null || !ModelState.IsValid)
            {
                return Page();
            }

            // Create new local development SignIn

            await SignInWith(Guid.NewGuid(), this.AuthorName);

            return RedirectToPage("/public");
        }

        /// <summary>
        ///     Handles Post request for SignIn with existing Author
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> OnPostSignIn()
        {
            // Local development SignIn

            var author = this.dbContext.Authors.Single(x => x.AuthorId == this.ExistingAuthorId);

            await SignInWith(author.AuthorId, author.Name);

            return RedirectToPage("/public");
        }

        /// <summary>
        ///     Handles Get request for SignOut of authenticated Author
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> OnGetSignOut()
        {
            // Local development SignOut

            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToPage("/public");
        }

        /// <summary>
        ///     Ensure that this page ONLY responds in Local Development     
        /// </summary>
        /// <param name="context"></param>
        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        { 
            if (!hostEnvironment.IsDevelopment())
            {
                context.Result = NotFound();
            }
        }

        private async Task SignInWith(Guid userId, string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, userName),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties { IsPersistent = false, };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}
