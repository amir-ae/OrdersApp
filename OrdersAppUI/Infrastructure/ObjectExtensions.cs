namespace OrdersAppUI.Infrastructure;

public static class ObjectExtensions
{
    public static T? DeserializeHTML<T>(this string value, T _)
    {
        if (string.IsNullOrEmpty(value))
            return default;

        return JsonSerializer.Deserialize<T>(HttpUtility.HtmlDecode(value));
    }

    public static string? SerializeHTML(this object value)
    {
        if (value == null)
            return null;

        return HttpUtility.HtmlEncode(JsonSerializer.Serialize(value));
    }

    public static T? Deserialize<T>(this string value, T _)
    {
        if (string.IsNullOrEmpty(value))
            return default;

        return JsonSerializer.Deserialize<T>(value);
    }

    public static string? Serialize(this object value)
    {
        if (value == null)
            return null;

        return JsonSerializer.Serialize(value);
    }
}
