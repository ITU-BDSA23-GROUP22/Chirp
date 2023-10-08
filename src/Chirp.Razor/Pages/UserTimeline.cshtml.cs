using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }
    [FromQuery(Name="page")]
    public string page { get; set; }

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet(string author)
    {
        int pageNumber = 1;
        try {
            pageNumber = int.Parse(page);
        }
        catch(Exception e) {}
        finally {
            Cheeps = _service.GetCheepsFromAuthor(author, pageNumber);
        }
        return Page();
    }
}
