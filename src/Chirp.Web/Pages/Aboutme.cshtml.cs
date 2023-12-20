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
        private readonly IChirpService chirpService;

        private readonly IPresentationService presentationService;

        public AuthorDTO? Author { get; set; } = null;

        public IEnumerable<CheepDTO> Cheeps { get; set; } = null!;

        public AboutMeModel(IChirpService chirpService, IPresentationService presentationService)
        {
            this.presentationService = presentationService
                ?? throw new ArgumentNullException(nameof(presentationService));

            this.chirpService = chirpService;
        }

        /// <summary>
        ///     Handles Get request for authenticated Author Information
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            return Page();
        }

        /// <summary>
        ///     Handles Post request for ForgetMe functionality
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> OnPostForgetMe()
        {
            await HttpContext.SignOutAsync();

            await chirpService.AnonymizeAuthor(presentationService.GetAuthenticatedAuthor().Id);

            return RedirectToPage("/signin");
        }

        /// <summary>
        ///     Handles Post request for DownloadMyInfo functionality
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> OnPostDownloadMyInfo()
        {
            AuthorDTO author = presentationService.GetAuthenticatedAuthor();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Name:");
            sb.AppendLine(author.Name);
            sb.AppendLine();
            sb.AppendLine("Followed users:");

            foreach(Guid followedUser in author.followingIds)
            {
                sb.AppendLine(followedUser.ToString());
            }

            sb.AppendLine();
            sb.AppendLine("Cheeps:");

            Response.Headers["Content-Disposition"] = "attachment;filename=information.txt";

            Response.ContentType = "text/plain";

            await Response.Body.WriteAsync(Encoding.UTF8.GetBytes(sb.ToString()));

            return new EmptyResult();
        }
    }
}