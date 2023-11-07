using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using Microsoft.VisualBasic;
namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{

    private readonly ICheepRepository _service;

    public IEnumerable<CheepDTO> Cheeps { get; set; } = null!;
    [FromQuery(Name = "page")]
    public string page { get; set; } = null!;

    [BindProperty]
    public string Text { get; set; }


    public PublicModel(ICheepRepository service)
    {
        _service = service;
    }


    public ActionResult OnGet()
    {
        int pageNumber = 1;
        try
        {
            pageNumber = int.Parse(page);
        }
        catch (Exception) { }
        finally
        {
            Cheeps = _service.GetAllCheeps(pageNumber);
        }
        return Page();
    }

    public ActionResult OnPost()
    {
        var Text = Request.Form["testing"];
        if (Text.FirstOrDefault() != null)
        {
            var text = Text;

            var author = new AuthorDTO("newTesterMan", "newTesterMan@gmail.com");
            _service.WriteCheep(text, DateTime.Now, author);

        }
        else
        {
        }
        return RedirectToPage("/Public");
    }

}
