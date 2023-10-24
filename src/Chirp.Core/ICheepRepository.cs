namespace Chirp.Core;
public interface ICheepRepository
{
    public void AddAuthor(String name, string email);

    //public void DeleteAuthor(Author author);

    //public void WriteCheep(string text, DateTime publishTimestamp, Author author);

    //public void DeleteCheep(Cheep cheep);

    public IEnumerable<CheepDTO> GetAllCheeps(int page);

    public IEnumerable<CheepDTO> GetCheepsByAuthor(string author, int page);

    public CheepDTO GetCheepById(int id);

    public AuthorDTO GetAuthor(int id);
}