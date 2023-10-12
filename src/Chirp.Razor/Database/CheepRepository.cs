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
        if (!db.Authors.Any(a => a.Email == email))
        {
            db.Add(new Author { Name = name, Email = email });
            db.SaveChanges();
        }
        db.ChangeTracker.Clear();
    }

    public void DeleteAuthor(Author author)
    {
        db.Remove(author);
        db.SaveChanges();
        db.ChangeTracker.Clear();
    }

    public void WriteCheep(string text, DateTime publishTimestamp, Author author)
    {
        var existingAuthor = db.Authors.FirstOrDefault(a => a.Email == author.Email);
        if (existingAuthor != null)
        {
            db.Add<Cheep>(new Cheep { Text = text, TimeStamp = publishTimestamp, authorEmail = author.Email });
            db.SaveChanges();
        }
        db.ChangeTracker.Clear();
    }

    public void DeleteCheep(Cheep cheep)
    {
        var cheepToDelete = db.Cheeps.Find(cheep.CheepId);
        if (cheepToDelete != null)
        {
            db.Cheeps.Remove(cheepToDelete);
            db.SaveChanges();
        }
        db.ChangeTracker.Clear();
    }

    public IEnumerable<Cheep> GetAllCheeps()
    {
        return db.Cheeps.ToList();
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