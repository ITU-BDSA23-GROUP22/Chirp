using System.Diagnostics;
using System.Drawing;
using System.IO;

using SimpleDB;
namespace Chirp.CLI.test;

public class End2End
{

    [Fact]
    public void TestWriteCheep()
    {
        var user = Environment.UserName;
        var ActualOutput = "";

        var process = new Process();
        using (process = new Process())
        {
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = "run --cheep Hello!!!";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = "../../../../../src/Chirp.CLI";
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            StreamReader reader = process.StandardOutput;
            ActualOutput = reader.ReadToEnd();
            process.WaitForExit();

        }

        var ExpectedOutput = "Cheep posted successfully.";

        Assert.StartsWith(ExpectedOutput, ActualOutput);
    }

    [Fact]
    public void TestReadCheep()
    {
        string output = "";
        using (var process = new Process())
        {
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = "run --read 11";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = "../../../../../src/Chirp.CLI";
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            StreamReader reader = process.StandardOutput;
            output = reader.ReadToEnd();
            process.WaitForExit();
        }

        var ExpectedOutput = "Hello!!!";

        Assert.Contains(ExpectedOutput, output);

    }
}