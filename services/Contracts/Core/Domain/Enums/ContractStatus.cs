using System.ComponentModel;

namespace Contracts.Core.Domain.Enums;

public enum ContractStatus
{
    [Description("Μη Ορισμένη")]
    NotDefined,

    [Description("Εκκρεμή")]
    Pending,

    [Description("Εγκεκριμένη")]
    Approved
}
