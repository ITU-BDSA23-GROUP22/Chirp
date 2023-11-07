using System.ComponentModel.DataAnnotations;

namespace Chirp.Core;
public record CheepDTO(string Author, [property: StringLength(160)] string Message, string Timestamp);