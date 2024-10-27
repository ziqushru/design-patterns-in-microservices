using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using FluentValidation.Internal;

namespace Contracts.WebApi.Utils;

public class CamelCasePropertyNameResolver
{
    public static string ResolvePropertyName(Type type, MemberInfo memberInfo, LambdaExpression expression)
    {
        var propertyName = DefaultPropertyNameResolver(memberInfo, expression);

        var words = propertyName.Split('.', StringSplitOptions.RemoveEmptyEntries);

        var camelCaseWords = new string[words.Length];

        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length > 1)
            {
                camelCaseWords[i] = ToCamelCase(words[i]);
            }
        }

        return string.Join('.', camelCaseWords);
    }

    private static string DefaultPropertyNameResolver(MemberInfo memberInfo, LambdaExpression expression)
    {
        if (expression != null)
        {
            var chain = PropertyChain.FromExpression(expression);

            if (chain.Count > 0)
            {
                return chain.ToString();
            }
        }

        return memberInfo.Name;
    }

    private static string ToCamelCase(string word)
    {
        if (string.IsNullOrEmpty(word) || !char.IsUpper(word[0]))
        {
            return word;
        }

        var chars = word.ToCharArray();

        for (var i = 0; i < chars.Length; i++)
        {
            if (i == 1 && !char.IsUpper(chars[i]))
            {
                break;
            }

            var hasNext = i + 1 < chars.Length;

            if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
            {
                break;
            }

            chars[i] = char.ToLower(chars[i], CultureInfo.InvariantCulture);
        }

        return new string(chars);
    }
}
