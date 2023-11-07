namespace Chirp.Infrastructure.test;

using Chirp.Core;
using Chirp.Infrastructure;

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
    public void TestNewCheep()
    {
        CheepRepository repo = new CheepRepository();

        string name = "John Doe";
        string email = "john.doe@example.com";
        repo.AddAuthor(name, email);

        AuthorDTO author = repo.GetAuthor(name);
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
        AuthorDTO author = repo.GetAuthor(name);
        repo.WriteCheep("hello hello", DateTime.Now, author);
        Assert.Single(repo.GetCheepsByAuthor(author));
    }

    [Fact]
    public void TestGetAllCheeps()
    {
        CheepRepository repo = new CheepRepository();

        string name = "John Doe";
        string email = "john.doe@example.com";
        repo.AddAuthor(name, email);
        AuthorDTO author = repo.GetAuthor(name);
        for (int i = 0; i < 32; i++)
        {
            repo.WriteCheep("Test cheep " + i, DateTime.Now, author);
        }

        int index = 0;
        foreach (CheepDTO loopCheep in repo.GetAllCheeps(0))
        {
            Assert.Equals(loopCheep.message, "Test cheep " + index);
            index++;
        }
        Assert.Single(repo.GetCheepsByAuthor(author));
    }

    public void TestCheepById()
    {

    }

    public void TestAuthorById()
    {

    }
}