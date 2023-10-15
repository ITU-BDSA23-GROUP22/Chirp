public interface ICheepService

{

    public List<CheepDTO> GetCheeps(int page);
    public List<CheepDTO> GetCheepsFromAuthor(string author, int page);
}

public class CheepService : ICheepService
{
    CheepRepository dbCall = new CheepRepository();
    public List<CheepDTO> GetCheeps(int page)
    {
        var databaseCheeps = dbCall.GetAllCheeps(page);
        List<CheepDTO> DTOCheeps = new();
        foreach (Cheep cheep in databaseCheeps)
        {
            Author cheepAuthor = dbCall.GetAuthor(cheep.AuthorId);
            DTOCheeps.Add(new CheepDTO(cheep.Author.Name, cheep.Text, cheep.TimeStamp.ToString()));
        }
        return DTOCheeps;
    }
    
    public List<CheepDTO> GetCheepsFromAuthor(string author, int page)
    {
        var databaseCheeps = dbCall.GetCheepsByAuthor(author,page);
        List<CheepDTO> DTOCheeps = new();
        foreach (Cheep cheep in databaseCheeps)
        {
            Author cheepAuthor = dbCall.GetAuthor(cheep.AuthorId);
            DTOCheeps.Add(new CheepDTO(cheep.Author.Name, cheep.Text, cheep.TimeStamp.ToString()));
        }
        return DTOCheeps;
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
