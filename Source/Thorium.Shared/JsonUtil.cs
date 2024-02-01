using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Thorium.Shared
{
    public static class JsonUtil
    {
        public static JsonSerializerOptions CaseInsensitive { get; } = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }
}
