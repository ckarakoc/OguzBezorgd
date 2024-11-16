using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Server.Core.SwaggerSchemaFilters;

public class AddDefaultPasswordFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.RequestBody?.Content != null &&
            operation.RequestBody.Content.ContainsKey("application/json"))
        {
            var schema = operation.RequestBody.Content["application/json"].Schema;

            if (schema.Properties.ContainsKey("password"))
            {
                schema.Properties["password"].Example = new OpenApiString("Pa$$w0rd");
            }
        }
    }
}