using Wang.Seamas.Web.Common.Extensions;

namespace Wang.Seamas.Web.CommonTest;

public class StringExtensionsTest
{
    
    [Theory]
    [InlineData("MyVariable", "my_variable", "myVariable", "MyVariable")]
    [InlineData("myVariable", "my_variable", "myVariable", "MyVariable")]
    [InlineData("HTTPServer", "http_server", "httpServer", "HttpServer")]
    [InlineData("some_value-name.here", "some_value_name_here", "someValueNameHere", "SomeValueNameHere")]
    [InlineData("Version1Value2", "version1_value2", "version1Value2", "Version1Value2")]
    public void ConversionTests(string input, string expectedSnake, string expectedCamel, string expectedPascal)
    {
        
        Assert.Equal(expectedSnake, input.ToSnakeCase());
        Assert.Equal(expectedCamel, input.ToCamelCase());
        Assert.Equal(expectedPascal, input.ToPascalCase());
    }

    [Fact]
    public void NullAndEmpty()
    {
        string n = null!;

        // extension methods should return null when input is null
        Assert.Null(n.ToSnakeCase());
        Assert.Null(n.ToCamelCase());
        Assert.Null(n.ToPascalCase());
        

        var empty = string.Empty;
        
        Assert.Empty(empty.ToSnakeCase());
        Assert.Empty(empty.ToCamelCase());
        Assert.Empty(empty.ToPascalCase());
    }
}