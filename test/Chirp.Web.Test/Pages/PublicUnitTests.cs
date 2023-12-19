using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;
using Chirp.Core;
using Chirp.Core.Services;
using Chirp.SharedUsings;

namespace Chirp.Web.Pages
{
    public class PublicUnitTests
    {
        private readonly HttpClient httpClient;
        private readonly Mock<IChirpService> chirpServiceMock;

        public PublicUnitTests(ITestOutputHelper output)
        {
            var outputLoggerFactory = new OutputLoggerFactory(output);

            this.chirpServiceMock = new Mock<IChirpService>();

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

                        // Replace ChirpService with our ChirpService mock implementation
                        services.RemoveAll<IChirpService>();
                        services.AddTransient<IChirpService>(x => this.chirpServiceMock.Object);
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
        public async Task OnGet_With_No_Parameters_Should_Return_StatusCode_OK()
        {
            // Arrange

            // Act
            var actual = await this.httpClient.GetAsync("/");

            // Assert
            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        }

        [Fact]
        public async Task OnPost_With_AuthentictedUser_And_CheepText_Should_CreateCheep_And_Return_StatusCode_OK()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var authorName = "Mr.Author";
            SetAuthenticatedUser(authorId, authorName);

            var cheepText = "Hello";
            var form = new Dictionary<string, string>()
            {
                ["cheepText"] = cheepText,
            };

            this.chirpServiceMock.Setup(x => x.GetAuthor(authorId)
                ).Returns(
                    Task.FromResult(
                        (AuthorDTO?)new AuthorDTO(authorId, authorName, Enumerable.Empty<Guid>()
                        )));


            // Act
            using var content = new FormUrlEncodedContent(form);
            var actual = await this.httpClient.PostAsync("/?handler=share", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);

            this.chirpServiceMock.Verify(x => x.CreateCheep(
                authorId,
                cheepText
                ), Times.Once);
        }

        [Fact]
        public async Task OnPost_With_NoneAuthenticted_And_CheepText_Should_Return_StatusCode_BadRequest()
        {
            // Arrange
            var cheepText = "Hello";
            var form = new Dictionary<string, string>()
            {
                ["cheepText"] = cheepText,
            };

            // Act
            using var content = new FormUrlEncodedContent(form);
            var actual = await this.httpClient.PostAsync("/handler=share", content);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, actual.StatusCode);
        }

        #region Private helpers

        private void SetAuthenticatedUser(Guid userId, string userName)
        {
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthenticationHandler.AuthenticationScheme);
            this.httpClient.DefaultRequestHeaders.Add(TestAuthenticationHandler.UserId, userId.ToString());
            this.httpClient.DefaultRequestHeaders.Add(TestAuthenticationHandler.UserName, userName);
        }

        #endregion
    }
}