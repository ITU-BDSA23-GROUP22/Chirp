namespace Chirp.CLI.test;
using System.Globalization;


public class UserInterfaceTest
{
    [Fact]
    public void StringFormatting()
    {

        var testCheep = new Cheep("Test Bruger", "Dette er en test", 1695121763);
        DateTime convertedTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        convertedTime = convertedTime.AddSeconds(1695121763);
        convertedTime = convertedTime.ToLocalTime();
        string formattedTime = convertedTime.ToString("dd-MM-yy HH:mm");

        var testMessage = "Test Bruger @ " + formattedTime + ": Dette er en test";

        Assert.Equal(testMessage, testCheep.FormattedCheep());
    }
}