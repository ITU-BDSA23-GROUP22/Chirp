using System.Net;
using System.Net.Http.Headers;
using Chirp.Infrastructure;
using Chirp.SharedUsings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Chirp.IntegrationTests
{
    public class IntegrationTest
    {
        private readonly ILogger logger;
        private readonly HttpClient httpClient;
        private readonly ChirpDBContext dbContext;
        private readonly SqliteConnection connection;

        public IntegrationTest(ITestOutputHelper output)
        {
            var outputLoggerFactory = new OutputLoggerFactory(output);
            this.logger = outputLoggerFactory.CreateLogger<IntegrationTest>();

            this.connection = new SqliteConnection($"DataSource=:memory:");
            connection.Open();

            this.dbContext = new ChirpDBContext(
                new DbContextOptionsBuilder<ChirpDBContext>()
                    .EnableDetailedErrors(true)
                    .EnableSensitiveDataLogging(true)
                    .UseSqlite(connection)
                    .Options
            );
            this.dbContext.Database.EnsureCreated();

            var factory = new WebApplicationFactory<Chirp.Web.Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // override logging to redirect logs to xunit output log
                        services.RemoveAll<ILoggerFactory>();
                        services.AddSingleton<ILoggerFactory>(x => outputLoggerFactory);

                        // Override authentication scheme
                        services.AddAuthentication(TestAuthenticationHandler.AuthenticationScheme)
                            .AddScheme<TestAuthenticationHandlerOptions, TestAuthenticationHandler>(TestAuthenticationHandler.AuthenticationScheme, options => { });

                        // Override razor options to disable AntiforgeryToken during tests
                        services.AddRazorPages().AddRazorPagesOptions(options =>
                        {
                            options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
                        });

                        // Override ChirpDbContext with in-memory SqLite database
                        services.RemoveAll<DbContextOptions<ChirpDBContext>>();
                        services.AddDbContext<ChirpDBContext>(options =>
                            options.EnableDetailedErrors(true)
                            .EnableSensitiveDataLogging(true)
                            .UseSqlite(connection)
                        );
                    });
                });

            this.httpClient = factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = true
                });
            this.httpClient.BaseAddress = new Uri("https://localhost/");
        }

        [Fact]
        public async Task PublicPage_Share_Cheep_Flow()
        {
            // Arrange - Share Cheep with new author should create Auhtor and Cheep

            var author1Id = Guid.NewGuid();
            var author1Name = "author1";
            var cheep1Text = "Hello";

            // Act
            var share_Cheep1_result = await PublicPage_Share_Cheep(author1Id, author1Name, cheep1Text);

            // Assert
            Assert.Equal(HttpStatusCode.OK, share_Cheep1_result.StatusCode);

            var author1 = Assert.Single(this.dbContext.Authors.Where(x =>
                    x.AuthorId == author1Id &&
                    x.Name == author1Name));

            Assert.Single(this.dbContext.Cheeps.Where(x =>
                x.AuthorId == author1.AuthorId &&
                x.Text == cheep1Text));


            // Arrange - Share Cheep with existing author should create Cheep

            var cheep2Text = "Hello2";

            // Act
            var share_Cheep2_result = await PublicPage_Share_Cheep(author1Id, author1Name, cheep2Text);

            // Assert
            Assert.Equal(HttpStatusCode.OK, share_Cheep2_result.StatusCode);

            Assert.Single(this.dbContext.Authors);

            Assert.Single(this.dbContext.Cheeps.Where(x =>
                x.AuthorId == author1.AuthorId &&
                x.Text == cheep2Text));


            // Arrange - Share Cheep with new author should create Author and Cheep

            var author2Id = Guid.NewGuid();
            var author2Name = "author2";
            var cheep3Text = "Hello3";
            var share_Cheep3_result = await PublicPage_Share_Cheep(author2Id, author2Name, cheep3Text);

            // Assert
            Assert.Equal(HttpStatusCode.OK, share_Cheep3_result.StatusCode);

            var author2 = Assert.Single(this.dbContext.Authors.Where(x =>
                    x.AuthorId == author2Id &&
                    x.Name == author2Name));

            Assert.Single(this.dbContext.Cheeps.Where(x =>
                x.AuthorId == author2.AuthorId &&
                x.Text == cheep3Text));
        }

        [Fact]
        public async Task PuglicPage_Follow_Author_Flow()
        {
            // Arrange - Share Cheep with new author should create Auhtor and Cheep

            var author1Id = Guid.NewGuid();
            var author1Name = "author1";
            var cheep1Text = "Hello";

            // Act
            var share_Cheep1_result = await PublicPage_Share_Cheep(author1Id, author1Name, cheep1Text);

            // Assert
            Assert.Equal(HttpStatusCode.OK, share_Cheep1_result.StatusCode);


            // Arrange - Follow Author with new author should create Author and follow Author

            var author1 = Assert.Single(this.dbContext.Authors.Where(x =>
                x.AuthorId == author1Id &&
                x.Name == author1Name));

            var author2Id = Guid.NewGuid();
            var author2Name = "author2";

            var follow_Author1_result = await PublicPage_Follow_Author(author2Id, author2Name, author1.AuthorId);

            // Assert
            Assert.Equal(HttpStatusCode.OK, follow_Author1_result.StatusCode);

            var author2 = Assert.Single(this.dbContext.Authors.Where(x =>
                x.AuthorId == author2Id &&
                x.Name == author2Name));

            Assert.Single(dbContext.AuthorAuthorRelations.Where(x =>
                x.AuthorId == author2.AuthorId &&
                x.AuthorToFollowId == author1.AuthorId));

            Assert.Single(author2.Following.Where(x =>
                x.AuthorId == author2.AuthorId &&
                x.AuthorToFollowId == author1.AuthorId));


            // Arrange - UnFollow Author should unfollow Author

            // Act
            var unfollow_Author1_result = await PublicPage_Unfollow_Author(author2Id, author2Name, author1.AuthorId);

            // Assert
            Assert.Equal(HttpStatusCode.OK, unfollow_Author1_result.StatusCode);

            Assert.Empty(dbContext.AuthorAuthorRelations.Where(x =>
                x.AuthorId == author2.AuthorId &&
                x.AuthorToFollowId == author1.AuthorId));

            Assert.Single(this.dbContext
                .Authors.Single(x =>
                    x.AuthorId == author2Id &&
                    x.Name == author2Name)

                .Following.Where(x =>
                    x.AuthorId == author2.AuthorId &&
                    x.AuthorToFollowId == author1.AuthorId));
        }

        private async Task<HttpResponseMessage> PublicPage_Share_Cheep(Guid authorId, string authorName, string cheepText)
        {
            SetAuthenticatedUser(authorId, authorName);

            var form = new Dictionary<string, string>()
            {
                ["cheepText"] = cheepText,
            };

            using var content = new FormUrlEncodedContent(form);
            return await this.httpClient.PostAsync("/?handler=share", content);
        }

        private async Task<HttpResponseMessage> PublicPage_Follow_Author(Guid authorId, string authorName, Guid authorToFollowId)
        {
            SetAuthenticatedUser(authorId, authorName);

            var form = new Dictionary<string, string>()
            {
                ["authorToFollowId"] = authorToFollowId.ToString(),
            };

            using var content = new FormUrlEncodedContent(form);
            return await this.httpClient.PostAsync("/?handler=follow", content);
        }

        private async Task<HttpResponseMessage> PublicPage_Unfollow_Author(Guid authorId, string authorName, Guid authorToUnfollowId)
        {
            SetAuthenticatedUser(authorId, authorName);

            var form = new Dictionary<string, string>()
            {
                ["authorToUnfollowId"] = authorToUnfollowId.ToString(),
            };

            using var content = new FormUrlEncodedContent(form);
            return await this.httpClient.PostAsync("/?handler=unfollow", content);
        }


        
        private void SetAuthenticatedUser(Guid userId, string userName)
        {
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthenticationHandler.AuthenticationScheme);
            this.httpClient.DefaultRequestHeaders.Remove(TestAuthenticationHandler.UserName);
            this.httpClient.DefaultRequestHeaders.Add(TestAuthenticationHandler.UserName, userName);
            this.httpClient.DefaultRequestHeaders.Remove(TestAuthenticationHandler.UserId);
            this.httpClient.DefaultRequestHeaders.Add(TestAuthenticationHandler.UserId, userId.ToString());
        }
    }
}