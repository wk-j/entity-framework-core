namespace Immutable;

public record User : Entity {
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
}
