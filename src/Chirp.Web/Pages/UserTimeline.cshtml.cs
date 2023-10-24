using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _service;
    public IEnumerable<CheepDTO> Cheeps { get; set; } = null!;
    [FromQuery(Name = "page")]
    public string page { get; set; } = null!;

    public UserTimelineModel(ICheepRepository service)
    {
        _service = service;
    }

    public ActionResult OnGet(string author)
    {
        int pageNumber = 1;
        try
        {
            pageNumber = int.Parse(page);
        }
        catch (Exception) { }
        finally
        {
            Cheeps = _service.GetCheepsByAuthor(author, pageNumber);
        }
        return Page();
    }
}
