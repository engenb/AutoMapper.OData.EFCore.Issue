using System;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LeaseCrunch.GeneralLedger.Infrastructure.Swagger;

internal class VersionFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var newPaths = new OpenApiPaths();
        foreach (var kvp in swaggerDoc.Paths)
        {
            newPaths[kvp.Key.Replace(
                $"{{{nameof(swaggerDoc.Info.Version)}}}",
                swaggerDoc.Info.Version,
                StringComparison.InvariantCultureIgnoreCase)] = kvp.Value;
        }

        swaggerDoc.Paths = newPaths;

        foreach (var parameters in swaggerDoc.Paths.Values
                     .SelectMany(x => x.Operations.Values)
                     .Select(x => x.Parameters))
        {
            if (parameters.FirstOrDefault(x => x.Name.Equals(
                    nameof(swaggerDoc.Info.Version),
                    StringComparison.InvariantCultureIgnoreCase)) is { } versionParam)
            {
                parameters.Remove(versionParam);
            }
            else if (parameters.FirstOrDefault(x => x.Name == "api-version") is { } vParam)
            {
                parameters.Remove(vParam);
            }

        }
    }
}