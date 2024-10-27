using System.ComponentModel;

namespace Contracts.Core.Domain.Enums;

public enum ContractStatus
{
    [Description("NotDefined")]
    NotDefined,

    [Description("Αναμονή")]
    Pending,

    [Description("Επικυρωμένο")]
    Approved
}
