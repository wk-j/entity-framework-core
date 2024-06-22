using System.ComponentModel.DataAnnotations;

namespace Immutable;

public record Entity {
    [Key]
    public int Id { get; init; }
}
