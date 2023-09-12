using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using CsvHelper;

class Program
{
  
  public static void Main(String[] args)
  {

    if (args[0] == "read") 
    {
        try
        {
            using (var sr = new StreamReader("chirp_cli_db.csv"))
            using (var csv = new CsvReader(sr, CultureInfo.InvariantCulture)){
                
                csv.Read();
                csv.ReadHeader();
                while (csv.Read()){
                    var record = csv.GetRecord<Cheep>();
                    if(record != null){
                        Console.WriteLine(record.FormattedCheep());
                    }
                }
            }
            
        }
        catch (IOException e)
        {
            Console.WriteLine("This file could not be read");
            Console.WriteLine(e.Message);
        }
    } 
    else if (args[0] == "cheep")
    {
        DateTimeOffset convertedTime = DateTimeOffset.UtcNow;
        string auth = Environment.UserName;
        string mess = $"{args[1]}";
        
        using (var stream = File.Open("chirp_cli_db.csv", FileMode.Append))
        using(var writer = new StreamWriter(stream))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.NextRecord();
                csv.WriteRecord(new Cheep(auth, mess, convertedTime.ToUnixTimeSeconds()));
            }
    }
  }
}