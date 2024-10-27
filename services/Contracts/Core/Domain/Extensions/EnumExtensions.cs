using System;
using System.ComponentModel;

namespace Contracts.Core.Domain.Extensions;

public static class EnumExtensions
{
    public static string GetDescription<T>(this T enumValue) where T : struct
    {
        var type = enumValue.GetType();

        if (!type.IsEnum)
        {
            throw new ArgumentException("EnumerationValue must be of Enum type", nameof(enumValue));
        }

        var enumValueString = enumValue.ToString();

        if (enumValueString is null or "")
        {
            throw new ArgumentNullException(nameof(enumValue));
        }

        var memberInfo = type.GetMember(enumValueString);

        if (memberInfo.Length <= 0)
        {
            return enumValueString;
        }

        var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

        return attrs.Length > 0 ?
            ((DescriptionAttribute)attrs[0]).Description :
            enumValueString;
    }
}
