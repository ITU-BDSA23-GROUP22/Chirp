namespace Chirp.Web.ViewModels
{
    public class AuthorViewModel
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; }
        public bool CanFollow { get; }
        public bool CanUnfollow { get; }

        public AuthorViewModel(Guid id, string name, string email, bool canFollow, bool canUnfollow)
        {
            this.Id = id;
            this.Name = name;
            this.Email = email;
            this.CanFollow = canFollow;
            this.CanUnfollow = canUnfollow;
        }
    }
}