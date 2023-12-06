public class Author
{
    public Guid AuthorId { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }

    public IEnumerable<Cheep> Cheeps { get; set; } = new List<Cheep>();
    public IEnumerable<AuthorAuthorRelation> Following { get; set; } = new List<AuthorAuthorRelation>();
}
