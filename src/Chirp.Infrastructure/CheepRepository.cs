using System;
using Chirp.Core;
public class CheepRepository : ICheepRepository
{
    readonly ChirpContext db;
    public CheepRepository(ChirpContext chirpContext)
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
            db.Add<Cheep>(new Cheep { Text = text, TimeStamp = publishTimestamp, AuthorId = author.AuthorId, Author = author });

        }
        else
        {
            AddAuthor(author.Name, author.Email);
            db.Add<Cheep>(new Cheep { Text = text, TimeStamp = publishTimestamp, AuthorId = author.AuthorId, Author = author });

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

    public IEnumerable<CheepDTO> GetAllCheeps(int page)
    {
        List<CheepDTO> cheepDTOList = new List<CheepDTO>();
        var cheeps = db.Cheeps
        .Skip(32 * (page - 1))
        .Take(32)
        .ToList();
        foreach (Cheep cheep in cheeps)
        {
            cheepDTOList.Add(new CheepDTO(GetAuthor(cheep.AuthorId).Name, cheep.Text, cheep.TimeStamp.ToString()));
        }
        return cheepDTOList;
    }

    public AuthorDTO GetAuthor(int id)
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

    public CheepDTO GetCheepById(int id)
    {
        var cheep = db.Cheeps.Find(id);
        if (cheep != null)
        {
            return new CheepDTO(cheep.Author.Name, cheep.Text, cheep.TimeStamp.ToString());
        }
        else
        {
            throw new Exception("Cheep not found");
        }
    }

    public IEnumerable<CheepDTO> GetCheepsByAuthor(string author, int page)
    {
        List<CheepDTO> cheepDTOList = new List<CheepDTO>();
        var cheeps = db.Cheeps
        .Where(cheep => cheep.Author != null && cheep.Author.Name == author)
        .Skip(32 * (page - 1))
        .Take(32)
        .ToList();
        foreach (Cheep cheep in cheeps)
        {
            if (cheep.Author != null)
            {
                cheepDTOList.Add(new CheepDTO(cheep.Author.Name, cheep.Text, cheep.TimeStamp.ToString()));
            }
            else
            {
                Console.WriteLine("Cheep Author is null");
            }
            
        }
        return cheepDTOList;
    }


}