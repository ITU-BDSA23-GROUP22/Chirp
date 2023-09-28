using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text.Json;
using Microsoft.VisualBasic;

class Program
{
  private static readonly HttpClient httpClient = new HttpClient();

  public static async Task Main(string[] args)
  {
    //Code just below here specifies the available commands and what the helper function writes about the commands.
    var rootCommand = new RootCommand{
      new Option<string>(
            "--read",description:"Specify how many cheeps you want to read (reading from oldest to newest)"),
      new Option<string>(
            "--cheep", description: "Write a cheep")
    };

    rootCommand.Description = "This is the Twitter clone Chirp!";

    //Koden under checker hvilekn command der er bleevet kaldt.
    //Jeg tror der er en pænere måde at lave det her check på, men det her virkede da jeg testede det.
    //Kald read med --read <Number of cheeps to read>
    //Kald cheep med --cheep <Message>
    //Check helper funktionen ved at skrive -- --hellp ELLER -- --h ELLER -- -?

    rootCommand.Handler = CommandHandler.Create<string, string>(async (read, cheep) =>
    {
      if (!string.IsNullOrEmpty(read))
      {
        var response = await httpClient.GetAsync("https://bdsagroup22chirpremotedb.azurewebsites.net/cheeps");
        if (response.IsSuccessStatusCode)
        {
          var json = await response.Content.ReadAsStringAsync();
          var results = JsonSerializer.Deserialize<Cheep[]>(json);
          UserInterface.printCheeps(results);
        }
        else
        {
          Console.WriteLine("Error fetching cheeps.");
        }
      }
      else if (!string.IsNullOrEmpty(cheep))
      {
        try
        {
          var cheepData = new
          {
            Author = Environment.UserName,
            Message = cheep,
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
          };
          var jsonData = JsonSerializer.Serialize(cheepData);
          var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
          var response = await httpClient.PostAsync("https://bdsagroup22chirpremotedb.azurewebsites.net/cheep", content);

          if (response.IsSuccessStatusCode)
          {
            Console.WriteLine("Cheep posted successfully.");
          }
          else
          {
            Console.WriteLine("Error posting cheep.");
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Error: {ex.Message}");
        }
      }
      else
      {
        Console.WriteLine("No option specified. Run --help to see options");
      }
    });

    rootCommand.Invoke(args);
  }
}


// using System;
// using System.ComponentModel.Design;
// using System.Globalization;
// using System.IO;
// using System.Runtime.CompilerServices;
// using CsvHelper;
// using SimpleDB;
// using System.Linq;
// using System.CommandLine;
// using System.CommandLine.Invocation;

// class Program
// {

//   public static void Main(String[] args)
//   {
//     IDatabaseRepository<Cheep> database = CsvDatabase<Cheep>.getInstance("../../data/chirp_cli_db.csv");
//     //The below implementation if System.CommandLine is created with the help of: https://www.youtube.com/watch?v=nLKh_QaA3oU    

//     //Code just below here specifies the available commands and what the helper function writes about the commands.
//     var rootCommand = new RootCommand{
//           new Option<string>(
//             "--read",description:"Specify how many cheeps you want to read (reading from oldest to newest)"),
//           new Option<string>(
//             "--cheep", description: "Write a cheep")
//         };
//     rootCommand.Description = "This is the Twitter clone Chirp!";

//     //Koden under checker hvilekn command der er bleevet kaldt.
//     //Jeg tror der er en pænere måde at lave det her check på, men det her virkede da jeg testede det.
//     //Kald read med --read <Number of cheeps to read>
//     //Kald cheep med --cheep <Message>
//     //Check helper funktionen ved at skrive -- --hellp ELLER -- --h ELLER -- -?
//     rootCommand.Handler = CommandHandler.Create<string, string>((read, cheep) =>
//         {
//           if (!string.IsNullOrEmpty(read))
//           {
//             IEnumerable<Cheep> results = database.Read(int.Parse(read));
//             UserInterface.printCheeps(results);
//           }
//           else if (!string.IsNullOrEmpty(cheep))
//           {
//             DateTimeOffset convertedTime = DateTimeOffset.UtcNow;
//             string auth = Environment.UserName;
//             string mess = $"{cheep}";
//             database.Store(new Cheep(auth, mess, convertedTime.ToUnixTimeSeconds()));
//           }
//           else
//           {
//             Console.WriteLine("No option specified. Run --help to see options");
//           }
//         });
//     rootCommand.Invoke(args);
//   }
// }