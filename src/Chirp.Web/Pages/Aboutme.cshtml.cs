using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using Chirp.Core.Services;
using Microsoft.AspNetCore.Authentication;
using System.Text;

namespace Chirp.Web.Pages
{
    public class AboutMeModel : PageModel
    { 
        private readonly IPresentationService presentationService;

        public IEnumerable<AuthorDTO> Authors { get; set; } = null!;

        public AboutMeModel(IPresentationService presentationService)
        {
            this.presentationService = presentationService
                ?? throw new ArgumentNullException(nameof(presentationService));
        }

        /// <summary>
        ///     Handles Get request for authenticated Author Information
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> OnGet()
        {
            var authenticatedUser = presentationService.GetAuthenticatedAuthor();

            if (authenticatedUser == null)
            {
                return Unauthorized();
            }

            this.Authors = await presentationService.GetFollowingAuthors(authenticatedUser.Id);


            return Page();
            
        }

        /// <summary>
        ///     Handles Post request for ForgetMe functionality
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> OnPostForgetMe()
        {
            var authenticatedUser = presentationService.GetAuthenticatedAuthor();

            if (authenticatedUser == null)
            {
                return Unauthorized();
            }

            await HttpContext.SignOutAsync();

            await presentationService.AnonymizeAuthor(authenticatedUser.Id);

            return RedirectToPage("/Public");
        }

        /// <summary>
        ///     Handles Post request for DownloadMyInfo functionality
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> OnPostDownloadMyInfo()
        {

            var authenticatedUser = presentationService.GetAuthenticatedAuthor();

            if (authenticatedUser == null)
            {
                return Unauthorized();
            }

            var downloadInformation = await presentationService.GetCheepsAndFollowerDownloadForAuthor(authenticatedUser.Id);

            Response.Headers["Content-Disposition"] = "attachment;filename=information.txt";

            Response.ContentType = "text/plain";

            await Response.Body.WriteAsync(Encoding.UTF8.GetBytes(downloadInformation));

            return new EmptyResult();
        }
    }
}