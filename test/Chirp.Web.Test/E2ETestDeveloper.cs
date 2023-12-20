using System.Diagnostics;
using System.Drawing;
using System.IO;
using Xunit;


namespace Chirp.Web.Test;

public class End2EndTestDeveloper
{
    [Fact]
        public void TestProcessStart()
    {
        var testProcess = new StartProcess().StartProc();
        try
        {
            Assert.True(testProcess.Start());
        }
        finally
        {
            testProcess.Dispose();
        }



    }

    [Fact]
    public void TestAzureDatabaseNotNULL()
    {
        var testProcess = new StartProcess().StartProc();
        try
        {
            testProcess.Start();
            
        }
        finally
        {
            testProcess.Dispose();
        }
    }

    [Fact]
    public void TestAzureDatabaseSeededCorrectly()
    {

    }
}
