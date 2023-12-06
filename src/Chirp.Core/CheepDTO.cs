using System.ComponentModel.DataAnnotations;

namespace Chirp.Core
{
    public record CheepDTO(AuthorDTO Author, Guid Id, [property: StringLength(160)] string Message, string Timestamp);
}