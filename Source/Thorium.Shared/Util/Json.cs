using System.Text.Json;

namespace Thorium.Shared.Util
{
    public static class Json
    {
        public static JsonSerializerOptions CaseInsensitive { get; } = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }
}
