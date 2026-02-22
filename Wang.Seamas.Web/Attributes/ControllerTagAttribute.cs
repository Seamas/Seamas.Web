namespace Wang.Seamas.Web.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ControllerTagAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}