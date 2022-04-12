using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace WebSocket.Core;

[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class ExtenssionMethods
{
   
    public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
    {
        foreach (var item in sequence) action(item);
    }
    /// <summary>
    /// Gets the value associated with the specified key, or <c>default</c> of <typeparamref name="TValue"/> if not present in the <paramref name="dictionary"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dictionary">The <see cref="IDictionary{TKey,TValue}"/> to search.</param>
    /// <param name="key">The key to search for.</param>
    /// <param name="defaultValue">Optional. Value to return in case not found. Default is <c>default</c>.</param>
    /// <returns>
    /// The value of <paramref name="key"/> in the <paramref name="dictionary"/>, or <c>default</c> of <typeparamref name="TValue"/> if not found or if <paramref name="dictionary"/> is <c>null</c>.
    /// </returns>
    public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default!)
    {
        return (dictionary is null || !dictionary.TryGetValue(key, out var value)
            ? defaultValue
            : value);
    }
    public static string? GetDescription<T>(this T @enum) where T : Enum
    {
        var attributes = (@enum.GetType()
                .GetField(@enum.ToString()) ?? throw new InvalidOperationException())
            .GetCustomAttributes<DescriptionAttribute>(false);
        return attributes.FirstOrDefault()?.Description;
    }

    public static Dictionary<T?, string?> GetMemberDescriptions<T>() where T : Enum
    {
        return typeof(T).GetFields()
            .ToDictionary(f => (T)f.GetValue(null), f => f.GetCustomAttributes<DescriptionAttribute>(false).FirstOrDefault()?.Description);
    }

    public static IDictionary<string, T?> GetDescriptionMembers<T>() where T : Enum
    {
        IDictionary<string, T?> dictionary = new Dictionary<string, T?>();
        foreach (var field in typeof(T).GetFields())
        {
            var f = (field, desc: field.GetCustomAttributes<DescriptionAttribute>(false).FirstOrDefault()?.Description);
            if (!(f.desc is null)) dictionary.Add(f.desc!, (T) f.field.GetValue(f));
        }

        return dictionary;
    }
    public static IEnumerable<(string name, string value)> AsEnumerable(this NameValueCollection collection) => collection
        .Cast<string>()
        .Select(name => (name, collection[name]))!;
    
    /// <summary>
    /// Indicates whether a specified string is null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="value">The string to test.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="value"/> is <c>null</c> or <see cref="string.Empty"/>,
    /// or if <paramref name="value"/> consists exclusively of white-space characters.
    /// </returns>
    /// <seealso cref="string.IsNullOrWhiteSpace"/>
    public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// Indicates whether the specified string is null or an <see cref="F:System.String.Empty"></see> string.
    /// </summary>
    /// <param name="value">The string to test.</param>
    /// <returns>
    /// <c>true</c> if the <paramref name="value">value</paramref> parameter is null or an empty string (""); otherwise, <c>false</c>.
    /// </returns>
    /// <seealso cref="string.IsNullOrEmpty"/>
    public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);
    
    public static void IgnoreAwait(this Task task) => task.ConfigureAwait(false);
    
    public static Uri AddPath(this Uri uri, string path) => new UriBuilder(uri).AddPath(path).Uri;

    public static UriBuilder AddPath(this UriBuilder builder, string path)
    {
        builder.Path += path.StartsWith("/") ? path : $"/{path}";
        return builder;
    }
}