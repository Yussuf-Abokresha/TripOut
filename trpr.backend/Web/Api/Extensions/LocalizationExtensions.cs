using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace Api.Extensions
{
    public static class LocalizationExtensions
    {
        public static IServiceCollection AddLocalizationServices(this IServiceCollection services)
        {
            services.AddLocalization(options => { options.ResourcesPath = "Resources"; });
            return services;
        }
        public static void UseLocalization(this IApplicationBuilder app)
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("ar")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
        }
    }
}