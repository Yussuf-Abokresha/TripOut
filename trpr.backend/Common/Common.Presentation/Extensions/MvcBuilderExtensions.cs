using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Presentation.Extensions;

public static class MvcBuilderExtensions
{
    public static IMvcBuilder AddApplicationParts(this IMvcBuilder builder, Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
            builder.AddApplicationPart(assembly);
        return builder;
    }
}
