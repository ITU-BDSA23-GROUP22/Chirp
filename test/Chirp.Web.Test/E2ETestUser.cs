using System.Diagnostics;
using System.Security.Permissions;
using Chirp.Core;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.Identity.Client;
using Microsoft.Playwright;
using Xunit;
using Xunit.Abstractions;

namespace Chirp.Web.Test;

public class End2EndTestUser
{
    private readonly ITestOutputHelper output;

    private async Task PageLogin(IPage page)
    {
        await page.GotoAsync("https://bdsagroup22chirprazor.azurewebsites.net");
        await page.WaitForSelectorAsync(".nav-link.text-dark[href*='MicrosoftIdentity/Account/SignIn']");
        await page.ClickAsync(".nav-link.text-dark[href*='MicrosoftIdentity/Account/SignIn']");
        await page.WaitForSelectorAsync("#GitHubExchange");
        await page.ClickAsync("#GitHubExchange");
        //Page would not update fast enough and program grapped the wrong URL. Had to implement a sleep
        await page.WaitForTimeoutAsync(8000);
        await page.ScreenshotAsync(new()
        {
            Path = "../../../ScreenshotLogin.png",
            FullPage = true,
        });
        bool startsWithPrefix = page.Url.StartsWith("https://bdsagroup22chirprazor", StringComparison.OrdinalIgnoreCase);
        if (!startsWithPrefix)
        {
            bool startsWithPrefixGithub = page.Url.StartsWith("https://github.com/login?", StringComparison.OrdinalIgnoreCase);
            if (startsWithPrefixGithub)
            {
                await page.GetByLabel("Username or email address").FillAsync("Myfakegithubaccount");
                await page.GetByLabel("Password").FillAsync("Myfakegithubpassword");
                await page.GetByRole(AriaRole.Button, new() { Name = "Sign in", Exact = true }).ClickAsync();
                await page.WaitForURLAsync("https://bdsagroup22chirprazor.azurewebsites.net/");

            }
            else

            {
                output.WriteLine(page.Url);
                //var htmlContent = await page.ContentAsync();
                //output.WriteLine(htmlContent);
                await page.WaitForSelectorAsync(".btn.btn-primary.width-full.ws-normal");
                await page.ClickAsync(".btn.btn-primary.width-full.ws-normal");
            }
        }
    }
    public End2EndTestUser(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public async Task Open_Public_Timeline_Not_Logged_In_Mainpage()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();


        await page.GotoAsync("https://bdsagroup22chirprazor.azurewebsites.net");
        await page.WaitForSelectorAsync("h2");
        var h2 = await page.QuerySelectorAsync("h2");
        if (h2 != null)
        {
            var h2Content = await h2.InnerTextAsync();
            Assert.Equal("Public Timeline", h2Content);
        }
        else
        {
            output.WriteLine("Could not find h2");
            Assert.True(false);
        }
        await browser.CloseAsync();
    }




    [Fact]
    public async Task Open_Public_Timeline_Not_Logged_In_Navigation_Bar()
    {
        List<string> correctButtons = new List<string> {
            "Register",
            "Login"
        };
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();


        await page.GotoAsync("https://bdsagroup22chirprazor.azurewebsites.net");
        await page.WaitForSelectorAsync(".navigation");

        var thirdDivSelector = await page.QuerySelectorAsync(".navigation > div > div");
        if (thirdDivSelector != null)
        {
            var navigationButtons = await thirdDivSelector.QuerySelectorAllAsync(".nav-link.text-dark[href*='MicrosoftIdentity/Account/SignIn']");
            int buttonLoopCounter = 0;
            foreach (var link in navigationButtons)
            {
                var linkName = await link.InnerTextAsync();
                Assert.Equal(correctButtons[buttonLoopCounter], linkName);
                buttonLoopCounter++;
            }
        }
        else
        {
            output.WriteLine("Could not find third div");
            Assert.True(false);
        }

        await browser.CloseAsync();
    }




    [Fact]
    public async Task Open_Public_Timeline_Not_Logged_In_Pagination()
    {
        List<string> cheepsPage1 = new List<string>
        {
        };
        List<string> cheepsPage2 = new List<string>
        {
        };
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();

        await page.GotoAsync("https://bdsagroup22chirprazor.azurewebsites.net");
        await page.WaitForSelectorAsync(".cheeps");
        var formSelector = await page.QuerySelectorAsync(".cheeps > form");
        if (formSelector != null)
        {
            var cheeps = await formSelector.QuerySelectorAllAsync("li");
            foreach (var link in cheeps)
            {
                var cheep = await link.InnerTextAsync();
                cheepsPage1.Add(cheep);
            }
        }
        else
        {
            output.WriteLine("Could not find <form> for cheeps");
            Assert.True(false);
        }

        await page.GotoAsync("https://bdsagroup22chirprazor.azurewebsites.net/?page=2");
        await page.WaitForSelectorAsync(".cheeps");
        var formSelectorPage2 = await page.QuerySelectorAsync(".cheeps > form");
        if (formSelectorPage2 != null)
        {
            var cheeps = await formSelectorPage2.QuerySelectorAllAsync("li");
            foreach (var link in cheeps)
            {
                var cheep = await link.InnerTextAsync();
                cheepsPage2.Add(cheep);
            }
        }
        else
        {
            output.WriteLine("Could not find <form> for cheeps");
            Assert.True(false);
        }
        for (int i = 0; i < cheepsPage1.Count; i++)
        {
            Assert.NotEqual(cheepsPage1[i], cheepsPage2[i]);
        }
        await browser.CloseAsync();
    }



    [Fact]
    public async Task Open_Public_Timeline_Not_Logged_in_Max_Cheeps_Shown()
    {
        List<string> maxCheeps = new List<string>
        {
        };
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();

        await page.GotoAsync("https://bdsagroup22chirprazor.azurewebsites.net/?page=5");
        await page.WaitForSelectorAsync(".cheeps");
        var formSelector = await page.QuerySelectorAsync(".cheeps > form");
        if (formSelector != null)
        {
            var cheeps = await formSelector.QuerySelectorAllAsync("li");
            foreach (var link in cheeps)
            {
                var cheep = await link.InnerTextAsync();
                maxCheeps.Add(cheep);
            }
        }
        else
        {
            output.WriteLine("Could not find <form> for cheeps");
            Assert.True(false);
        }
        Assert.Equal(5, maxCheeps.Count);
        await browser.CloseAsync();
    }




    [Fact]
    public async Task Open_Public_Timeline_Logged_In_Navigation_bar()
    {

        List<string> correctButtons = new List<string> {
            "my timeline",
            "public timeline ",
            "about me ",
            "Logout [Myfakegithubaccount]",
            "Profile ",
            "About me"
        };
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();

        await PageLogin(page);
        output.WriteLine(page.Url);
        await page.WaitForSelectorAsync(".navigation");


        var thirdDivSelector = await page.QuerySelectorAsync(".navigation > div > div");
        var elementsWithHref = await thirdDivSelector.QuerySelectorAllAsync("[href]");
        int buttonLoopCounter = 0;
        foreach (var link in elementsWithHref)
        {
            var linkName = await link.InnerTextAsync();
            Assert.Equal(correctButtons[buttonLoopCounter], linkName);
            buttonLoopCounter++;
        }


        await browser.CloseAsync();
    }
    [Fact]
    public async Task Follow_Users_And_See_Them_On_Private_Timeline()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();
        await PageLogin(page);
        await page.WaitForSelectorAsync("a:has-text('my timeline')");
        await page.ClickAsync("a:has-text('my timeline')");

        var h2 = await page.QuerySelectorAsync("h2");

        if (h2 != null)
        {
            var h2Content = await h2.InnerTextAsync();

            //Check if we are on private timeline
            Assert.Equal("Myfakegithubaccount's Timeline", h2Content);
        }

        var doesClassExist = await page.QuerySelectorAsync(".cheeps");

        //Check if no cheeps currently exist on private timeline
        Assert.True(doesClassExist == null);

        await page.WaitForSelectorAsync("a:has-text('public timeline ')");
        await page.ClickAsync("a:has-text('public timeline ')");
        var formSelector = await page.QuerySelectorAsync(".cheeps > form");
        if (formSelector != null)
        {
            var buttonSelector = await formSelector.QuerySelectorAsync(".btn");
            if (buttonSelector != null)
            {
                await buttonSelector.ClickAsync();
            }
            else
            {
                output.WriteLine("Button was null");
            }
        }

        await page.WaitForSelectorAsync("a:has-text('my timeline')");
        await page.ClickAsync("a:has-text('my timeline')");

        var doesClassExistAfterFollow = await page.QuerySelectorAsync(".cheeps");

        //Check if cheeps exists on private timeline after following a user
        Assert.True(doesClassExistAfterFollow != null);

        var cheepString = "";
        var formSelectorAfterFollow = await page.QuerySelectorAsync(".cheeps > form");
        if (formSelectorAfterFollow != null)
        {
            var cheep = await formSelectorAfterFollow.QuerySelectorAsync("li");
            cheepString = await cheep.InnerTextAsync();

            //Check if the first cheep is indeed what it should be on the private timeline
            Assert.Equal("Jacqualine Gilcoine  Starbuck now is what we hear the worst. — 08/01/2023 13:17:39", cheepString);
            var buttonSelector = await formSelectorAfterFollow.QuerySelectorAsync(".btn");
            if (buttonSelector != null)
            {
                await buttonSelector.ClickAsync();
            }
            else
            {
                output.WriteLine("Button was null");
            }
        }
        var doesClassExistAfterUnfollow = await page.QuerySelectorAsync(".cheeps");

        //Check if private timeline contains no cheeps after unfollow
        Assert.True(doesClassExistAfterUnfollow == null);
        await browser.CloseAsync();

    }
}