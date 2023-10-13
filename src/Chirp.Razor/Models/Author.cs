public class Author {
    public int AuthorId { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public IEnumerable<Cheep> Cheeps { get; set; }
}