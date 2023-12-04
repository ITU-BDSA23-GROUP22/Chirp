using System.Net;
using System.Net.Http.Headers;
using Chirp.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;
using Moq;
using Chirp.Core;

namespace Chirp.Web.Test.Pages
{
    public class PublicUnitTests
    {
        private readonly HttpClient httpClient;
        private readonly Mock<IChirpService> chirpServiceMock;

        public PublicUnitTests()
        {
            this.chirpServiceMock = new Mock<IChirpService>();

            var factory = new WebApplicationFactory<Chirp.Web.Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
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

            this.httpClient = factory.CreateClient();
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
            var authorName = "Mr.Author";
            var authorEmail = "Mr.Author@Email";
            SetAuthenticatedUser(authorName, authorEmail);

            var cheepText = "Hello";
            var form = new Dictionary<string, string>()
            {
                ["cheepText"] = cheepText,
            };

            // Act
            using var content = new FormUrlEncodedContent(form);
            var actual = await this.httpClient.PostAsync("/", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);

            this.chirpServiceMock.Verify(x => x.CreateCheep(
                It.Is<AuthorDTO>(x =>
                    x.Name == authorName &&
                    x.Email == authorEmail
                    ),
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
            var actual = await this.httpClient.PostAsync("/", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, actual.StatusCode);
        }

        #region Private helpers

        private void SetAuthenticatedUser(string userName, string userEmail)
        {
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthenticationHandler.AuthenticationScheme);
            this.httpClient.DefaultRequestHeaders.Add(TestAuthenticationHandler.UserName, userName);
            this.httpClient.DefaultRequestHeaders.Add(TestAuthenticationHandler.UserEmail, userEmail);
        }

        #endregion
    }
}