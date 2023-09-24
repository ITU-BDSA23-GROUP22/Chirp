using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using CsvHelper;
using SimpleDB;
using System.Linq;
using System.CommandLine;
using System.CommandLine.Invocation;

class Program
{
  
  public static void Main(String[] args)
  {

    IDatabaseRepository<Cheep> database = new CsvDatabase<Cheep>("../../data/chirp_cli_db.csv");
    //The below implementation if System.CommandLine is created with the help of: https://www.youtube.com/watch?v=nLKh_QaA3oU    
    
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
    rootCommand.Handler = CommandHandler.Create<string, string>((read, cheep) =>
        {
            if (!string.IsNullOrEmpty(read))
            {
                IEnumerable<Cheep> results = database.Read(int.Parse(read));
                UserInterface.printCheeps(results);
            }
            else if (!string.IsNullOrEmpty(cheep))
            {
                DateTimeOffset convertedTime = DateTimeOffset.UtcNow;
                string auth = Environment.UserName;
                string mess = $"{cheep}";
                database.Store(new Cheep(auth, mess, convertedTime.ToUnixTimeSeconds()));
            }
            else
            {
                Console.WriteLine("No option specified. Run --help to see options");
            }
        });
     rootCommand.Invoke(args);
  }
}