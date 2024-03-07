using System.Text.Json.Serialization;

namespace Plogging.Core.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortProperty
{
    Weight,
    Distance,
    Steps
}

