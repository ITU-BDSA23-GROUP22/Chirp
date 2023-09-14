using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using CsvHelper;
using SimpleDB;
using System.Linq;

class Program
{
  
  public static void Main(String[] args)
  {

    IDatabaseRepository<Cheep> database = new CsvDatabase<Cheep>("chirp_cli_db.csv");

    if (args[0] == "read") 
    {
        IEnumerable<Cheep> results = database.Read(int.Parse(args[1]));
        UserInterfce.printCheeps(results);
    } 
    else if (args[0] == "cheep")
    {
        DateTimeOffset convertedTime = DateTimeOffset.UtcNow;
        string auth = Environment.UserName;
        string mess = $"{args[1]}";
        database.Store(new Cheep(auth, mess, convertedTime.ToUnixTimeSeconds()));
    }
  }
}