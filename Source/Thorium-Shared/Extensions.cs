public static class Extensions
{
    public static string FirstCharacterToLower(this string str)
    {
        if(string.IsNullOrEmpty(str) || char.IsLower(str, 0))
        {
            return str;
        }

        return char.ToLowerInvariant(str[0]) + str.Substring(1);
    }

    public static string FirstCharacterToUpper(this string str)
    {
        if(string.IsNullOrEmpty(str) || char.IsUpper(str, 0))
        {
            return str;
        }

        return char.ToUpperInvariant(str[0]) + str.Substring(1);
    }
}

