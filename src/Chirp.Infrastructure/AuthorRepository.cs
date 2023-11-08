using System;
using Chirp.Core;

public class AuthorRepository : IAuthorRepository
{
    readonly ChirpContext db;
    public AuthorRepository(ChirpContext chirpContext)
    {
        db = chirpContext;
    }

    public void AddAuthor(string name, string email)
    {
        if (!db.Authors.Any(a => a.Email == email))
        {
            db.Add(new Author { Name = name, Email = email, Cheeps = new List<Cheep>() });
        }
        db.SaveChanges();
        db.ChangeTracker.Clear();
    }
    public void DeleteAuthor(AuthorDTO author)
    {
        db.Remove(author);
        db.SaveChanges();
        db.ChangeTracker.Clear();
    }

    public AuthorDTO GetAuthorByID(int id)
    {
        var author = db.Authors
        .Where(b => b.AuthorId == id)
        .FirstOrDefault();
        if (author != null)
        {
            return new AuthorDTO(author.Name, author.Email);
        }
        else
        {
            throw new NullReferenceException("Author not found");
        }

    }

    public AuthorDTO GetAuthor(string EmailOrName)
    {
        Author? author = null;
        if (EmailOrName.Contains("@") == true)
        {
            author = db.Authors
           .Where(b => b.Email == EmailOrName)
           .FirstOrDefault();
        }
        else
        {
            author = db.Authors
           .Where(b => b.Name == EmailOrName)
           .FirstOrDefault();
        }

        if (author != null)
        {
            return new AuthorDTO(author.Name, author.Email);
        }
        else
        {
            throw new NullReferenceException("Author not found");
        }

    }
}