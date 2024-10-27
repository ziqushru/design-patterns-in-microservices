using System.ComponentModel;

namespace Consumers.Core.Domain.Enums;

public enum ConsumerType
{
    [Description("NotDefined")]
    NotDefined,

    [Description("Person")]
    Person,

    [Description("Organization")]
    Organization
}
