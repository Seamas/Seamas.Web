using Wang.Seamas.Web.Common.Exceptions;

namespace Wang.Seamas.Web.Common.Utils;

/// <summary>
/// 断言判断工具
/// 必须包括的核心方法
/// 空值检查：notNull(), isNull()
/// 布尔断言：isTrue(), isFalse()
/// 字符串检查：notEmpty(), notBlank()
/// 集合检查: notEmpty() 
/// 状态检查：state()
/// </summary>
public class Assert
{

    public static Func<string, Exception> ExceptionFunc { get; set; } = message => new BizException(message);

    #region 断言方法


    #region 使用默认异常类型的断言方法

    public static void IsNull(object obj, string message)
    {
        IsNull(obj, message, ExceptionFunc);
    }

    public static void NotNull(object obj, string message)
    {
        NotNull(obj, message, ExceptionFunc);
    }

    public static void IsTrue(bool condition, string message)
    {
        IsTrue(condition, message, ExceptionFunc);
    }

    public static void IsFalse(bool condition, string message)
    {
        IsFalse(condition, message, ExceptionFunc);
    }

    public static void NotEmpty(string value, string message)
    {
        NotEmpty(value, message, ExceptionFunc);
    }

    public static void NotBlank(string value, string message)
    {
        NotBlank(value, message, ExceptionFunc);
    }

    public static void NotEmpty(IEnumerable<object> values, string message)
    {
        NotEmpty(values, message, ExceptionFunc);
    }

    public static void State(bool expression, string message)
    {
        State(expression, message, ExceptionFunc);
    }

    #endregion

    #region 使用自定义异常类型的断言方法

    public static void IsNull(object obj, string message, Func<string, Exception> func)
    {
        if (obj != null)
        {
            throw func(message);
        }
    }

    public static void NotNull(object obj, string message, Func<string, Exception> func)
    {
        if (obj == null)
        {
            throw func(message);
        }
    }

    public static void IsTrue(bool condition, string message, Func<string, Exception> func)
    {
        if (!condition)
        {
            throw func(message);
        }
    }

    public static void IsFalse(bool condition, string message, Func<string, Exception> func)
    {
        if (condition)
        {
            throw func(message);
        }
    }

    public static void NotEmpty(string value, string message, Func<string, Exception> func)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw func(message);
        }
    }

    public static void NotBlank(string value, string message, Func<string, Exception> func)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw func(message);
        }
    }

    public static void NotEmpty(IEnumerable<object> values, string message, Func<string, Exception> func)
    {
        if (!values.Any())
        {
            throw func(message);
        }
    }

    public static void State(bool expression, string message, Func<string, Exception> func)
    {
        if (!expression)
        {
            throw func(message);
        }
    }


    #endregion

    #endregion
}