namespace Chirp.Razor.test;

using Chirp.Razor;

public class ContextUnitTest
{
    [Fact]
    public void TestNewUser()
    {
        string name = "John Doe";
        string email = "john.doe@example.com";
        using var db = new ChirpContext();
        db.RemoveRange(db.Authors);
        db.Add(new Author { Name = name, Email = email });
        db.SaveChanges();
        Assert.True(db.Authors.First().Email == email, $"Result should be '{email}', not '" + db.Authors.First().Email + "'");
    }

    [Fact]
    public void TestRemoveUser()
    {
        string name = "John Doe";
        string email = "john.doe@example.com";
        Author author = new Author { Name = name, Email = email };
        using var db = new ChirpContext();
        db.RemoveRange(db.Authors);
        db.Add(author);
        db.SaveChanges();
        Assert.True(db.Authors.Count() == 1, $"Result should be 1, not " + db.Authors.Count());
        db.Remove(author);
        db.SaveChanges();
        Assert.True(db.Authors.Count() == 0, $"Result should be 0, not " + db.Authors.Count());
    }
}