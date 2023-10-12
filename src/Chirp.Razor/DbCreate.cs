using System;
public class dbCreator
{

    public void createDIfNotExists(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<ChirpContext>();


            }
            catch (Exception ex)
            {
                Console.WriteLine("Something happened");
            }
        }

    }
}