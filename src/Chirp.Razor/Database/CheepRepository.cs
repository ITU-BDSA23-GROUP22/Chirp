using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            db.Add(new Author { Name = name, Email = email, Cheeps = new List<Cheep>()});

        }
        db.SaveChanges();
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
            db.Add<Cheep>(new Cheep { Text = text, TimeStamp = publishTimestamp,  AuthorId= author.AuthorId, Author =  author});

        }
        db.SaveChanges();
        db.ChangeTracker.Clear();
    }

    public void DeleteCheep(Cheep cheep)
    {
        var cheepToDelete = db.Cheeps.Find(cheep.CheepId);
        if (cheepToDelete != null)
        {
            db.Cheeps.Remove(cheepToDelete);

        }
        db.SaveChanges();
        db.ChangeTracker.Clear();
    }

    public IEnumerable<Cheep> GetAllCheeps(int page)
    {
        return db.Cheeps
        .Skip(32*(page - 1))
        .Take(32)
        .ToList();
    }

    public Author GetAuthor(int id)
    {
        var author = db.Authors
        .Where(b => b.AuthorId == id)
        .FirstOrDefault();
        if (author != null){
            return author;
        }else{
            throw new NullReferenceException("Author not found");
        }
        
    }

    public Cheep GetCheepById(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Cheep> GetCheeps(int page, int amount)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Cheep> GetCheepsByAuthor(string author, int page)
    {
        return db.Cheeps
        .Where(cheep => cheep.Author.Name == author)
        .Skip(32*(page - 1))
        .Take(32)
        .ToList();
    }


}