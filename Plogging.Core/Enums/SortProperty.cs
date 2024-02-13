using System.Text.Json.Serialization;

namespace Plogging.Core.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortProperty
{
    ScrapCount,
    Distance,
    Steps
}
