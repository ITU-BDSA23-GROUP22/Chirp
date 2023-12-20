using System.ComponentModel.DataAnnotations;

namespace Chirp.Web.ViewModels
{
    public class AuthorSearchViewModel
    {
        [MaxLength(50, ErrorMessage = "Search text is too long, max 50 characters")]
        public string? SearchText { get; set; }
    }
}