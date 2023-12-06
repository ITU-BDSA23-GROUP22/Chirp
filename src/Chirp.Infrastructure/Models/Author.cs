public class Author
{
    public Guid AuthorId { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }

    public required IEnumerable<Cheep> Cheeps { get; set; }
    public required IEnumerable<AuthorAuthorRelation> Following { get; set; } = new List<AuthorAuthorRelation>();
}
