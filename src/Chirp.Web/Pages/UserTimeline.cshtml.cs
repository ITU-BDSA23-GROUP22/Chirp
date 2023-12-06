﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using Microsoft.AspNetCore.Authorization;
using Chirp.Core.Services;
using Chirp.Web.Pages;

namespace Chirp.Razor.Pages;

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

    public CheepsPageModel cheepsModel { get; set; }

    public UserTimelineModel(IChirpService chirpService )
    {
        this.chirpService = chirpService;
    }

    public async Task<ActionResult> OnGet()
    {
        this.Author = await chirpService.GetAuthor(AuthorId);

        //-----------
        var cheeps = await chirpService.GetCheepsByAuthor(AuthorId, this.GetPageNumber());

        this.cheepsModel = new CheepsPageModel();
        this.cheepsModel.cheeps = cheeps;

        var authorDto = this.GetAuthenticatedAuthor();
        if (authorDto == null)
        {
            this.cheepsModel.authorsFollowedByAuthenticatedUser = new List<Guid>();
        }
        else
        {
            var authenticatedUser = await chirpService.GetAuthor(authorDto.Email);
            if (authenticatedUser == null)
            {
                this.cheepsModel.authorsFollowedByAuthenticatedUser = new List<Guid>();
            }
            else
            {
                this.cheepsModel.authorsFollowedByAuthenticatedUser = authenticatedUser.followingIds;
            }
        }

        return Page();
    }

    public async Task<ActionResult> OnPostFollow(Guid authorToFollowId)
    {
        var authorDto = this.GetAuthenticatedAuthor();
        if (authorDto == null)
        {
            return BadRequest();
        }

        await this.chirpService.FollowAuthor(authorDto, authorToFollowId);

        return Redirect($"/{AuthorId}");
    }

    public async Task<ActionResult> OnPostUnfollow(Guid authorToUnfollowId)
    {
        var authorDto = this.GetAuthenticatedAuthor();
        if (authorDto == null)
        {
            return BadRequest();
        }

        await this.chirpService.UnfollowAuthor(authorDto, authorToUnfollowId);

        return Redirect($"/{AuthorId}");
    }

    public bool IsUserAuthenticated()
    {
        return User.Identity?.IsAuthenticated == true;
    }

    private AuthorDTO? GetAuthenticatedAuthor()
    {

        if (IsUserAuthenticated())
        {
            return new AuthorDTO(
                Guid.Empty,
                User.Identity?.Name ?? string.Empty,
                User.Claims?.SingleOrDefault(x => x.Type == "emails")?.Value ?? string.Empty,
                new List<Guid>()
            );
        }
        return null;
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
