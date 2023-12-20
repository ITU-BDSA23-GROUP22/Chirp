using System.ComponentModel.DataAnnotations;

namespace Chirp.Web.ViewModels
{
    /// <summary>
    ///     Provides CheepShareViewModel for sharing a Cheep.
    /// </summary>
    public class CheepShareViewModel
    {
        [Required(ErrorMessage = "Cheeptext requried")]
        [MaxLength(160, ErrorMessage = "Cheep is too long, max 160 characters")]
        public string CheepText { get; set; } = string.Empty;
    }
}