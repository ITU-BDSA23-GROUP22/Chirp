using Chirp.Core;

namespace Chirp.Web.ViewModels
{
    public class AuthorViewModel
    {
        public Guid Id { get; }
        public string Name { get; }
        public bool CanFollow { get; }
        public bool CanUnfollow { get; }

        public AuthorViewModel(Guid id, string name, AuthorDTO? authenticatedAuthor)
        {
            this.Id = id;
            this.Name = name;
            this.CanFollow = this.DoCanFollow(authenticatedAuthor, id);
            this.CanUnfollow = this.DoCanUnfollow(authenticatedAuthor, id);
        }

        private bool DoCanFollow(AuthorDTO? authenticatedAuthor, Guid authorToFollowId)
        {
            if (authenticatedAuthor == null)
            {
                // Annonymous author
                return false;
            }
            if (authenticatedAuthor.Id == authorToFollowId)
            {
                // Cannot follow self
                return false;
            }
            if (authenticatedAuthor.followingIds.Contains(authorToFollowId))
            {
                // Already following
                return false;
            }

            return true;
        }

        private bool DoCanUnfollow(AuthorDTO? authenticatedAuthor, Guid authorToUnfollowId)
        {
            if (authenticatedAuthor == null)
            {
                // Annonymous author
                return false;
            }
            if (authenticatedAuthor.Id == authorToUnfollowId)
            {
                // Cannot unfollow self
                return false;
            }
            if (!authenticatedAuthor.followingIds.Contains(authorToUnfollowId))
            {
                // not following
                return false;
            }

            return true;
        }
    }
}