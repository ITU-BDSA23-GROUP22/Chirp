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
        DbPath = Path.Combine(Path.GetTempPath(), "cheeping.db").Replace("\\","/");

    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options){
        //options.UseSqlite($"Data Source=C:/Users/Patrick/AppData/Local/Temp/cheeping.db");
        options.UseSqlite($"Data Source={DbPath}");

    }
}


public class Cheep {
    public int CheepId { get; set; }
    public int AuthorId { get; set; }
    public string Text { get; set; }
    public DateTime TimeStamp { get; set; }
    public Author Author {get; set;}
}
public class Author {
    public int AuthorId { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public IEnumerable<Cheep> Cheeps { get; set; }
}