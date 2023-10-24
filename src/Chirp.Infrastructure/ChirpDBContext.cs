using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


//the following code is adapted from the documentation 
//https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli
public class ChirpContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;

    public string DbPath { get; } = null!;
    public ChirpContext(DbContextOptions<ChirpContext> options) : base(options)
    {
    }
    public ChirpContext()
    {
        DbPath = Path.Combine(Path.GetTempPath(), "cheeping.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options){
        //options.UseSqlite($"Data Source=C:/Users/Patrick/AppData/Local/Temp/cheeping.db");
        options.UseSqlite($"Data Source={DbPath}");

    }

}