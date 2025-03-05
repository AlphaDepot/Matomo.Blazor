
namespace Matomo.Blazor.Variables;

/// <summary>
///  Represents a custom variable that can be used to track custom data in the Matomo API using the Custom Variable Plugin.
/// </summary>
public class CustomVariable()
{
    public int Slot { get; init; } 
    public string Name { get; init; } = null!; 
    public string Value { get; init; }  = null!; 
    public string Scope { get; init; }  = null!; 

    public CustomVariable(int slot, string name, string value, VariableScope scope) : this()
    {
        Slot = slot;
        Name = name;
        Value = value;
        Scope = scope.GetDescription();

    }
}

