namespace Chirp.Core;
public record AuthorDTO(Guid Id, string Name, IEnumerable<Guid> followingIds);