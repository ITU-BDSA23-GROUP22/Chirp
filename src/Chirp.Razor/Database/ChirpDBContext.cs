using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


//the following code is adapted from the documentation 
//https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli
public class ChirpContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    public string DbPath { get; }
    public ChirpContext(DbContextOptions<ChirpContext> options) : base(options)
    {
    }

    public ChirpContext()
    {
        DbPath = Environment.GetEnvironmentVariable("CHIRPDBPATH")

        ?? Path.Combine(Path.GetTempPath(), "cheeping.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");


}

public class Author
{
    public string Name { get; set; }
    [Key]
    public string Email { get; set; }

    public List<Cheep> UserCheeps { get; } = new List<Cheep>();
}

public class Cheep
{
    [Key]
    public int CheepId { get; set; }
    public string Text { get; set; }

    public DateTime TimeStamp { get; set; }
    [ForeignKey("authorEmail")]
    public String authorEmail { get; set; }
}