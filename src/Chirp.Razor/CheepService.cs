public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService

{

    public List<CheepViewModel> GetCheeps(int page);
    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page);
}

public class CheepService : ICheepService
{
    List<List<string>> cheeps = DBFacade.readDB(0, 32);
    List<CheepViewModel> cheepsTotal = new();
    // These would normally be loaded from a database for example
    private static readonly List<CheepViewModel> _cheeps = new()
        {
            new CheepViewModel("Helge", "Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", DBFacade.readDB(0, 32)[2][1], UnixTimeStampToDateTimeString(1690895308)),
        };

    public List<CheepViewModel> GetCheeps(int page)
    {
        cheeps = DBFacade.readDB(page, 32);
        List<CheepViewModel> cheepsTotal = new();
        foreach (List<string> cheepList in cheeps)
        {
            cheepsTotal.Add(new CheepViewModel(cheepList[0], cheepList[1], UnixTimeStampToDateTimeString(Convert.ToDouble(cheepList[2]))));
        }
        return cheepsTotal;
    }

    private string UnixTimeStampToDateTimeString(string v)
    {
        throw new NotImplementedException();
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page)
    {
        // filter by the provided author name
        GetCheeps(page);
        return cheepsTotal.Where(x => x.Author == author).ToList();
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
