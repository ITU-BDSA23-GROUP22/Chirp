using System.Text.Json;
using CsvHelper;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var csvFilePath = "/tmp/csvdb.csv";
if (!File.Exists(csvFilePath))
{
    FileStream fs = File.Create(csvFilePath);
    fs.Close();
}
using (var stream = File.Open(csvFilePath, FileMode.Append))
using (var writer = new StreamWriter(stream))
using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)){
csv.NextRecord();
csv.WriteRecord(new Cheep("rehe","checkitup", 300));
}


app.MapPost("/cheep", async context =>
{
    try
    {
        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        var cheep = JsonSerializer.Deserialize<Cheep>(requestBody);

        using (var stream = File.Open(csvFilePath, FileMode.Append))
        using (var writer = new StreamWriter(stream))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.NextRecord();
            csv.WriteRecord(cheep);
        }

        context.Response.StatusCode = 201;
        await context.Response.WriteAsync("Cheep stored successfully.");
    }
    catch (Exception exception)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync($"Error: {exception.Message}");
    }
});

app.MapGet("/cheeps", async context =>
{
    try
    {
        var cheeps = new List<Cheep>();
        using (var sr = new StreamReader(csvFilePath))
        using (var csv = new CsvReader(sr, CultureInfo.InvariantCulture))
        {
            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                var cheep = csv.GetRecord<Cheep>();
                if (cheep != null)
                {
                    cheeps.Add(cheep);
                }
            }
        }


        var json = JsonSerializer.Serialize(cheeps);

        context.Response.StatusCode = 200;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(json);


    }
    catch (Exception exception)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync($"Error: {exception.Message}");
    }
});

app.Run();

public record Cheep(string Author, string Message, long Timestamp);
