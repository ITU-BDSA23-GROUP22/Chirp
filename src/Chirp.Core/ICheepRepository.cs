namespace Chirp.Core;
public interface ICheepRepository
{
    public void DeleteCheep(CheepDTO cheep);

    public void WriteCheep(string text, DateTime publishTimestamp, AuthorDTO author);
    public IEnumerable<CheepDTO> GetAllCheeps(int page);

    public IEnumerable<CheepDTO> GetCheepsByAuthor(string author, int page);

    public CheepDTO GetCheepById(int id);

}