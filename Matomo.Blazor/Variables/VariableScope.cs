using System.ComponentModel;

namespace Matomo.Blazor.Variables;

public enum VariableScope
{
    [Description("visit")]
    Visit,
    [Description("page")]
    Page
}

public static class VariableScopeExtensions
{
    public static string GetDescription(this VariableScope scope)
    {
        var field = scope.GetType().GetField(scope.ToString());
        var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field!, typeof(DescriptionAttribute))!;
        return attribute.Description;
    }
}