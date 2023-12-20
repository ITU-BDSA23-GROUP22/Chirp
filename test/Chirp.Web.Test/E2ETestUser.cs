using System.Diagnostics;
using System.Security.Permissions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using Microsoft.Playwright;
using Xunit;
using Xunit.Abstractions;

namespace Chirp.Web.Test;

public class End2EndTestUser
{
    private readonly ITestOutputHelper output;
    public End2EndTestUser(ITestOutputHelper output)
        {
            this.output = output;
        }
            
    [Fact]
    public async Task Open_Public_Timeline_Not_Logged_In_Mainpage(){
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();


        await page.GotoAsync("https://bdsagroup22chirprazor.azurewebsites.net");
        await page.WaitForSelectorAsync("h2");
        var h2 = await page.QuerySelectorAsync("h2");
        if (h2 != null){
            var h2Content = await h2.InnerTextAsync();
            Assert.Equal("Public Timeline",h2Content);
        }else{
            output.WriteLine("Could not find h2");
            Assert.True(false);
        }
        await browser.CloseAsync();
    }



    
    [Fact]
    public async Task Open_Public_Timeline_Not_Logged_In_Navigation_Bar(){
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
        if (thirdDivSelector != null){
        var navigationButtons = await thirdDivSelector.QuerySelectorAllAsync(".nav-link.text-dark[href*='MicrosoftIdentity/Account/SignIn']");
        int buttonLoopCounter = 0;
        foreach (var link in navigationButtons){
            var linkName = await link.InnerTextAsync();
            Assert.Equal(correctButtons[buttonLoopCounter],linkName);
            buttonLoopCounter++;
        }
        }else{
            output.WriteLine("Could not find third div");
            Assert.True(false);
        }
        
        await browser.CloseAsync();
        }




        [Fact]
        public async Task Open_Public_Timeline_Not_Logged_In_Pagination(){
         List<string> cheepsPage1 = new List<string> { 
        };
        List<string> cheepsPage2 = new List<string> { 
        };
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();

        await page.GotoAsync("https://bdsagroup22chirprazor.azurewebsites.net");
        await page.WaitForSelectorAsync(".cheeps");
        var formSelector = await page.QuerySelectorAsync(".cheeps > form");
        if (formSelector != null){
        var cheeps = await formSelector.QuerySelectorAllAsync("li");
        foreach (var link in cheeps){   
            var cheep = await link.InnerTextAsync();
            cheepsPage1.Add(cheep);
        }
        }else{
            output.WriteLine("Could not find <form> for cheeps");
            Assert.True(false);
        }
        
        await page.GotoAsync("https://bdsagroup22chirprazor.azurewebsites.net/?page=2");
        await page.WaitForSelectorAsync(".cheeps");
        var formSelectorPage2 = await page.QuerySelectorAsync(".cheeps > form");
        if (formSelectorPage2 != null){
        var cheeps = await formSelectorPage2.QuerySelectorAllAsync("li");
        foreach (var link in cheeps){   
            var cheep = await link.InnerTextAsync();
            cheepsPage2.Add(cheep);
        }
        }else{
            output.WriteLine("Could not find <form> for cheeps");
            Assert.True(false);
        }
        for (int i = 0; i < cheepsPage1.Count; i++){
            Assert.NotEqual(cheepsPage1[i], cheepsPage2[i]);
        }
        await browser.CloseAsync();
        }



        [Fact]
        public async Task Open_Public_Timeline_Not_Logged_in_Max_Cheeps_Shown(){
             List<string> maxCheeps = new List<string> { 
        };
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();

        await page.GotoAsync("https://bdsagroup22chirprazor.azurewebsites.net/?page=5");
        await page.WaitForSelectorAsync(".cheeps");
        var formSelector = await page.QuerySelectorAsync(".cheeps > form");
        if (formSelector != null){
        var cheeps = await formSelector.QuerySelectorAllAsync("li");
        foreach (var link in cheeps){   
            var cheep = await link.InnerTextAsync();
            maxCheeps.Add(cheep);
        }
        }else{
            output.WriteLine("Could not find <form> for cheeps");
            Assert.True(false);
        }
        Assert.Equal(5,maxCheeps.Count);
        await browser.CloseAsync();
        }
    }