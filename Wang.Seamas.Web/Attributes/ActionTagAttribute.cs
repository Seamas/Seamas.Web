namespace Wang.Seamas.Web.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class ActionTagAttribute(string name): Attribute
{
    public string Name { get; } = name;
}