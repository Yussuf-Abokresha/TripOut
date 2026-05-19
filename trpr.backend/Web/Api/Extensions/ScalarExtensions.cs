using Microsoft.OpenApi;
using System.Text.Json.Nodes;

namespace Api.Extensions
{
    public static class ScalarExtensions
    {
        public static void ConfigureScalar(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddOpenApi(options =>
            {

                options.AddDocumentTransformer((document, context, ct) =>
                {
                    foreach (var path in document.Paths.Values)
                    {
                        foreach (var op in path.Operations!.Values)
                        {
                            op.Parameters ??= [];

                            if (!op.Parameters.Any(p => p.Name == "X-Language"))
                            {
                                op.Parameters.Add(new OpenApiParameter()
                                {
                                    Name = "X-Language",
                                    In = ParameterLocation.Header,
                                    Required = false,
                                    Description = "Select language",
                                    Schema = new OpenApiSchema
                                    {
                                        Type = JsonSchemaType.String,
                                        Enum =
                                        [
                                            JsonValue.Create("en"),
                                            JsonValue.Create("ar")
                                        ],
                                        Default = JsonValue.Create("en")
                                    }
                                });
                            }

                            AddHeader(op, "X-User-Id", "User Id header");
                            AddHeader(op, "X-User-Role", "Comma separated roles (Admin,User)");
                            AddHeader(op, "X-Language", "Select language");

                        }
                    }

                    return Task.CompletedTask;
                });
            });
        }

        static void AddHeader(
            OpenApiOperation op,
            string name,
            string description)
        {
            if (op.Parameters?.Any(p => p.Name == name) ?? false) return;

            op.Parameters?.Add(new OpenApiParameter
            {
                Name = name,
                In = ParameterLocation.Header,
                Description = description,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = JsonSchemaType.String
                }
            });
        }
    }
}