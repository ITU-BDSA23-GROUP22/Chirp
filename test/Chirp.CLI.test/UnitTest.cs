namespace Chirp.CLI.test;



public class UserInterfaceTest
{
    [Fact]
    public void StringFormatting()
    {
        var testCheep = new Cheep("Test Bruger", "Dette er en test", 1695121763);
        Assert.Equal("Test Bruger @ 19-09-23 13:09: Dette er en test", testCheep.FormattedCheep());
    }
}