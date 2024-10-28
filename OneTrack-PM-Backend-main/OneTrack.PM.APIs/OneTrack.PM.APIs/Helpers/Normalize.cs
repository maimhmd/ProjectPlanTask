using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
public static class Normalize
{
    public static string RemoveDiacritics(string text)
    {
        if(text == null)
            return string.Empty;
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

        for (int i = 0; i < normalizedString.Length; i++)
        {
            char c = normalizedString[i];
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark &&
                unicodeCategory != UnicodeCategory.ModifierLetter)
            {
                if (c == 1577)
                    stringBuilder.Append("ه");
                else if (c == 1609)
                    stringBuilder.Append("ي");
                else
                    stringBuilder.Append(c);
            }
        }

        return RemoveWhitespace(stringBuilder
            .ToString()
            .Normalize(NormalizationForm.FormC).Trim().Replace("د ا", "دا"));
    }
    public static string RemoveWhitespace(string input)
    {
        if (input == null)
            return null;
        return new string(input.ToCharArray()
            .Where(c => !Char.IsWhiteSpace(c))
            .ToArray());
    }
}