namespace Chirp.Core;

public interface IAuthorRepository
{
    public void AddAuthor(String name, string email);

    public void DeleteAuthor(AuthorDTO author);

    public AuthorDTO GetAuthorByID(int id);

    public AuthorDTO GetAuthor(string EmailOrName);


}