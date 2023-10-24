using Chirp.Core;
public interface ICheepService

{

    public IEnumerable<CheepDTO> GetCheeps(int page);
    public IEnumerable<CheepDTO> GetCheepsFromAuthor(string author, int page);
}

public class CheepService : ICheepService
{
    CheepRepository dbCall = new CheepRepository();
    public IEnumerable<CheepDTO> GetCheeps(int page)
    {
        return dbCall.GetAllCheeps(page);
    }
    
    public IEnumerable<CheepDTO> GetCheepsFromAuthor(string author, int page)
    {
        return dbCall.GetCheepsByAuthor(author,page);
       
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
