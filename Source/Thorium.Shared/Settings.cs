using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Thorium.Shared
{
    public static class Settings
    {
        private static readonly List<JsonDocument> documents = [];

        public static void LoadJson(string filePath)
        {
            JsonDocument document = JsonDocument.Parse(File.ReadAllText(filePath));
            documents.Insert(0, document);
        }

        public static T? Get<T>(string key)
        {
            foreach (var doc in documents)
            {
                if (doc.RootElement.TryGetProperty(key, out JsonElement element))
                {
                    return element.Deserialize<T>();
                }
            }
            throw new Exception("Could not find " + key + " in settings");
        }

        public static T? Get<T>(string key, T? defaultValue)
        {
            foreach (var doc in documents)
            {
                if (doc.RootElement.TryGetProperty(key, out JsonElement element))
                {
                    return element.Deserialize<T>();
                }
            }
            return defaultValue;
        }

        static Settings()
        {
            if (File.Exists("settings.json"))
            {
                LoadJson("settings.json");
            }
        }
    }
}
