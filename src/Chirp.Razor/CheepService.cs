public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService

{

    public List<CheepViewModel> GetCheeps(int page);
    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page);
}

public class CheepService : ICheepService
{
    CheepRepository dbCalls = new CheepRepository();
    public List<CheepViewModel> GetCheeps(int page)
    {
        var cheeps = dbCalls.GetAllCheeps();
        List<CheepViewModel> cheepsTotal = new();
        foreach (Cheep cheep in cheeps)
        {
            Author cheepAuthor = dbCalls.GetAuthor(cheep.AuthorId);
            cheepsTotal.Add(new CheepViewModel(cheepAuthor.Name, cheep.Text, cheep.TimeStamp.ToString()));
        }
        return cheepsTotal;
    }

    private string UnixTimeStampToDateTimeString(string v)
    {
        throw new NotImplementedException();
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page)
    {

        var cheeps = dbCalls.GetAllCheeps();
        List<CheepViewModel> cheepsTotal = new();
        foreach (Cheep cheep in cheeps)
        {
            Author cheepAuthor = dbCalls.GetAuthor(cheep.AuthorId);
            cheepsTotal.Add(new CheepViewModel(cheepAuthor.Name, cheep.Text, cheep.TimeStamp.ToString()));
        }
        return cheepsTotal.Where(cheep => cheep.Author == author).ToList();


    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
