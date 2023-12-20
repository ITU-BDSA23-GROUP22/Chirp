using System.Diagnostics;
using System.Drawing;
using System.IO;


namespace Chirp.Web.Test;

public class StartProcess
{
    public Process StartProc()
    {
        var user = Environment.UserName;
        var startInfo = false;
        var process = new Process();
        process = new Process();
        process.StartInfo.FileName = "dotnet";
        process.StartInfo.Arguments = "run";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.WorkingDirectory = "../../../../../src/Chirp.Web";
        process.StartInfo.RedirectStandardOutput = true;
        return process;
    }
}