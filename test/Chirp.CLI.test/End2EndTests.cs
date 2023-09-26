using System.Diagnostics;
using System.IO;

using SimpleDB;
namespace Chirp.CLI.test;


public class End2End
{
    [Fact]
    public void TestReadCheep()
    {
        string output = "";
        using (var process = new Process())
        {
            process.StartInfo.FileName = "../../../../../src/Chirp.CLI/bin/Debug/net7.0/Chirp.CLI.exe";
            process.StartInfo.Arguments = "--read 10";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = "../../../../../src/Chirp.CLI";
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            StreamReader reader = process.StandardOutput;
            output = reader.ReadToEnd();
            process.WaitForExit();
        }
        string fstCheep = output.Split("\n")[0];

        // Assert
        Assert.StartsWith("ropf", fstCheep);
        Assert.EndsWith("Hello, BDSA students!\r", fstCheep);
    }


    [Fact]
    public void TestWriteCheep()
    {
        var user = Environment.UserName;
        IDatabaseRepository<Cheep> database = new CsvDatabase<Cheep>("../../../../../data/chirp_cli_db.csv");
        using (var process = new Process())
        {
            process.StartInfo.FileName = "../../../../../src/Chirp.CLI/bin/Debug/net7.0/Chirp.CLI.exe";
            process.StartInfo.Arguments = "--cheep Hello!!!";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = "../../../../../src/Chirp.CLI";
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            process.WaitForExit();
        }
        var fstCheep = database.GetLastItem();
        var userAndMessage = user + ",Hello!!!";
        database.DeleteLastLine();
        Assert.StartsWith(userAndMessage, fstCheep);
    }

}