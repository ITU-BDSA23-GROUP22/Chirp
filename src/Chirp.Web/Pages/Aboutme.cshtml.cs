using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using Chirp.Core.Services;
using System.ComponentModel.DataAnnotations;
using Chirp.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using System.Text;

namespace Chirp.Web.Pages
{
    public class AboutMeModel : PageModel
    {
        public AuthorDTO? Author { get; set; } = null;

        public async Task<ActionResult> OnPost()
        {
            if (!User.Identity.IsAuthenticated) return Unauthorized();

            return Page();
        }

        private readonly IChirpService chirpService;
        private readonly IPresentationService presentationService;

        public IEnumerable<CheepDTO> Cheeps { get; set; } = null!;
        public AboutMeModel(IChirpService chirpService, IPresentationService presentationService)
        {
            this.presentationService = presentationService
                ?? throw new ArgumentNullException(nameof(presentationService));
            this.chirpService = chirpService;
        }

        public IActionResult OnPostForgetMe()
        {
            HttpContext.SignOutAsync();
            chirpService.DeleteAuthor(presentationService.GetAuthenticatedAuthor().Id);

            return RedirectToPage("/signin");
        }

        public IActionResult OnPostDownloadMyInfo()
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

            Response.Body.WriteAsync(Encoding.UTF8.GetBytes(sb.ToString()));

            return new EmptyResult();
        }
    }
}