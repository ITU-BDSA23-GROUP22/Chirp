using System;
public class CheepRepository : ICheepRepository
{
    readonly ChirpContext db;
    public CheepRepository()
    {
        db = new ChirpContext();
    }

    public void AddAuthor(string name, string email)
    {
        db.Add(new Author { Name = name, Email = email });
        db.SaveChanges();
        db.ChangeTracker.Clear();
        //throw new NotImplementedException();
    }

    public void DeleteAuthor(Author author)
    {
        db.Remove(author);
        db.SaveChanges();
        db.ChangeTracker.Clear();
        //throw new NotImplementedException();
    }

    public void WriteCheep(string text, DateTime publishTimestamp, Author author)
    {
        db.Add(new Cheep { Id = 1, Text = text, TimeStamp = publishTimestamp, CheepAuthor = author });
        db.SaveChanges();
        db.ChangeTracker.Clear();
    }

    public void DeleteCheep(Cheep cheep)
    {
        db.Remove(cheep);
        db.SaveChanges();
        db.ChangeTracker.Clear();
        //throw new NotImplementedException();
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


}