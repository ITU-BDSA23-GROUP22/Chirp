using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<ICheepService, CheepService>();

/*
var tester = new CheepRepository();

var testAuthor = new Author { Name = "TesterMcMuffin", Email = "Tester@Muffinn.dk" };
tester.AddAuthor("TesterMcMuffin", "Tester@Muffinn.dk");


var currentTime = DateTime.UtcNow;
var testCheep = new Cheep { CheepId = 13, Text = "Dette er en test cheep", TimeStamp = currentTime, authorEmail = testAuthor.Email };
tester.WriteCheep("Dette er en test cheep", currentTime, testAuthor);


tester.DeleteAuthor(testAuthor);
//tester.DeleteCheep(testCheep);
*/


var app = builder.Build();
//DBFacade.createDB();
//DBFacade.readDB(0, 10, null);
var tester = new dbCreator();
tester.createDIfNotExists(app);
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
