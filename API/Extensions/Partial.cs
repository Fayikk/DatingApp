using System.Text.Json.Serialization;

namespace API.Extensions
{
    public static class Partial
    {
        
        public static JsonConverter<DateOnly> DateOnlyConverter { get; }
        public static JsonConverter<TimeOnly> TimeOnlyConverter { get; }
    }
}