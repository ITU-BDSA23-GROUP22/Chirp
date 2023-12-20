namespace Chirp.Web.ViewModels
{
    /// <summary>
    ///     Provides CheepViewModel for markup code rendering a Cheep
    /// </summary>
    public class CheepViewModel
    {
        public Guid Id { get; }
        public string Message { get; }
        public string Timestamp { get; }

        public AuthorViewModel Author { get; }

        public CheepViewModel(Guid id, string message, string timestamp, AuthorViewModel author)
        {
            this.Id = id;
            this.Message = message;
            this.Timestamp = timestamp;
            this.Author = author;
        }
    }
}