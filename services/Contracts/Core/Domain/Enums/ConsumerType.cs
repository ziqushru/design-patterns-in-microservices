using System.ComponentModel;

namespace Contracts.Core.Domain.Enums;

public enum ConsumerType
{
    [Description("NotDefined")]
    NotDefined,

    [Description("Person")]
    Person,

    [Description("Organization")]
    Organization
}
