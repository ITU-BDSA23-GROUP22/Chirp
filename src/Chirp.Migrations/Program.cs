using Chirp.Web;

namespace Chirp.Migrations
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Any())
            {
                var cmd = args.FirstOrDefault() ?? string.Empty;
                switch (cmd)
                {
                    case "recreate":
                        Console.WriteLine("Recreate ChirpDatabase..");

                        using (var chirpContext = new DbContextFactory().CreateDbContext(args))
                        {
                            chirpContext.Database.EnsureDeleted();
                            chirpContext.Database.EnsureCreated();
                        }
                        break;

                    case "seed":
                        Console.WriteLine("Seeding ChirpDatabase..");

                        using (var chirpContext = new DbContextFactory().CreateDbContext(args))
                        {
                            chirpContext.Database.EnsureCreated();

                            DbInitializer.SeedDatabase(chirpContext);
                        }
                        break;
                }

            }
        }
    }
}