namespace Immutable;

public record File : Entity {
    public string Name { set; get; }
    public List<Property> Properties { set; get; }
}

public record Property : Entity {
    public string Name { set; get; }
    public int FileId { set; get; }
    public File File { set; get; }

    public string? StringValue { set; get; }
    public int? IntValue { set; get; }
    public DateTime? DateTimeValue { set; get; }

    public PropertyType Type { set; get; }
}

public enum PropertyType {
    String,
    Int,
    DateTime
}
