namespace Chirp.Core;
public record AuthorDTO(Guid Id, string Name, string Email, IEnumerable<Guid> followingIds);