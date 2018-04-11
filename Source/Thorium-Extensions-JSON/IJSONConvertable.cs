using Newtonsoft.Json.Linq;

namespace Thorium_Extensions_JSON
{
    public interface IJSONConvertable
    {
        void FromJSON(JToken json);
        JToken ToJSON();
    }
}
