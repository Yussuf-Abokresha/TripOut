using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace Common.Application.Serialization
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        public IStringLocalizer Create(Type resourceSource)
            => CreateJsonLocalizer();

        public IStringLocalizer Create(string baseName, string location)
            => CreateJsonLocalizer();

        private IStringLocalizer CreateJsonLocalizer()
        {
            var culture = CultureInfo.CurrentUICulture;
            var path = Path.Combine(AppContext.BaseDirectory, "Resources", $"{culture.Name}.json");
            return new JsonStringLocalizer(path);
        }
    }
}