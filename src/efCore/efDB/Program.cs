using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;


//the following code is adapted from the documentation 
//https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli
public class ChirpContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    public string DbPath { get; }

    public ChirpContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "cheeping.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

public class Author
{
    public string Name { get; set; }
    public string Email { get; set; }

    public List<int> UserCheeps { get; } = new();
}

public class Cheep
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime TimeStamp { get; set; }
    public Author CheepAuthor { get; set; }
}