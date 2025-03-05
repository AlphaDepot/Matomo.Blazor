namespace Matomo.Blazor.Dimensions;
/// <summary>
///  Represents a custom dimension that can be used to track custom data in the Matomo API using the built-in Dimension feature.
/// </summary>
public class CustomDimension
{
    public int Id { get; set; }
    public string Value { get; set; }

    public CustomDimension(int id, string value)
    {
        Id = id;
        Value = value;
    }
}