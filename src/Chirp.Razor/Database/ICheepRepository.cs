public interface ICheepRepository
{
    public void AddAuthor(String name, string email);

    public void DeleteAuthor(Author author);

    public void WriteCheep(string text, DateTime publishTimestamp, Author author);

    public void DeleteCheep(Cheep cheep);

    public IEnumerable<Cheep> GetAllCheeps(int page);

    public IEnumerable<Cheep> GetCheeps(int page, int amount);

    public IEnumerable<Cheep> GetCheepsByAuthor(Author author);

    public Cheep GetCheepById(int id);

    public Author GetAuthor(int id);
}