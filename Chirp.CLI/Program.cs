// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");


using System; 
using System.IO;

class Program
{
  
  public static void Main(String[] args)
  {

    if (args[0] == "read") 
    {
        try
        {
            using (var sr = new StreamReader("chirp_cli_db.csv"))
            {
                sr.ReadLine();
                
                string currentLine;
                while((currentLine = sr.ReadLine()) != null)
                {
                    
                    string author = currentLine.Substring(0,currentLine.IndexOf(","));
                    
                    string timeStamp = currentLine.Substring(currentLine.LastIndexOf(",") + 1);
                    long seconds = long.Parse(timeStamp);
                    DateTime convertedTime = new DateTime(1970,1,1,0,0,0,0, DateTimeKind.Utc);
                    convertedTime = convertedTime.AddSeconds(seconds);
                    convertedTime = convertedTime.ToLocalTime();

                    int lengthOfMessage = currentLine.LastIndexOf(",") - currentLine.IndexOf(",");
                    string message = currentLine.Substring(currentLine.IndexOf(",") +1, lengthOfMessage -1);

                    string cheep = $"{author} @ {convertedTime} {message}";
                    Console.WriteLine(cheep);
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
        // convertedTime = convertedTime;
        string author = Environment.UserName;
        string message = $"\"{args[1]}\"";
        string cheep = $"{author},{message},{convertedTime.ToUnixTimeSeconds()}";

        using (StreamWriter sw = File.AppendText("chirp_cli_db.csv"))
        {
            sw.WriteLine(cheep);
        }
    }
  }
}