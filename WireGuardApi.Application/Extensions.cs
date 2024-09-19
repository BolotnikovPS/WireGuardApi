namespace WireGuardApi.Application;

internal static class Extensions
{
    public static string RemoveNewLine(this string text) => text.Replace(Environment.NewLine, " ").Trim();

    public static string RemoveText(this string text, string textToRemove) => text.Replace(textToRemove, " ").Trim();
}