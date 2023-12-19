using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.ViewModels
{
    public class CheepShareViewModel
    {
        public string? CheepText { get; set; }

        public CheepShareViewModel()
        { }
    }
}