using System;
public class CheepRepository : ICheepRepository
{

    public void AddAuthor(string name, string email)
    {
        using var db = new ChirpContext();
        db.Add(new Author { Name = name, Email = email });
        db.SaveChanges();
        //throw new NotImplementedException();
    }

    public void DeleteAuthor(Author author)
    {
        throw new NotImplementedException();
    }

    public void DeleteCheep(Cheep cheep)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Cheep> GetAllCheeps()
    {
        throw new NotImplementedException();
    }

    public Author GetAuthor(string name)
    {
        throw new NotImplementedException();
    }

    public Cheep GetCheepById(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Cheep> GetCheeps(int page, int amount)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Cheep> GetCheepsByAuthor(Author author)
    {
        throw new NotImplementedException();
    }

    public void WriteCheep(string text, DateTime publishTimestamp, Author author)
    {
        throw new NotImplementedException();
    }
}