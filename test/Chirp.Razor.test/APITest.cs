namespace Chirp.Razor.Test;
using System.Net;

public class APITest
{
    [Fact]
    public void VerifyEndpointPublicTimeline()
    {
        var url = "https://bdsagroup22chirprazor.azurewebsites.net/";
        var request = WebRequest.Create(url);
        request.Method = "GET";

        using var response = request.GetResponse();
        using var pageStream = response.GetResponseStream();

        using var reader = new StreamReader(pageStream);
        var webRespons = reader.ReadToEnd();


        Assert.Contains("In various enchanted attitudes, like the Sperm Whale.", webRespons);
        Assert.Contains("It was but a very ancient cluster of blocks generally painted green, and for no other, he shielded me.", webRespons);
        Assert.Contains("Is there a small outhouse which stands opposite to me, so as to my charge.", webRespons);
    }
    [Fact]
    public void VerifyEndpointPrivateTimeline()
    {
        var url = "https://bdsagroup22chirprazor.azurewebsites.net/Roger%20Histand";
        var request = WebRequest.Create(url);
        request.Method = "GET";

        using var response = request.GetResponse();
        using var pageStream = response.GetResponseStream();

        using var reader = new StreamReader(pageStream);
        var webRespons = reader.ReadToEnd();


        Assert.Contains("You are here for at all?", webRespons);
        Assert.Contains("The murder of its outrages were traced home to the horse''s head, and skirting in search of them.", webRespons);
        Assert.Contains("Yes, yes, I am horror-struck at this callous and hard-hearted, said she.", webRespons);

    }


}


