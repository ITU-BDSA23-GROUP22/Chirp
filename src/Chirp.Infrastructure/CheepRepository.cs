using System;
using Chirp.Core;

public class CheepRepository : ICheepRepository
{
    readonly ChirpContext db;
    readonly AuthorRepository authorRepo;
    public CheepRepository(ChirpContext chirpContext)
    {
        db = chirpContext;
        authorRepo = new AuthorRepository(chirpContext);
    }

    public void WriteCheep(string text, DateTime publishTimestamp, AuthorDTO author)
    {
        var existingAuthor = db.Authors.FirstOrDefault(a => a.Email == author.Email);
        if (existingAuthor != null)
        {

            db.Add<Cheep>(new Cheep { Text = text, TimeStamp = publishTimestamp, AuthorId = existingAuthor.AuthorId, Author = existingAuthor });

        }
        else
        {
            authorRepo.AddAuthor(author.Name, author.Email);
            var getNewAuthor = db.Authors.FirstOrDefault(a => a.Email == author.Email);
            db.Add<Cheep>(new Cheep { Text = text, TimeStamp = publishTimestamp, AuthorId = getNewAuthor.AuthorId, Author = getNewAuthor });

        }
        db.SaveChanges();
        db.ChangeTracker.Clear();
    }

    public void DeleteCheep(CheepDTO cheep)
    {
        var cheepToDelete = db.Cheeps.Find(cheep); // Might be an issue here? .Find(cheep) of type CheepDTO
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
            cheepDTOList.Add(new CheepDTO(authorRepo.GetAuthorByID(cheep.AuthorId).Name, cheep.Text, cheep.TimeStamp.ToString()));
        }
        return cheepDTOList;
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
        .Where(cheep => cheep.Author.Name == author)
        .Skip(32 * (page - 1))
        .Take(32)
        .ToList();
        foreach (Cheep cheep in cheeps)
        {
            cheepDTOList.Add(new CheepDTO(authorRepo.GetAuthorByID(cheep.AuthorId).Name, cheep.Text, cheep.TimeStamp.ToString()));

        }
        return cheepDTOList;
    }


}