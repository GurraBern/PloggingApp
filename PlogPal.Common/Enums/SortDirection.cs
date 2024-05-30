using System.Text.Json.Serialization;

namespace PlogPal.Common.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortDirection
{
    Ascending,
    Descending
}
