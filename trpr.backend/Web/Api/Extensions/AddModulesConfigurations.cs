using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Extensions
{
    public static class AddModulesConfigurations
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection services, ConfigurationManager configuration, params string[] Modules)
        {
            foreach (var module in Modules)
            {
                configuration.AddJsonFile($"appsettings.{module}.Development.json", optional: true, reloadOnChange: true).AddEnvironmentVariables();
            }
            return services;
        }
    }
}