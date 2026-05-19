using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace Common.Application.Serialization
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        private readonly Dictionary<string, string> _resources;

        public JsonStringLocalizer(string path)
        {
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                _resources = JsonSerializer.Deserialize<Dictionary<string, string>>(json)
                            ?? new Dictionary<string, string>();
            }
            else
            {
                _resources = new Dictionary<string, string>();
            }
        }

        // Simple key lookup
        public LocalizedString this[string name]
        {
            get
            {
                var value = _resources.ContainsKey(name)
                    ? _resources[name]
                    : name;

                return new LocalizedString(name, value, value == name);
            }
        }

        // Key lookup with formatting
        public LocalizedString this[string name, params object[] arguments]
            => new LocalizedString(name, string.Format(this[name].Value, arguments));

        // Return all strings
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
            => _resources.Select(r => new LocalizedString(r.Key, r.Value, false));

        // Optional: culture-specific instance
        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            var path = GetFilePathForCulture(culture);
            return new JsonStringLocalizer(path);
        }

        private static string GetFilePathForCulture(CultureInfo culture)
        {
            // Resources folder next to assembly or app root
            var basePath = AppContext.BaseDirectory;
            var fileName = $"{culture.Name}.json"; // e.g., en.json, ar.json
            return Path.Combine(basePath, "Resources", fileName);
        }
    }
}