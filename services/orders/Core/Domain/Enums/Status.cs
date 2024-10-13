using System.ComponentModel;

namespace Core.Domain.Enums;

public enum Status
{
    [Description("NotDefined")]
    NotDefined,

    [Description("Queued")]
    Queued,

    [Description("Submitted")]
    Submitted,

    [Description("Processed")]
    Processed,

    [Description("Invoiced")]
    Invoiced,

    [Description("Departed")]
    Departed,

    [Description("Completed")]
    Completed,

    [Description("Returned")]
    Returned,

    [Description("Cancelled")]
    Cancelled
}
