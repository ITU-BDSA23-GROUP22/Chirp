namespace Chirp.Core;
public interface ICheepRepository
{
    public void AddAuthor(String name, string email);

    public void DeleteAuthor(AuthorDTO author);

    public void WriteCheep(string text, DateTime publishTimestamp, AuthorDTO author);

    public void DeleteCheep(CheepDTO cheep);

    public IEnumerable<CheepDTO> GetAllCheeps(int page);

    public IEnumerable<CheepDTO> GetCheepsByAuthor(string author, int page);

    public CheepDTO GetCheepById(int id);

    public AuthorDTO GetAuthor(int id);
}