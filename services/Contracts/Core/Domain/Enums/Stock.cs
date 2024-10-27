using System.ComponentModel;

namespace Core.Domain.Enums;

public enum Stock
{
    [Description("Άλλο")]
    Other,

    [Description("eShop")]
    Eshop,

    [Description("Instagram")]
    Instagram
}
