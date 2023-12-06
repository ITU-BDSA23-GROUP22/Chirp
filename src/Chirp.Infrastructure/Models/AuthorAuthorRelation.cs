public class AuthorAuthorRelation
{
    public required Guid AuthorId { get; set; }
    public required Guid AuthorToFollowId { get; set; }
    public required DateTime TimeStamp { get; set; }

    public required Author Author { get; set; }
    public required Author AuthorToFollow { get; set; }
}
