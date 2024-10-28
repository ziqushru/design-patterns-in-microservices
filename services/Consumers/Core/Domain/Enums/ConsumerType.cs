using System.ComponentModel;

namespace Consumers.Core.Domain.Enums;

public enum ConsumerType
{
    [Description("Μη Ορισμένο")]
    NotDefined,

    [Description("Φυσικό Πρόσωπο")]
    Individual,

    [Description("Επιχείρηση")]
    Company
}
