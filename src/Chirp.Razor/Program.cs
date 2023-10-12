using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<ICheepService, CheepService>();
builder.Services.AddSingleton<ICheepRepository, CheepRepository>();


var app = builder.Build();

using (var context = new ChirpContext()) {
            context.Database.EnsureCreated();
            var tester1 = new CheepRepository();

            var testAuthor = new Author { Name = "TesterMcMuffin", Email = "Tester@Muffinn.dk" };
            tester1.AddAuthor("TesterMcMuffin", "Tester@Muffinn.dk");


            var currentTime = DateTime.UtcNow;
            var testCheep = new Cheep { CheepId = 13, Text = "Dette er en test cheep", TimeStamp = currentTime, authorEmail = testAuthor.Email };
            tester1.WriteCheep("Dette er en test cheep", currentTime, testAuthor);

            
        }


//new dbCreator().createDIfNotExists(app);
/*



tester.DeleteAuthor(testAuthor);
//tester.DeleteCheep(testCheep);
*/
/*
builder.Services.AddDbContext<ChirpContext>();



builder.Services.BuildServiceProvider().GetService<ChirpContext>().Database.Migrate();
*/



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();
