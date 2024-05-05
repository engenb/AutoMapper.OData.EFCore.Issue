using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace LeaseCrunch.GeneralLedger.Infrastructure.Swagger;

internal class ODataPreferHeaderFilter : IDocumentFilter
{
    private static readonly string[] ResponseStatusCodesWithBody = { $"{Status200OK:D}", $"{Status201Created:D}" };

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        foreach (var op in swaggerDoc.Paths.Values
                     .SelectMany(x => x.Operations)
                     .Where(IsOperationMutative)
                     .Where(IsOperationWithResponseBody)
                     .Select(x => x.Value))
        {
            op.Parameters.Add(new OpenApiParameter
            {
                Name = "Prefer",
                In = ParameterLocation.Header,
                Examples = new Dictionary<string, OpenApiExample>
                {
                    { "--", new OpenApiExample{Value = null}},
                    { "representation", new OpenApiExample { Value = new OpenApiString("return=representation") } },
                    { "minimal", new OpenApiExample { Value = new OpenApiString("return=minimal") } }
                },
                Required = false
            });
        }
    }

    private static bool IsOperationMutative(KeyValuePair<OperationType, OpenApiOperation> kvp) =>
        kvp.Key is OperationType.Post or OperationType.Patch or OperationType.Put;

    private static bool IsOperationWithResponseBody(KeyValuePair<OperationType, OpenApiOperation> kvp) =>
        ResponseStatusCodesWithBody.Intersect(kvp.Value.Responses.Select(r => r.Key)).Any();
}