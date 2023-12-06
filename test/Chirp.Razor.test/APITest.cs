// namespace Chirp.Razor.Test;
// using System.Net;

// public class APITest
// {
//     [Fact]
//     public void VerifyEndpointPublicTimeline()
//     {
//         var url = "https://bdsagroup22chirprazor.azurewebsites.net/";
//         var request = WebRequest.Create(url);
//         request.Method = "GET";

//         using var response = request.GetResponse();
//         using var pageStream = response.GetResponseStream();

//         using var reader = new StreamReader(pageStream);
//         var webRespons = reader.ReadToEnd();


//         Assert.Contains("In various enchanted attitudes, like the Sperm Whale.", webRespons);
//         Assert.Contains("It was but a very ancient cluster of blocks generally painted green, and for no other, he shielded me.", webRespons);
//         Assert.Contains("Is there a small outhouse which stands opposite to me, so as to my charge.", webRespons);
//     }

//     [Fact]
//     public void VerifyEndpointPublicTimelinePageination()
//     {
//         var url = "https://bdsagroup22chirprazor.azurewebsites.net/?page=12";
//         var request = WebRequest.Create(url);
//         request.Method = "GET";

//         using var response = request.GetResponse();
//         using var pageStream = response.GetResponseStream();

//         using var reader = new StreamReader(pageStream);
//         var webRespons = reader.ReadToEnd();


//         Assert.Contains("He swaggered up a curtain, there stepped the man who called himself Stapleton was talking all the five dried pips.", webRespons);
//         Assert.Contains("And another thousand to him as possible.", webRespons);
//         Assert.Contains("She is, as you or the Twins.", webRespons);
//     }



//     [Fact]
//     public void VerifyEndpointPrivateTimeline()
//     {
//         var url = "https://bdsagroup22chirprazor.azurewebsites.net/Roger%20Histand";
//         var request = WebRequest.Create(url);
//         request.Method = "GET";

//         using var response = request.GetResponse();
//         using var pageStream = response.GetResponseStream();

//         using var reader = new StreamReader(pageStream);
//         var webRespons = reader.ReadToEnd();


//         Assert.Contains("You are here for at all?", webRespons);
//         Assert.Contains("Immense as whales, the Commodore was pleased at the Museum of the whale.", webRespons);
//         Assert.Contains("Yes, yes, I am horror-struck at this callous and hard-hearted, said she.", webRespons);

//     }

//     [Fact]
//     public void VerifyEndpointPrivateTimelinePagination()
//     {
//         var url = "https://bdsagroup22chirprazor.azurewebsites.net/Roger%20Histand/?page=2";
//         var request = WebRequest.Create(url);
//         request.Method = "GET";

//         using var response = request.GetResponse();
//         using var pageStream = response.GetResponseStream();

//         using var reader = new StreamReader(pageStream);
//         var webRespons = reader.ReadToEnd();


//         Assert.Contains("For, owing to the blood of those fine whales, Hand, boys, over hand!", webRespons);
//         Assert.Contains("You will see to the spot.", webRespons);
//         Assert.Contains("This he placed the slipper upon the whale, where all is well.", webRespons);

//     }
// }