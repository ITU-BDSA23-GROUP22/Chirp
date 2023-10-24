namespace Chirp.Razor.test;

using Chirp.Razor;

public class RepositoryUnitTest
{
    [Fact]
    public void TestNewUser()
    {
        CheepRepository repo = new CheepRepository();

        string name = "John Doe";
        string email = "john.doe@example.com";
        repo.AddAuthor(name, email);
    }

    [Fact]
    public void TestDeleteUser()
    {
        CheepRepository repo = new CheepRepository();

        string name = "John Doe";
        string email = "john.doe@example.com";
        repo.AddAuthor(name, email);

        Assert.NotNull(repo.GetAuthor(name));

        Author author = repo.GetAuthor(name);
        repo.DeleteAuthor(author);

        Assert.Null(repo.GetAuthor(name));
    }

    [Fact]
    public void TestNewCheep()
    {
        CheepRepository repo = new CheepRepository();

        string name = "John Doe";
        string email = "john.doe@example.com";
        repo.AddAuthor(name, email);

        Author author = repo.GetAuthor(name);
        repo.WriteCheep("hello hello", DateTime.Now, author);
        Assert.Single(repo.GetAllCheeps());
    }

    [Fact]
    public void TestCheepsByAuthor()
    {
        CheepRepository repo = new CheepRepository();

        string name = "John Doe";
        string email = "john.doe@example.com";
        repo.AddAuthor(name, email);
        Author author = repo.GetAuthor(name);
        repo.WriteCheep("hello hello", DateTime.Now, author);
        Assert.Single(repo.GetCheepsByAuthor(author));
    }
}