using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using Microsoft.AspNetCore.Authorization;
using Chirp.Core.Services;
namespace Chirp.Razor.Pages;

[AllowAnonymous]
public class UserTimelineModel : PageModel
{
    #region Mapped Razor properties 

    [BindProperty(SupportsGet = true, Name = "author")]
    public Guid AuthorId { get; set; }

    [FromQuery(Name = "page")]
    public string page { get; set; } = null!;

    #endregion
    private readonly IChirpService chirpService;

    public AuthorDTO? Author { get; set; } = null;

    public IEnumerable<CheepDTO> Cheeps { get; set; } = null!;

    public UserTimelineModel(IChirpService chirpService)
    {
        this.chirpService = chirpService;
    }

    public async Task<ActionResult> OnGet(string author)
    {
        this.Author = await chirpService.GetAuthor(AuthorId);
        //-----------
        this.Cheeps = await chirpService.GetCheepsByAuthor(AuthorId, this.GetPageNumber());
        return Page();
    }

    private int GetPageNumber()
    {
        if (int.TryParse(this.page, out var pageNumber))
        {
            return pageNumber;
        }
        return 1;
    }

}
